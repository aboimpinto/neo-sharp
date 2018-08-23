using System.IO;
using NeoSharp.BinarySerialization;
using Newtonsoft.Json;

namespace NeoSharp.Core.Models
{
    public abstract class SignedTransaction : TransactionBase, IBinarySerializable      // TODO: Maybe the combination of this two on the SignedTransactionBase abstract class.
    {
        #region Public Properties 
        [BinaryProperty(255)]
        [JsonProperty("witness")]
        public SignedWitness[] Witness { get; private set; }
        #endregion

        #region IBinarySerializable implementation
        public int Serialize(IBinarySerializer serializer, BinaryWriter writer, BinarySerializerSettings settings = null)
        {
            var serializeResult = 2;

            writer.Write((byte)Type);
            writer.Write(Version);

            // Exclusive transaction data
            serializeResult += this.SerializeExecusiveData(serializer, writer, settings);

            // Shared transaction data
            serializeResult += serializer.Serialize(this.Attributes, writer, settings);
            serializeResult += serializer.Serialize(this.Inputs, writer, settings);
            serializeResult += serializer.Serialize(this.Outputs, writer, settings);

            // Serialize sign
            if (settings?.Filter?.Invoke(nameof(this.Witness)) != false)
            {
                serializeResult += serializer.Serialize(this.Witness, writer, settings);
            }

            return serializeResult;
        }
        #endregion

        #region Public Virtual Methods
        protected virtual int SerializeExecusiveData(IBinarySerializer serializer, BinaryWriter writer, BinarySerializerSettings settings = null)
        {
            return 0;
        }
        #endregion

        #region Protected Methods
        public void SignTransaction(UnsignedTransaction unsignedTransaction)
        {
            this.Witness = new SignedWitness[unsignedTransaction.Witness.Length];
            for (var i = 0; i < unsignedTransaction.Witness.Length; i++)
            {
                this.Witness[i] = new SignedWitness(unsignedTransaction.Witness[i]);
            }

            var signingSettings = BinarySerializer.Default.Serialize(this, new BinarySerializerSettings
            {
                Filter = x => x != nameof(this.Witness)
            });

            this.Sign(unsignedTransaction, signingSettings);
        }
        #endregion
    }
}
