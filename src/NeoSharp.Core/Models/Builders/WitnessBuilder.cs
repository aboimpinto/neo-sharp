using NeoSharp.VM;

namespace NeoSharp.Core.Models.Builders
{
    public class WitnessBuilder
    {
        public UnsignedWitness BuildUnsignedGenesisWitness()
        {
            var unsignedWitness = new UnsignedWitness
            {
                InvocationScript = new byte[0],
                VerificationScript = new[] { (byte)EVMOpCode.PUSH1 }
            };

            return unsignedWitness;
        }
    }
}
