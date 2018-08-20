using System;

namespace NeoSharp.Core.Models.Factories
{
    public class SignedTransactionFactory
    {
        #region Public Methods 
        public SignedTransaction GetSignedTransaction(TransactionBase transactionBase)
        {
            if (transactionBase.GetType() == typeof(UnsignedRegisterTransaction))
            {
                return new SignedRegisterTransaction((UnsignedRegisterTransaction)transactionBase);
            }
            if (transactionBase.GetType() == typeof(SignedRegisterTransaction))
            {
                return (SignedRegisterTransaction)transactionBase;
            }
            else if (transactionBase.GetType() == typeof(UnsignedMinerTransaction))
            {
                // TODO [AboimPinto]: Return SignedMinnerTransaction
            }
            else if (transactionBase.GetType() == typeof(UnsignedIssueTransaction))
            {
                // TODO [AboimPinto: Return SignedIssueTransaction]
            }

            throw new InvalidOperationException("SignedTransactionFactory could not find the signed transaction type for the unsigned transaction provided.");
        }
        #endregion
    }
}
