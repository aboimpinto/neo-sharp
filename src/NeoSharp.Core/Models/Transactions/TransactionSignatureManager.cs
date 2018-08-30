using System;
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
        #endregion

        #region Constructor 
        public TransactionSignatureManager(Crypto crypto, IWitnessSignatureManager witnessSignatureManager, IBinarySerializer binarySerializer)
        {
            _crypto = crypto;
            _witnessSignatureManager = witnessSignatureManager;
            _binarySerializer = binarySerializer;
        }
        #endregion

        #region ITransactionSignatureManager implementation 
        public SignedTransactionBase Sign(TransactionBase transaction)
        {
            if (transaction.GetType() == typeof(RegisterTransaction))
            {
                return new RegisterTransactionSignatureManager(this._crypto, this._witnessSignatureManager, this._binarySerializer)
                    .Sign((RegisterTransaction)transaction);
            }
            else if (transaction.GetType() == typeof(MinerTransaction))
            {
                return new MinerTransactionSignatureManager(this._crypto, this._witnessSignatureManager, this._binarySerializer)
                    .Sign((MinerTransaction)transaction);
            }
            else if (transaction.GetType() == typeof(IssueTransaction))
            {
                return new IssueTransactionSignatureManager(this._crypto, this._witnessSignatureManager, this._binarySerializer)
                    .Sign((IssueTransaction) transaction);
            }

            throw new NotSupportedException($"The type {transaction.GetType()} doesn't have a SignatureManger.");
        }
        #endregion
    }
}
