using System;
using System.IO;
using System.Linq;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Converters;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Types;

namespace NeoSharp.Core.Models.Transactions
{
    [Serializable]
    [BinaryTypeSerializer(typeof(SignedTransactionBaseSerializer))]
    public class SignedTransactionBase : ISignedTransactionBase, IBinarySerializable
    {
        #region Private Fields 
        private readonly ITransactionBase _transactionBase;
        #endregion

        #region ISignedTransactionBase implementation 

        public UInt256 Hash { get; private set; }

        public TransactionType Type => this._transactionBase.Type;

        public byte Version => this._transactionBase.Version; 

        public TransactionAttribute[] Attributes => this._transactionBase.Attributes.ToArray();

        public CoinReference[] Inputs => this._transactionBase.Inputs.ToArray();

        public TransactionOutput[] Outputs => this._transactionBase.Outputs.ToArray();

        public Witnesses.ISignedWitnessBase[] Witness { get; private set; }
        #endregion

        #region Constructor 
        public SignedTransactionBase(ITransactionBase transactionBase)
        {
            this._transactionBase = transactionBase;
        }
        #endregion

        #region Virtual Methods 
        public virtual int SerializeExecusiveData(IBinarySerializer serializer, BinaryWriter writer, BinarySerializerSettings settings = null)
        {
            return 0;
        }
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

        #region Public Methods 
        public void Sign()
        {
            // TODO [AboimPinto]: Find a way for the BinarySerializer be injected.
            var serilizationData = BinarySerializer.Default.Serialize(this, new BinarySerializerSettings
            {
                Filter = x => x != nameof(ISignedTransactionBase.Witness)
            });

            this.Witness = this._transactionBase.Witness.Select(unsignedWitness => unsignedWitness.Sign()).ToArray();

            this.Hash = new UInt256(Crypto.Default.Hash256(serilizationData));
        }
        #endregion
    }
}
