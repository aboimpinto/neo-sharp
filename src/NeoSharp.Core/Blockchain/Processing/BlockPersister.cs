using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using NeoSharp.Core.Blockchain.Repositories;
using NeoSharp.Core.Logging;
using NeoSharp.Core.Models;
using NeoSharp.Core.Network;

namespace NeoSharp.Core.Blockchain.Processing
{
    public class BlockPersister : IBlockPersister
    {
        private readonly IBlockRepository _blockRepository;
        private readonly IBlockchainContext _blockchainContext;
        private readonly IBlockHeaderPersister _blockHeaderPersister;
        private readonly ITransactionPersister<Models.Transaction> _transactionPersister;
        private readonly ITransactionPool _transactionPool;
        private readonly ILogger<BlockPersister> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockPersister"/> class.
        /// </summary>
        /// <param name="blockRepository"></param>
        /// <param name="blockchainContext"></param>
        /// <param name="blockHeaderPersister"></param>
        /// <param name="transactionPersister"></param>
        /// <param name="transactionPool"></param>
        /// <param name="logger"></param>
        public BlockPersister(
            IBlockRepository blockRepository,
            IBlockchainContext blockchainContext,
            IBlockHeaderPersister blockHeaderPersister,
            ITransactionPersister<Models.Transaction> transactionPersister,
            ITransactionPool transactionPool,
            ILogger<BlockPersister> logger)
        {
            _blockRepository = blockRepository;
            _blockchainContext = blockchainContext;
            _blockHeaderPersister = blockHeaderPersister;
            _transactionPersister = transactionPersister;
            _transactionPool = transactionPool;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task Persist(params Block[] blocks)
        {
            foreach (var block in blocks)
            {
                if (block.Index == 0)
                {
                    // Persisting genesis block
                    await this.PersistBlock(block);
                    continue;
                }

                using (var transactionScope = new TransactionScope())
                {
                    if (await PersistBlock(block))
                    {
                        transactionScope.Complete();
                    }
                    else
                    {
                        transactionScope.Dispose();
                    }
                }
            }
        }

        private async Task<bool> PersistBlock(Block block)
        {
            var index = await _blockRepository.GetTotalBlockHeight();

            var blockHeader = await _blockRepository.GetBlockHeader(block.Hash);

            if (blockHeader == null ||
                blockHeader.Type == HeaderType.Header && blockHeader.Hash.Equals(block.Hash))
            {
                if (block.GetBlockHeader().Type == HeaderType.Extended && block.Index > 0)
                {
                    await _blockHeaderPersister.Update(block.GetBlockHeader());
                }
                else
                {
                    await _blockHeaderPersister.Persist(block.GetBlockHeader());
                }

                _logger.LogDebug($"The block {block.Index} with hash {block.Hash} was persisted.");

                if (index + 1 == block.Index)
                {
                    await _blockRepository.SetTotalBlockHeight(block.Index);
                    index = block.Index;
                }

                try
                {
                    foreach (var transaction in block.Transactions)
                    {
                        await _transactionPersister.Persist(transaction);
                        _transactionPool.Remove(transaction.Hash);
                    }

                    _blockchainContext.CurrentBlock = block;
                    return true;
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, $"There was an error persisting transaction of the block {block.Hash} with Index {block.Index}");
                }
            }

            return false;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<BlockHeader>> Persist(params BlockHeader[] blockHeaders)
        {
            return await _blockHeaderPersister.Persist(blockHeaders);
        }

        /// <inheritdoc />
        public async Task<bool> IsBlockPersisted(Block block)
        {
            var blockHeader = await _blockRepository.GetBlockHeader(block.Hash);

            if (blockHeader?.Type == HeaderType.Extended)
            {
                _logger.LogDebug($"The block \"{block.Hash.ToString(true)}\" exists already on the blockchain.");
                return true;
            }

            if (blockHeader != null && blockHeader.Hash != block.Hash)
            {
                _logger.LogDebug($"The block \"{block.Hash.ToString(true)}\" has an invalid hash.");       // <-- [AboimPinto] I'm not sure if this validation should be on this method.
                return true;
            }

            return false;
        }
    }
}