namespace NeoSharp.Core.Models
{
    public class UnsignedIssueTransaction : UnsignedTransaction
    {
        #region Constructor 
        public UnsignedIssueTransaction()
        {
            this.Type = TransactionType.IssueTransaction;
        }
        #endregion
    }
}
