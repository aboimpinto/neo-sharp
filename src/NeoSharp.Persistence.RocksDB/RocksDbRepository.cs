using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Models;
using NeoSharp.Core.Persistence;
using NeoSharp.Types;

namespace NeoSharp.Persistence.RocksDB
{
    public class RocksDbRepository : IRepository, IDisposable
    {
        private readonly IRocksDbContext _rocksDbContext;
        private readonly IBinarySerializer _binarySerializer;

        private readonly byte[] _sysCurrentBlockKey = { (byte)DataEntryPrefix.SysCurrentBlock };
        private readonly byte[] _sysCurrentBlockHeaderKey = { (byte)DataEntryPrefix.SysCurrentHeader };
        private readonly byte[] _sysVersionKey = { (byte)DataEntryPrefix.SysVersion };
        private readonly byte[] _indexHeightKey = { (byte)DataEntryPrefix.IxIndexHeight };

        private bool _disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="RocksDbRepository"/> class.
        /// </summary>
        /// <param name="rocksDbContext">The RocksDb context.</param>
        /// <param name="binarySerializer">The binary serializer.</param>
        public RocksDbRepository(
            IRocksDbContext rocksDbContext,
            IBinarySerializer binarySerializer)
        {
            _rocksDbContext = rocksDbContext ?? throw new ArgumentNullException(nameof(rocksDbContext));
            _binarySerializer = binarySerializer ?? throw new ArgumentNullException(nameof(binarySerializer));
        }

        /// <inheritdoc />
        public async Task<uint> GetTotalBlockHeight()
        {
            var raw = await _rocksDbContext.Get(_sysCurrentBlockKey);
            return raw == null ? uint.MinValue : BitConverter.ToUInt32(raw, 0);
        }

        /// <inheritdoc />
        public async Task SetTotalBlockHeight(uint height)
        {
            await _rocksDbContext.Save(_sysCurrentBlockKey, BitConverter.GetBytes(height));
        }

        /// <inheritdoc />
        public async Task<uint> GetTotalBlockHeaderHeight()
        {
            var raw = await _rocksDbContext.Get(_sysCurrentBlockHeaderKey);
            return raw == null ? uint.MinValue : BitConverter.ToUInt32(raw, 0);
        }

        /// <inheritdoc />
        public async Task SetTotalBlockHeaderHeight(uint height)
        {
            await _rocksDbContext.Save(_sysCurrentBlockHeaderKey, BitConverter.GetBytes(height));
        }

        /// <inheritdoc />
        public async Task<string> GetVersion()
        {
            var raw = await _rocksDbContext.Get(_sysVersionKey);
            return raw == null ? null : _binarySerializer.Deserialize<string>(raw);
        }

        /// <inheritdoc />
        public async Task SetVersion(string version)
        {
            await _rocksDbContext.Save(_sysVersionKey, _binarySerializer.Serialize(version));
        }

