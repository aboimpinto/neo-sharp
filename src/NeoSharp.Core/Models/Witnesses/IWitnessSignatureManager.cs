namespace NeoSharp.Core.Models.Witnesses
{
    public interface IWitnessSignatureManager
    {
        SignedWitness Sign(Witness witness);
    }
}