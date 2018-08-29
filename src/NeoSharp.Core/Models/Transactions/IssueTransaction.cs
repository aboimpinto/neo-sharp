namespace NeoSharp.Core.Models.Transactions
{
    public class IssueTransaction : TransactionBase
    {
        #region Construtor 
        public IssueTransaction()
        {
            this.Type = TransactionType.IssueTransaction;
        }
        #endregion
    }
}
