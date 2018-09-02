using System;
using System.Collections.Generic;
using NeoSharp.Core.Types;

namespace NeoSharp.Core.Models.Transactions
{
    [Serializable]
    public class SignedIssueTransaction : SignedTransactionBase
    {
        #region Constructor 
        public SignedIssueTransaction(
            IssueTransaction issueTransaction, 
            IEnumerable<Witnesses.SignedWitness> witnesses, 
            Func<SignedTransactionBase, UInt256> hashCalculatorMethod)
            : base(issueTransaction, witnesses, hashCalculatorMethod)
        {
            this.Sign();
        }
        #endregion
    }
}
