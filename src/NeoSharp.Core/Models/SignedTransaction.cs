using System;
using NeoSharp.BinarySerialization;
using Newtonsoft.Json;

namespace NeoSharp.Core.Models
{
    public abstract class SignedTransaction : TransactionBase
    {
        #region Public Properties 
        [BinaryProperty(255)]
        [JsonProperty("witness")]
        public SignedWitness[] Witness { get; private set; }
        #endregion

        #region Constructor 
        protected SignedTransaction(UnsignedTransaction unsignedTransaction)
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
