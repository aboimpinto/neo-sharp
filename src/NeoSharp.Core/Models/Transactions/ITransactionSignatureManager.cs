namespace NeoSharp.Core.Models.Transactions
{
    public interface ITransactionSignatureManager
    {
        SignedTransactionBase Sign(TransactionBase transaction);
    }
}