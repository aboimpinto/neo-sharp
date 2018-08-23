using NeoSharp.BinarySerialization;
using NeoSharp.Core.Types;
using Newtonsoft.Json;

namespace NeoSharp.Core.Models.Witnesses
{
    public interface ISignedWitnessBase
    {
        [JsonProperty("txid")]
        UInt160 Hash { get; }

        [BinaryProperty(0, MaxLength = 65536)]
        [JsonProperty("invocation")]
        byte[] InvocationScript { get; }

        [BinaryProperty(1, MaxLength = 65536)]
        [JsonProperty("verification")]
        byte[] VerificationScript { get; }
    }
}
