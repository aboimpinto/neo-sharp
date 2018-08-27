﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NeoSharp.BinarySerialization;
using NeoSharp.BinarySerialization.SerializationHooks;
using NeoSharp.Core.Caching;
using NeoSharp.Core.Models;

namespace NeoSharp.Core.Converters
{
    public class SignedTransactionBaseSerializer : IBinaryCustomSerializable
    {
        /// <summary>
        /// Cache
        /// </summary>
        private static readonly ReflectionCache<byte> Cache = ReflectionCache<byte>.CreateFromEnum<TransactionType>();

        public object Deserialize(IBinaryDeserializer deserializer, BinaryReader reader, Type type, BinarySerializerSettings settings = null)
        {
            // Read transaction Type

            //var tx = Cache.CreateInstance<TransactionBase>(reader.ReadByte());

            //tx.Deserialize(deserializer, reader, settings);

            //return tx;
            throw new NotImplementedException();
        }

        public int Serialize(IBinarySerializer serializer, BinaryWriter writer, object value, BinarySerializerSettings settings = null)
        {
            var tx = (IBinarySerializable)value;

            return tx.Serialize(serializer, writer, settings);
        }
    }
}