using System.IO;
using NeoSharp.BinarySerialization;

namespace NeoSharp.Core.Models.Transactions
{
    public interface IMinerTransactionSignatureManager
    {
        SignedMinerTransaction Sign(MinerTransaction minerTransaction);

        MinerTransaction Deserializer(byte[] rawMinerTransaction, BinaryReader binaryReader, BinarySerializerSettings settings);
    }
}
