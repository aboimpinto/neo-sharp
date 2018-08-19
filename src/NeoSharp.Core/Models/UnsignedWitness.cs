using NeoSharp.Core.Types;

namespace NeoSharp.Core.Models
{
    public class UnsignedWitness : WitnessBase
    {
        public new UInt160 Hash { get; set; }

        public new byte[] InvocationScript { get; set; }

        public new byte[] VerificationScript { get; set; }
    }
}
