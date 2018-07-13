﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Models;
using NeoSharp.Core.Persistence;
using NeoSharp.Core.Types;

namespace NeoSharp.Persistence.RocksDB
{
    public class RocksDbRepository : IRepository, IDisposable
    {
        #region Private Fields

        private readonly IRocksDbContext _rocksDbContext;
        private readonly IBinarySerializer _serializer;
        private readonly IBinaryDeserializer _deserializer;

        private readonly byte[] _sysCurrentBlockKey = {(byte) DataEntryPrefix.SysCurrentBlock};
        private readonly byte[] _sysCurrentBlockHeaderKey = {(byte) DataEntryPrefix.SysCurrentHeader};
        private readonly byte[] _sysCurrentIndexHeightKey = {(byte) DataEntryPrefix.IxIndexHeight};

        #endregion

        #region Constructor

        public RocksDbRepository
        (
            IRocksDbContext rocksDbContext,
            IBinarySerializer serializer,
            IBinaryDeserializer deserializer
        )
        {
            _rocksDbContext = rocksDbContext ?? throw new ArgumentNullException(nameof(rocksDbContext));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
        }

        #endregion

        #region IRepository Data Members

        public async Task<UInt256> GetBlockHashFromHeight(uint height)
        {
            var hash = await _rocksDbContext.Get(height.BuildIxHeightToHashKey());
            return hash == null || hash.Length == 0 ? UInt256.Zero : new UInt256(hash);
        }

        public async Task AddBlockHeader(BlockHeaderBase blockHeader)
        {
            await _rocksDbContext.Save(blockHeader.Hash.BuildDataBlockKey(), _serializer.Serialize(blockHeader));
            await _rocksDbContext.Save(blockHeader.Index.BuildIxHeightToHashKey(), blockHeader.Hash.ToArray());
        }

        public async Task AddTransaction(Transaction transaction)
        {
            await _rocksDbContext.Save(transaction.Hash.BuildDataTransactionKey(), _serializer.Serialize(transaction));
        }

        public async Task<BlockHeaderBase> GetBlockHeader(UInt256 hash)
        {
            var rawHeader = await _rocksDbContext.Get(hash.BuildDataBlockKey());

            return rawHeader == null ? null : _deserializer.Deserialize<BlockHeaderBase>(rawHeader);
        }

        public async Task<BlockHeader> GetBlockHeaderExtended(UInt256 hash)
        {
            var rawHeader = await _rocksDbContext.Get(hash.BuildDataBlockKey());

            return rawHeader == null ? null : _deserializer.Deserialize<BlockHeader>(rawHeader);
        }

        public async Task<uint> GetTotalBlockHeight()
        {
            var raw = await _rocksDbContext.Get(_sysCurrentBlockKey);

            return raw == null ? uint.MinValue : BitConverter.ToUInt32(raw, 0);
        }

        public async Task SetTotalBlockHeight(uint height)
        {
            await _rocksDbContext.Save(_sysCurrentBlockKey, BitConverter.GetBytes(height));
        }

        public async Task<uint> GetTotalBlockHeaderHeight()
        {
            var raw = await _rocksDbContext.Get(_sysCurrentBlockHeaderKey);

            return raw == null ? uint.MinValue : BitConverter.ToUInt32(raw, 0);
        }

        public async Task SetTotalBlockHeaderHeight(uint height)
        {
            await _rocksDbContext.Save(_sysCurrentBlockHeaderKey, BitConverter.GetBytes(height));
        }

        public async Task<Transaction> GetTransaction(UInt256 hash)
        {
            var rawTransaction = await _rocksDbContext.Get(hash.BuildDataTransactionKey());

            return rawTransaction == null ? null : _deserializer.Deserialize<Transaction>(rawTransaction);
        }

        #endregion

        #region IRepository State Members

        public async Task<Account> GetAccount(UInt160 hash)
        {
            var raw = await _rocksDbContext.Get(hash.BuildStateAccountKey());
            return raw == null
                ? null
                : _deserializer.Deserialize<Account>(raw);
        }

        public async Task AddAccount(Account acct)
        {
            await _rocksDbContext.Save(acct.ScriptHash.BuildStateAccountKey(), _serializer.Serialize(acct));
        }

