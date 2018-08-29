using System;
using System.Collections.Generic;
using NeoSharp.Core.Types;

namespace NeoSharp.Core.Models.Transactions
{
    [Serializable]
    public class SignedIssueTransaction : SignedTransactionBase
    {
        #region Constructor 
        public SignedIssueTransaction(IssueTransaction issueTransaction, UInt256 hash, IEnumerable<Witnesses.SignedWitness> witnesses)
            : base(issueTransaction, hash, witnesses)
        {
        }
        #endregion
    }
}
