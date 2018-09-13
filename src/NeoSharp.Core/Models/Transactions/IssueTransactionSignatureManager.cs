using NeoSharp.BinarySerialization;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Models.Witnesses;

namespace NeoSharp.Core.Models.Transactions
{
    public class IssueTransactionSignatureManager : TransactionSignatureManagerBase, IIssueTransactionSignatureManager
    {
        #region Constructor 
        public IssueTransactionSignatureManager(Crypto crypto, IWitnessSignatureManager witnessSignatureManager, IBinarySerializer binarySerializer, IBinaryDeserializer binaryDeserializer) 
            : base(crypto, witnessSignatureManager, binarySerializer, binaryDeserializer)
        {
        }
        #endregion

        #region IIssueTransactionSignatureManager implementation 
        public SignedIssueTransaction Sign(IssueTransaction issueTransaction)
        {
            return this.Sign<IssueTransaction, SignedIssueTransaction>(issueTransaction);
        }
        #endregion
    }
}