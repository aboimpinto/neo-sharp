namespace NeoSharp.Core.Models.Witnesses
{
    public class Witness :  IWitnessBase
    {
        public byte[] InvocationScript { get; set; }

        public byte[] VerificationScript { get; set; }

        public ISignedWitnessBase Sign()
        {
            return new SignedWitness(this);
        }
    }
}
