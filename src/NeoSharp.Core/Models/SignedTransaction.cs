using System;
using NeoSharp.BinarySerialization;
using Newtonsoft.Json;

namespace NeoSharp.Core.Models
{
    [Serializable]
    public class SignedTransaction : TransactionBase
    {
        #region Public Properties 
        [BinaryProperty(255)]
        [JsonProperty("witness")]
        public SignedWitness[] Witness;
        #endregion

        #region Constructor 
        public SignedTransaction(UnsignedTransaction unsignedTransaction)
        {
            this.Sign(unsignedTransaction);

            for (var i = 0; i < unsignedTransaction.Witness.Length - 1; i++)
            {
                this.Witness[i] = new SignedWitness(unsignedTransaction.Witness[i]);
            }
        }
        #endregion
    }
}
