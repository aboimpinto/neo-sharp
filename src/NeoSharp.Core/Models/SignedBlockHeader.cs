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
            // TODO [AboimPinto]: In the old logic this was after the creation of the Hash. Creating new Witness object will produce a different hash on the BlockHeader. This need to be verified.
            this.Witness = new SignedWitness(unsignedBlockHeader.Witness);

            var signingSettings = BinarySerializer.Default.Serialize(this, new BinarySerializerSettings()
            {
                Filter = x => 
                    x != nameof(this.Witness) &&
                    x != nameof(this.Type)              // TODO [AboimPinto]: Previous logic had the TransactionHashes here, but the blockHeader don't have transactions. This need to be verified.
            });

            this.Sign(unsignedBlockHeader, signingSettings, null);      // TODO [AboimPinto]: Verify the MerkleRoot because in the BlockHeader there aren't transactions or transaction hashes.
        }
        #endregion
    }
}
