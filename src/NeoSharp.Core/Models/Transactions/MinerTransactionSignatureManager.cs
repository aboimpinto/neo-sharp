using System.IO;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Models.Witnesses;

namespace NeoSharp.Core.Models.Transactions
{
    public class MinerTransactionSignatureManager : TransactionSignatureManagerBase, IMinerTransactionSignatureManager
    {
        #region Constructor 
        public MinerTransactionSignatureManager(Crypto crypto, IWitnessSignatureManager witnessSignatureManager, IBinarySerializer binarySerializer)
            : base(crypto, witnessSignatureManager, binarySerializer)
        {
        }
        #endregion

        #region IMinerTransactionSignatureManager implementation 
        public SignedMinerTransaction Sign(MinerTransaction minerTransaction)
        {
            return this.Sign<MinerTransaction, SignedMinerTransaction>(minerTransaction);
        }
        #endregion

        #region Override Methods
        public override int SerializeExecusiveData(SignedTransactionBase transactionBase, BinaryWriter binaryWriter, BinarySerializerSettings settings = null)
        {
            var minerTransaction = (SignedMinerTransaction) transactionBase;

            binaryWriter.Write(minerTransaction.Nonce);
            return 4;
        }
        #endregion
    }
}
