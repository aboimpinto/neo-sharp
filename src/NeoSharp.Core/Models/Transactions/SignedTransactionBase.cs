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
        private readonly Func<SignedTransactionBase, UInt256> _hashCalculatorMethod;
        #endregion

        #region Public Properties 
        [BinaryProperty(0)]
        [JsonProperty("hash")]
        public UInt256 Hash { get; private set; }

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
        public SignedTransactionBase(
            TransactionBase transactionBase, 
            IEnumerable<Witnesses.SignedWitness> witnesses,
            Func<SignedTransactionBase, UInt256> hashCalculatorMethod)
        {
            this._transactionBase = transactionBase;
            _hashCalculatorMethod = hashCalculatorMethod;

            this.Witness = witnesses.ToArray();
        }
        #endregion

        #region Protected Methods 
        protected void Sign()
        {
            this.Hash = this._hashCalculatorMethod.Invoke(this);
        }
        #endregion
    }
}