        /// <inheritdoc />
        public async Task<UInt256> GetBlockHashFromHeight(uint height)
        {
            var hash = await _rocksDbContext.Get(height.BuildIxHeightToHashKey());
            return hash == null || hash.Length == 0 ? UInt256.Zero : new UInt256(hash);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<UInt256>> GetBlockHashesFromHeights(IEnumerable<uint> heights)
        {
            var heightsHashes = await _rocksDbContext.GetMany(heights.Select(h => h.BuildIxHeightToHashKey()));
            return heightsHashes.Values.Where(h => h != null && h.Length == UInt256.Zero.Size).Select(h => new UInt256(h));
        }

        /// <inheritdoc />
        public async Task AddBlockHeader(BlockHeader blockHeader)
        {
            await _rocksDbContext.Save(blockHeader.Hash.BuildDataBlockKey(), _binarySerializer.Serialize(blockHeader));
            await _rocksDbContext.Save(blockHeader.Index.BuildIxHeightToHashKey(), blockHeader.Hash.ToArray());
        }

        /// <inheritdoc />
        public async Task AddTransaction(Transaction transaction)
        {
            await _rocksDbContext.Save(transaction.Hash.BuildDataTransactionKey(), _binarySerializer.Serialize(transaction));
        }

        /// <inheritdoc />
        public async Task<BlockHeader> GetBlockHeader(UInt256 hash)
        {
            var rawHeader = await _rocksDbContext.Get(hash.BuildDataBlockKey());
            return rawHeader == null ? null : _binarySerializer.Deserialize<BlockHeader>(rawHeader);
        }

        /// <inheritdoc />
        public async Task<Transaction> GetTransaction(UInt256 hash)
        {
            var rawTransaction = await _rocksDbContext.Get(hash.BuildDataTransactionKey());
            return rawTransaction == null ? null : _binarySerializer.Deserialize<Transaction>(rawTransaction);
        }

        /// <inheritdoc />
        public async Task<Account> GetAccount(UInt160 hash)
        {
            var raw = await _rocksDbContext.Get(hash.BuildStateAccountKey());
            return raw == null
                ? null
                : _binarySerializer.Deserialize<Account>(raw);
        }

        /// <inheritdoc />
        public async Task AddAccount(Account acct)
        {
            await _rocksDbContext.Save(acct.ScriptHash.BuildStateAccountKey(), _binarySerializer.Serialize(acct));
        }

        /// <inheritdoc />
        public async Task DeleteAccount(UInt160 hash)
        {
            await _rocksDbContext.Delete(hash.BuildStateAccountKey());
        }

        /// <inheritdoc />
        public async Task<CoinState[]> GetCoinStates(UInt256 txHash)
        {
            var raw = await _rocksDbContext.Get(txHash.BuildStateCoinKey());
            return raw == null
                ? null
                : _binarySerializer.Deserialize<CoinState[]>(raw);
        }

        /// <inheritdoc />
        public async Task AddCoinStates(UInt256 txHash, CoinState[] coinstates)
        {
            await _rocksDbContext.Save(txHash.BuildStateCoinKey(), _binarySerializer.Serialize(coinstates));
        }

        /// <inheritdoc />
        public async Task DeleteCoinStates(UInt256 txHash)
        {
            await _rocksDbContext.Delete(txHash.BuildStateCoinKey());
        }

        /// <inheritdoc />
        public async Task<Validator> GetValidator(ECPoint publicKey)
        {
            var raw = await _rocksDbContext.Get(publicKey.BuildStateValidatorKey());
            return raw == null
                ? null
                : _binarySerializer.Deserialize<Validator>(raw);
        }

        /// <inheritdoc />
        public async Task AddValidator(Validator validator)
        {
            await _rocksDbContext.Save(validator.PublicKey.BuildStateValidatorKey(), _binarySerializer.Serialize(validator));
        }

        /// <inheritdoc />
        public async Task DeleteValidator(ECPoint point)
        {
            await _rocksDbContext.Delete(point.BuildStateValidatorKey());
        }

        /// <inheritdoc />
        public async Task<Asset> GetAsset(UInt256 assetId)
        {
            var raw = await _rocksDbContext.Get(assetId.BuildStateAssetKey());
            return raw == null ? null : _binarySerializer.Deserialize<Asset>(raw);
        }

        /// <inheritdoc />
        public async Task AddAsset(Asset asset)
        {
            await _rocksDbContext.Save(asset.Id.BuildStateAssetKey(), _binarySerializer.Serialize(asset));
        }

        /// <inheritdoc />
        public async Task DeleteAsset(UInt256 assetId)
        {
            await _rocksDbContext.Delete(assetId.BuildStateAssetKey());
        }

        /// <inheritdoc />
        public async Task<Contract> GetContract(UInt160 contractHash)
        {
            var raw = await _rocksDbContext.Get(contractHash.BuildStateContractKey());
            return raw == null
                ? null
                : _binarySerializer.Deserialize<Contract>(raw);
        }

        /// <inheritdoc />
        public async Task AddContract(Contract contract)
        {
            await _rocksDbContext.Save(contract.ScriptHash.BuildStateContractKey(), _binarySerializer.Serialize(contract));
        }

        /// <inheritdoc />
        public async Task DeleteContract(UInt160 contractHash)
        {
            await _rocksDbContext.Delete(contractHash.BuildStateContractKey());
        }

        /// <inheritdoc />
        public async Task<StorageValue> GetStorage(StorageKey key)
        {
            var raw = await _rocksDbContext.Get(key.BuildStateStorageKey());
            return raw == null
                ? null
                : _binarySerializer.Deserialize<StorageValue>(raw);
        }

        /// <inheritdoc />
        public async Task AddStorage(StorageKey key, StorageValue val)
        {
            await _rocksDbContext.Save(key.BuildStateStorageKey(), val.Value);
        }

        /// <inheritdoc />
        public async Task DeleteStorage(StorageKey key)
        {
            await _rocksDbContext.Delete(key.BuildStateStorageKey());
        }

        /// <inheritdoc />
        public async Task<uint> GetIndexHeight()
        {
            var raw = await _rocksDbContext.Get(_indexHeightKey);
            return raw == null ? uint.MinValue : BitConverter.ToUInt32(raw, 0);
        }

        /// <inheritdoc />
        public async Task SetIndexHeight(uint height)
        {
            await _rocksDbContext.Save(_indexHeightKey, BitConverter.GetBytes(height));
        }

        /// <inheritdoc />
        public async Task<HashSet<CoinReference>> GetIndexConfirmed(UInt160 hash)
        {
            var raw = await _rocksDbContext.Get(hash.BuildIndexConfirmedKey());
            return raw == null
                ? new HashSet<CoinReference>()
                : _binarySerializer.Deserialize<HashSet<CoinReference>>(raw);
        }

        /// <inheritdoc />
        public async Task SetIndexConfirmed(UInt160 hash, HashSet<CoinReference> coinReferences)
        {
            var bytes = _binarySerializer.Serialize(coinReferences);
            await _rocksDbContext.Save(hash.BuildIndexConfirmedKey(), bytes);
        }

        /// <inheritdoc />
        public async Task<HashSet<CoinReference>> GetIndexClaimable(UInt160 hash)
        {
            var raw = await _rocksDbContext.Get(hash.BuildIndexClaimableKey());
            return raw == null
                ? new HashSet<CoinReference>()
                : _binarySerializer.Deserialize<HashSet<CoinReference>>(raw);
        }

        /// <inheritdoc />
        public async Task SetIndexClaimable(UInt160 hash, HashSet<CoinReference> coinReferences)
        {
            var bytes = _binarySerializer.Serialize(coinReferences);
            await _rocksDbContext.Save(hash.BuildIndexClaimableKey(), bytes);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _rocksDbContext.Dispose();
            }

            _disposed = true;
        }
    }
}
