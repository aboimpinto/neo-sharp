namespace NeoSharp.Core.Models.Witnesses
{
    public interface IWitnessSignatureManager
    {
        SignedWitness SignWitness(Witness witness);
    }
}