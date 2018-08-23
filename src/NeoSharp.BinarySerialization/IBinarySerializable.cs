using System.IO;

namespace NeoSharp.BinarySerialization
{
    public interface IBinarySerializable
    {
        int Serialize(IBinarySerializer serializer, BinaryWriter writer, BinarySerializerSettings settings = null);
    }
}
