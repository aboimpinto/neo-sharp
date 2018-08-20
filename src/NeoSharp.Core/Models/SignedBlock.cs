using System;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Models.Factories;
using NeoSharp.Core.Types;
using Newtonsoft.Json;

namespace NeoSharp.Core.Models
{
    [Serializable]
    public class SignedBlock : BlockBase
    {
        #region Public Properties 
        [BinaryProperty(2)]
        [JsonProperty("merkleroot")]
        public UInt256 MerkleRoot { get; }

        [BinaryProperty(8)]
        [JsonProperty("script")]
        public SignedWitness Witness { get; }

        [BinaryProperty(100)]
        [JsonProperty("txhashes")]
        public UInt256[] TransactionHashes { get; }

        [BinaryProperty(100, MaxLength = 0x10000, Override = true)]
        public SignedTransaction[] Transactions { get; }

        [JsonProperty("txcount")] public int TransactionCount => TransactionHashes?.Length ?? 0; 
        #endregion

        #region Constructor 
        public SignedBlock(UnsignedBlock unsignedBlock)
        {
            // TODO [AboimPinto]: In the old logic this was after the creation of the Hash. Creating new Witness object will produce a different hash on the BlockHeader. This need to be verified.
            this.Witness = new SignedWitness(unsignedBlock.Witness);

            this.TransactionHashes = new UInt256[unsignedBlock.Transactions.Length];
            this.Transactions = new SignedTransaction[unsignedBlock.Transactions.Length];
            for (var i = 0; i < unsignedBlock.Transactions.Length; i++)
            {
                this.Transactions[i] = new SignedTransactionFactory().GetSignedTransaction(unsignedBlock.Transactions[i]);
                this.TransactionHashes[i] = this.Transactions[i].Hash;
            }

            var signingSettings = BinarySerializer.Default.Serialize(this, new BinarySerializerSettings()
            {
                Filter = x =>
                    x != nameof(this.Witness) &&
                    x != nameof(this.Type) &&
                    x != nameof(this.TransactionHashes) &&
                    x != nameof(this.Transactions) 
            });

            this.MerkleRoot = MerkleTree.ComputeRoot(TransactionHashes);

            this.Sign(unsignedBlock, signingSettings);
        }
        #endregion
    }
}
