using System;
using System.Collections.Generic;
using System.Linq;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Models.Transactions;
using NeoSharp.Core.Models.Witnesses;
using NeoSharp.Core.Types;
using Newtonsoft.Json;

namespace NeoSharp.Core.Models.Blocks
{
    public class SignedBlockBase
    {
        #region Private Fields 
        private readonly Block _block;
        #endregion

        #region Public Properties 
        [BinaryProperty(0)]
        [JsonProperty("version")]
        public uint Version => this._block.Version;

        [BinaryProperty(1)]
        [JsonProperty("previousblockhash")]
        public UInt256 PreviousBlockHash => this._block.PreviousBlockHash;

        [BinaryProperty(2)]
        [JsonProperty("merkleroot")]
        public UInt256 MerkleRoot { get; private set; }

        [BinaryProperty(3)]
        [JsonProperty("time")]
        public uint Timestamp => this._block.Timestamp;

        [BinaryProperty(4)]
        [JsonProperty("index")]
        public uint Index => this._block.Index;

        [BinaryProperty(5)]
        [JsonProperty("nonce")]
        public ulong ConsensusData => this._block.ConsensusData;

        [BinaryProperty(6)]
        [JsonProperty("nextconsensus")]
        public UInt160 NextConsensus => this._block.NextConsensus;

        [BinaryProperty(7)] public HeaderType Type => this._block.Type;

        [BinaryProperty(8)]
        [JsonProperty("script")]
        public SignedWitness Witness { get; }

        [BinaryProperty(100)]
        [JsonProperty("txhashes")]
        public UInt256[] TransactionHashes { get; }

        [BinaryProperty(100, MaxLength = 0x10000, Override = true)]
        public SignedTransactionBase[] Transactions { get; }

        [JsonProperty("txcount")]
        public virtual int TransactionCount => this.TransactionHashes?.Length ?? 0;

        [JsonProperty("hash")]
        public UInt256 Hash { get; }
        #endregion

        #region Constructor
        public SignedBlockBase(
            Block block, 
            SignedWitness signedWitness, 
            IReadOnlyList<SignedTransactionBase> signedTransactions,
            Func<SignedBlockBase, UInt256> hashCalculateMethod)
        {
            this._block = block;

            this.Witness = signedWitness;
            this.Transactions = signedTransactions.ToArray();
            this.TransactionHashes = signedTransactions.Select(x => x.Hash).ToArray();

            this.MerkleRoot = MerkleTree.ComputeRoot(this.TransactionHashes);

            this.Hash = hashCalculateMethod.Invoke(this);
        }
        #endregion
    }
}
