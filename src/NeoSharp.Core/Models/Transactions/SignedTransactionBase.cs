using System;
using System.Collections.Generic;
using System.Linq;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Types;
using Newtonsoft.Json;

namespace NeoSharp.Core.Models.Transactions
{
    [Serializable]
    public class SignedTransactionBase 
    {
        #region Private Fields 
        private readonly TransactionBase _transactionBase;
        #endregion

        #region Public Properties 
        [BinaryProperty(0)]
        [JsonProperty("hash")]
        public UInt256 Hash { get; }

        [BinaryProperty(1)]
        [JsonProperty("type")]
        public TransactionType Type => this._transactionBase.Type;

        [BinaryProperty(2)]
        [JsonProperty("version")]
        public byte Version => this._transactionBase.Version;

        [BinaryProperty(100)]
        [JsonProperty("attributes")]
        public TransactionAttribute[] Attributes => this._transactionBase.Attributes.ToArray();

        [BinaryProperty(101)]
        [JsonProperty("vin")]
        public CoinReference[] Inputs => this._transactionBase.Inputs.ToArray();

        [BinaryProperty(102)]
        [JsonProperty("vout")]
        public TransactionOutput[] Outputs => this._transactionBase.Outputs.ToArray();

        [BinaryProperty(255)]
        [JsonProperty("witness")]
        public Witnesses.SignedWitness[] Witness { get; }
        #endregion

        #region Constructor 
        public SignedTransactionBase(TransactionBase transactionBase, UInt256 hash, IEnumerable<Witnesses.SignedWitness> witnesses)
        {
            this._transactionBase = transactionBase;

            this.Hash = hash;
            this.Witness = witnesses.ToArray();
        }
        #endregion



        //#region Constructor 
        //public SignedTransactionBase(ITransactionBase transactionBase)
        //{
        //    this._transactionBase = transactionBase;
        //}
        //#endregion

        //#region Virtual Methods 
        //public virtual int SerializeExecusiveData(IBinarySerializer serializer, BinaryWriter writer, BinarySerializerSettings settings = null)
        //{
        //    return 0;
        //}
        //#endregion

        //#region IBinarySerializable implementation 
        //public int Serialize(IBinarySerializer serializer, BinaryWriter writer, BinarySerializerSettings settings = null)
        //{
        //    var serializeResult = 2;

        //    writer.Write((byte)Type);
        //    writer.Write(Version);

        //    // Exclusive transaction data
        //    serializeResult += this.SerializeExecusiveData(serializer, writer, settings);

        //    // Shared transaction data
        //    serializeResult += serializer.Serialize(this.Attributes, writer, settings);
        //    serializeResult += serializer.Serialize(this.Inputs, writer, settings);
        //    serializeResult += serializer.Serialize(this.Outputs, writer, settings);

        //    //// Serialize sign
        //    //if (settings?.Filter?.Invoke(nameof(this.Witness)) != false)
        //    //{
        //    //    serializeResult += serializer.Serialize(this.Witness, writer, settings);
        //    //}

        //    return serializeResult;
        //}
        //#endregion

        //#region Public Methods 
        //public void Sign()
        //{
        //    //// TODO [AboimPinto]: Find a way for the BinarySerializer be injected.
        //    //var serilizationData = BinarySerializer.Default.Serialize(this, new BinarySerializerSettings
        //    //{
        //    //    Filter = x => x != nameof(ISignedTransactionBase.Witness)
        //    //});

        //    //this.Witness = this._transactionBase.Witness.Select(unsignedWitness => unsignedWitness.Sign()).ToArray();

        //    //this.Hash = new UInt256(Crypto.Default.Hash256(serilizationData));
        //}
        //#endregion
    }
}