        public async Task DeleteAccount(UInt160 hash)
        {
            await _rocksDbContext.Delete(hash.BuildStateAccountKey());
        }

        public async Task<CoinState[]> GetCoinStates(UInt256 txHash)
        {
            var raw = await _rocksDbContext.Get(txHash.BuildStateCoinKey());
            return raw == null
                ? null
                : _deserializer.Deserialize<CoinState[]>(raw);
        }

        public async Task AddCoinStates(UInt256 txHash, CoinState[] coinstates)
        {
            await _rocksDbContext.Save(txHash.BuildStateCoinKey(), _serializer.Serialize(coinstates));
        }

        public async Task DeleteCoinStates(UInt256 txHash)
        {
            await _rocksDbContext.Delete(txHash.BuildStateCoinKey());
        }

        public async Task<Validator> GetValidator(ECPoint publicKey)
        {
            var raw = await _rocksDbContext.Get(publicKey.BuildStateValidatorKey());
            return raw == null
                ? null
                : _deserializer.Deserialize<Validator>(raw);
        }

        public async Task AddValidator(Validator validator)
        {
            await _rocksDbContext.Save(validator.PublicKey.BuildStateValidatorKey(), _serializer.Serialize(validator));
        }

        public async Task DeleteValidator(ECPoint point)
        {
            await _rocksDbContext.Delete(point.BuildStateValidatorKey());
        }

        public async Task<Contract> GetContract(UInt160 contractHash)
        {
            var raw = await _rocksDbContext.Get(contractHash.BuildStateContractKey());
            return raw == null
                ? null
                : _deserializer.Deserialize<Contract>(raw);
        }

        public async Task AddContract(Contract contract)
        {
            await _rocksDbContext.Save(contract.ScriptHash.BuildStateContractKey(), _serializer.Serialize(contract));
        }

        public async Task DeleteContract(UInt160 contractHash)
        {
            await _rocksDbContext.Delete(contractHash.BuildStateContractKey());
        }

        public async Task<StorageValue> GetStorage(StorageKey key)
        {
            var raw = await _rocksDbContext.Get(key.BuildStateStorageKey());
            return raw == null
                ? null
                : _deserializer.Deserialize<StorageValue>(raw);
        }

        public async Task AddStorage(StorageKey key, StorageValue val)
        {
            await _rocksDbContext.Save(key.BuildStateStorageKey(), val.Value);
        }

        public async Task DeleteStorage(StorageKey key)
        {
            await _rocksDbContext.Delete(key.BuildStateStorageKey());
        }

        #endregion

        #region IRepository Index Members

        public async Task<uint> GetIndexHeight()
        {
            var raw = await _rocksDbContext.Get(_sysCurrentIndexHeightKey);
            return raw == null ? uint.MinValue : BitConverter.ToUInt32(raw, 0);
        }

        public async Task SetIndexHeight(uint height)
        {
            await _rocksDbContext.Save(_sysCurrentIndexHeightKey, BitConverter.GetBytes(height));
        }

        public async Task<HashSet<CoinReference>> GetIndexConfirmed(UInt160 hash)
        {
            var raw = await _rocksDbContext.Get(hash.BuildIndexConfirmedKey());
            return raw == null
                ? new HashSet<CoinReference>()
                : _deserializer.Deserialize<HashSet<CoinReference>>(raw);
        }

        public async Task SetIndexConfirmed(UInt160 hash, HashSet<CoinReference> coinReferences)
        {
            var bytes = _serializer.Serialize(coinReferences);
            await _rocksDbContext.Save(hash.BuildIndexConfirmedKey(), bytes);
        }

        public async Task<HashSet<CoinReference>> GetIndexClaimable(UInt160 hash)
        {
            var raw = await _rocksDbContext.Get(hash.BuildIndexClaimableKey());
            return raw == null
                ? new HashSet<CoinReference>()
                : _deserializer.Deserialize<HashSet<CoinReference>>(raw);
        }

        public async Task SetIndexClaimable(UInt160 hash, HashSet<CoinReference> coinReferences)
        {
            var bytes = _serializer.Serialize(coinReferences);
            await _rocksDbContext.Save(hash.BuildIndexClaimableKey(), bytes);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _rocksDbContext.Dispose();
        }

        #endregion
    }
}