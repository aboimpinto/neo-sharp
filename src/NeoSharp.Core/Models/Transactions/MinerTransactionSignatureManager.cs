using System.IO;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Models.Witnesses;

namespace NeoSharp.Core.Models.Transactions
{
    public class MinerTransactionSignatureManager : TransactionSignatureManagerBase, IMinerTransactionSignatureManager
    {
        #region Constructor 
        public MinerTransactionSignatureManager(
            Crypto crypto, 
            IWitnessSignatureManager witnessSignatureManager, 
            IBinarySerializer binarySerializer, 
            IBinaryDeserializer binaryDeserializer)
            : base(crypto, witnessSignatureManager, binarySerializer, binaryDeserializer)
        {
        }
        #endregion

        #region IMinerTransactionSignatureManager implementation 
        public SignedMinerTransaction Sign(MinerTransaction minerTransaction)
        {
            return this.Sign<MinerTransaction, SignedMinerTransaction>(minerTransaction);
        }

        public MinerTransaction Deserializer(byte[] rawMinerTransaction, BinaryReader binaryReader, BinarySerializerSettings serializerSettings)
        {
            return this.Deserialize<MinerTransaction>(rawMinerTransaction, binaryReader, serializerSettings);
        }
        #endregion

        #region Override Methods
        public override int SerializeExecusiveData(SignedTransactionBase transactionBase, BinaryWriter binaryWriter, BinarySerializerSettings settings = null)
        {
            var minerTransaction = (SignedMinerTransaction) transactionBase;

            binaryWriter.Write(minerTransaction.Nonce);
            return 4;
        }

        public override void DeserializeExclusiveData(BinaryReader binaryReader, TransactionBase transaction)
        {
            var minerTransaction = (MinerTransaction) transaction;
            minerTransaction.Nonce = binaryReader.ReadUInt32();
        }
        #endregion
    }
}
