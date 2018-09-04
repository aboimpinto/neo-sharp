using System;
using System.Collections.Generic;
using System.Linq;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Models.Witnesses;
using NeoSharp.Core.Types;
using Newtonsoft.Json;

namespace NeoSharp.Core.Models.Blocks
{
    public class SignedBlockBase
    {
        #region Private Fields 
        private readonly BlockBase _block;
        private readonly Func<SignedBlock, UInt256> _signedBlockHashCalculatorMethod;
        private readonly Func<SignedBlockHeader, UInt256> _signedBlockHeaderHashCalculatorMethod;
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

        [JsonProperty("txcount")]
        public virtual int TransactionCount => this.TransactionHashes?.Length ?? 0;

        [JsonProperty("hash")]
        public UInt256 Hash { get; private set; }
        #endregion

        #region Constructor
        public SignedBlockBase(
            BlockBase block, 
            SignedWitness signedWitness, 
            IEnumerable<UInt256> signedTransactionHashes,
            Func<SignedBlock, UInt256> signedBlockHashCalculatorMethod)
        {
            this._block = block;
            this._signedBlockHashCalculatorMethod = signedBlockHashCalculatorMethod;

            this.Witness = signedWitness;
            this.TransactionHashes = signedTransactionHashes.ToArray();

            this.MerkleRoot = MerkleTree.ComputeRoot(this.TransactionHashes);
        }

        public SignedBlockBase(
            BlockBase blockHeader,
            SignedWitness signedWitness,
            IEnumerable<UInt256> signedTransactionsHashes,
            Func<SignedBlockHeader, UInt256> signedBlockHederHashCalculatorMethod)
        {
            this._block = blockHeader;
            this._signedBlockHeaderHashCalculatorMethod = signedBlockHederHashCalculatorMethod;

            this.Witness = signedWitness;
            this.TransactionHashes = signedTransactionsHashes.ToArray();

            this.MerkleRoot = MerkleTree.ComputeRoot(this.TransactionHashes);
        }
        #endregion

        #region Public Methods 

        public void SignedBlockHashCalculator()
        {
            this.Hash = this._signedBlockHashCalculatorMethod.Invoke((SignedBlock)this);
        }

        public void SignedBlockHeaderHashCalculator()
        {
            this.Hash = this._signedBlockHeaderHashCalculatorMethod.Invoke((SignedBlockHeader) this);
        }
        #endregion
    }
}
