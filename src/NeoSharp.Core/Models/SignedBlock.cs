using System;
using NeoSharp.BinarySerialization;
using Newtonsoft.Json;

namespace NeoSharp.Core.Models
{
    [Serializable]
    public class SignedBlock : BlockBase
    {
        #region Public Properties 
        [BinaryProperty(8)]
        [JsonProperty("script")]
        public SignedWitness Witness { get; }

        [BinaryProperty(100, MaxLength = 0x10000, Override = true)]
        public SignedTransaction[] Transactions;
        #endregion

        #region Constructor 
        public SignedBlock(UnsignedBlock unsignedBlock)
        {
            this.Sign(unsignedBlock);

            for (var i = 0; i < unsignedBlock.TransactionCount - 1; i++)
            {
                Transactions[i] = new SignedTransaction(unsignedBlock.Transactions[i]);
            }

            this.Witness = new SignedWitness(unsignedBlock.Witness);
        }
        #endregion
    }
}
