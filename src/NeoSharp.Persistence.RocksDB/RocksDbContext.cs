using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using RocksDbSharp;

namespace NeoSharp.Persistence.RocksDB
{
    public class RocksDbContext : IRocksDbContext, IEnlistmentNotification
    {
        private readonly RocksDb _rocksDb;

        private bool _disposed = false;

        private WriteBatch _currentWriteBatch = null;

        public RocksDbContext(RocksDbConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            // Initialize RocksDB (Connection String is the path to use)
            var options = new DbOptions().SetCreateIfMissing();

            // TODO #358: please avoid sync IO in constructor -> Open connection with the first operation for now
            _rocksDb = RocksDb.Open(options, config.FilePath);
        }

        /// <inheritdoc />
        public Task<byte[]> Get(byte[] key)
        {
            return Task.FromResult(_rocksDb.Get(key));
        }

        /// <inheritdoc />
        public Task<IDictionary<byte[], byte[]>> GetMany(IEnumerable<byte[]> keys)
        {
            return Task.FromResult<IDictionary<byte[], byte[]>>(_rocksDb.MultiGet(keys.ToArray())
                .ToDictionary(kv => kv.Key, k => k.Value));
        }

        /// <inheritdoc />
        public Task Save(byte[] key, byte[] content)
        {
            if (Transaction.Current == null)
            {
                _rocksDb.Put(key, content);
            }
            else
            {
                if (_currentWriteBatch == null)
                {
                    System.Transactions.Transaction.Current.EnlistVolatile(this, EnlistmentOptions.None);
                    _currentWriteBatch = new WriteBatch();
                }

                _currentWriteBatch.Put(key, content);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task Delete(byte[] key)
        {
            if (Transaction.Current == null)
            {
                _rocksDb.Remove(key);
            }
            else
            {
                if (_currentWriteBatch == null)
                {
                    System.Transactions.Transaction.Current.EnlistVolatile(this, EnlistmentOptions.None);
                    _currentWriteBatch = new WriteBatch();
                }

                _currentWriteBatch.Delete(key);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
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
                _rocksDb?.Dispose();
            }

            _disposed = true;
        }

        /// <inheritdoc />
        public void Commit(Enlistment enlistment)
        {
            if (_currentWriteBatch != null)
            {
                _rocksDb.Write(_currentWriteBatch);
                enlistment.Done();
            }

            _currentWriteBatch.Dispose();
            _currentWriteBatch = null;
        }

        /// <inheritdoc />
        public void InDoubt(Enlistment enlistment)
        {
            enlistment.Done();
        }

        /// <inheritdoc />
        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            preparingEnlistment.Prepared();
        }

        /// <inheritdoc />
        public void Rollback(Enlistment enlistment)
        {
            _currentWriteBatch.Dispose();
            _currentWriteBatch = null;
            enlistment.Done();
        }
    }
}