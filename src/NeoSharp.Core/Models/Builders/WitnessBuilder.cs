using NeoSharp.VM;

namespace NeoSharp.Core.Models.Builders
{
    public class WitnessBuilder
    {
        public Witnesses.Witness BuildGenesisWitness()
        {
            var unsignedWitness = new Witnesses.Witness
            {
                InvocationScript = new byte[0],
                VerificationScript = new[] { (byte)EVMOpCode.PUSH1 }
            };

            return unsignedWitness;
        }
    }
}
