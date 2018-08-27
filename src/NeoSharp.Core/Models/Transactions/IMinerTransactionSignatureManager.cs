namespace NeoSharp.Core.Models.Transactions
{
    public interface IMinerTransactionSignatureManager
    {
        SignedMinerTransaction Sign(MinerTransaction minerTransaction);
    }
}
