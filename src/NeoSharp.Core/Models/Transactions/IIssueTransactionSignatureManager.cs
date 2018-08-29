namespace NeoSharp.Core.Models.Transactions
{
    public interface IIssueTransactionSignatureManager
    {
        SignedIssueTransaction Sign(IssueTransaction minerTransaction);
    }
}
