using System;
using NeoSharp.BinarySerialization;
using Newtonsoft.Json;

namespace NeoSharp.Core.Models
{
    [Serializable]
    public class SignedBlockHeader : BlockBase
    {
        #region Public Properties 
        [BinaryProperty(8)]
        [JsonProperty("script")]
        public SignedWitness Witness { get; }
        #endregion

        #region Constructor 
        public SignedBlockHeader(UnsignedBlockHeader unsignedBlockHeader)
        {
            this.Sign(unsignedBlockHeader);

            this.Witness = new SignedWitness(unsignedBlockHeader.Witness);
        }
        #endregion
    }
}
