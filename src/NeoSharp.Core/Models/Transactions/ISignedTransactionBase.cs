using NeoSharp.BinarySerialization;
using NeoSharp.Core.Types;
using Newtonsoft.Json;

namespace NeoSharp.Core.Models.Transactions
{
    public interface ISignedTransactionBase
    {
        [BinaryProperty(0)]
        [JsonProperty("hash")]
        UInt256 Hash { get; }

        [BinaryProperty(1)]
        [JsonProperty("type")]
        TransactionType Type { get; }

        [BinaryProperty(2)]
        [JsonProperty("version")]
        byte Version { get; }

        [BinaryProperty(100)]
        [JsonProperty("attributes")]
        TransactionAttribute[] Attributes { get; }

        [BinaryProperty(101)]
        [JsonProperty("vin")]
        CoinReference[] Inputs { get; }

        [BinaryProperty(102)]
        [JsonProperty("vout")]
        TransactionOutput[] Outputs { get; }

        [BinaryProperty(255)]
        [JsonProperty("witness")]
        Witnesses.ISignedWitnessBase[] Witness { get; }
    }
}
