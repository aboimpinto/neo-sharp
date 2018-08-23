namespace NeoSharp.Core.Models.Transactions
{
    public interface IRegisterTransactionSignatureManager
    {
        SignedRegisterTransaction Sign(RegisterTransaction registerTransaction);
    }
}
