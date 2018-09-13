using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Models.Witnesses;

namespace NeoSharp.Core.Models.Transactions
{
    public class TransactionSignatureManager : ITransactionSignatureManager
    {
        #region Private Fields 
        private readonly Crypto _crypto;
        private readonly IWitnessSignatureManager _witnessSignatureManager;
        private readonly IBinarySerializer _binarySerializer;
        private readonly IBinaryDeserializer _binaryDeserializer;
        #endregion

        #region Constructor 
        public TransactionSignatureManager(
            Crypto crypto, 
            IWitnessSignatureManager witnessSignatureManager, 
            IBinarySerializer binarySerializer,
            IBinaryDeserializer binaryDeserializer)
        {
            this._crypto = crypto;
            this._witnessSignatureManager = witnessSignatureManager;
            this._binarySerializer = binarySerializer;
            this._binaryDeserializer = binaryDeserializer;
        }
        #endregion

        #region ITransactionSignatureManager implementation 
        public SignedTransactionBase Sign(TransactionBase transaction)
        {
            if (transaction.GetType() == typeof(RegisterTransaction))
            {
                return new RegisterTransactionSignatureManager(this._crypto, this._witnessSignatureManager, this._binarySerializer, this._binaryDeserializer)
                    .Sign((RegisterTransaction)transaction);
            }
            else if (transaction.GetType() == typeof(MinerTransaction))
            {
                return new MinerTransactionSignatureManager(this._crypto, this._witnessSignatureManager, this._binarySerializer, this._binaryDeserializer)
                    .Sign((MinerTransaction)transaction);
            }
            else if (transaction.GetType() == typeof(IssueTransaction))
            {
                return new IssueTransactionSignatureManager(this._crypto, this._witnessSignatureManager, this._binarySerializer, this._binaryDeserializer)
                    .Sign((IssueTransaction) transaction);
            }

            throw new NotSupportedException($"The type {transaction.GetType()} doesn't have a SignatureManger.");
        }

        public SignedTransactionBase Deserialize(byte[] rawBlockHeader)
        {
            var transactionDeselializer = new Dictionary<byte, Func<byte[], BinaryReader, TransactionBase>>
            {
                { 0, this.MinerDeserializer },
                { 64, this.RegisterDeserializer }
            };


            using (var ms = new MemoryStream(rawBlockHeader))
            {
                using (var binaryReader = new BinaryReader(ms, Encoding.UTF8, true))
                {
                    var deserializer = transactionDeselializer[binaryReader.ReadByte()];
                    deserializer.Invoke(rawBlockHeader, binaryReader);
                }
            }

            return null;
        }
        #endregion

        #region Private Fields 
        private TransactionBase MinerDeserializer(byte[] rawMinerTransaction, BinaryReader binaryReader)
        {
            return new MinerTransactionSignatureManager(
                this._crypto, 
                this._witnessSignatureManager,
                this._binarySerializer, 
                this._binaryDeserializer).Deserializer(rawMinerTransaction, binaryReader, null);
        }

        private TransactionBase RegisterDeserializer(byte[] rawRegisterTransaction, BinaryReader binaryReader)
        {
            return null;
        }
        #endregion
    }
}
