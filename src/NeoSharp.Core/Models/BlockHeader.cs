﻿using System;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Types;
using Newtonsoft.Json;

namespace NeoSharp.Core.Models
{
    /// <summary>
    /// Header
    /// </summary>
    [Serializable]
    public class BlockHeader
    {
        public enum HeaderType : byte
        {
            /// <summary>
            /// Block unavailable, no hashes, no TX data
            /// </summary>
            Header = 0,
            /// <summary>
            /// Block available, with TX hashes
            /// </summary>
            Extended = 1,
        }

        #region Serializable data

        [BinaryProperty(0)]
        [JsonProperty("version")]
        public uint Version;

        [BinaryProperty(1)]
        [JsonProperty("previousblockhash")]
        public UInt256 PreviousBlockHash;

        [BinaryProperty(2)]
        [JsonProperty("merkleroot")]
        public UInt256 MerkleRoot;

        [BinaryProperty(3)]
        [JsonProperty("time")]
        public uint Timestamp;

        [BinaryProperty(4)]
        [JsonProperty("index")]
        public uint Index;

        [BinaryProperty(5)]
        [JsonProperty("nonce")]
        public ulong ConsensusData;

        [BinaryProperty(6)]
        [JsonProperty("nextconsensus")]
        public UInt160 NextConsensus;

        /// <summary>
        /// Set the kind of the header
        /// </summary>
        [BinaryProperty(7)]
        public HeaderType Type;

        [BinaryProperty(8)]
        [JsonProperty("script")]
        public Witness Witness;

        [BinaryProperty(100)]
        [JsonProperty("txhashes")]
        public UInt256[] TransactionHashes { get; set; }

        #endregion

        #region Non serializable data

        [JsonProperty("txcount")]
        public virtual int TransactionCount => TransactionHashes?.Length ?? 0;

        [JsonProperty("hash")]
        public UInt256 Hash { get; set; }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public BlockHeader()
        {
            Type = HeaderType.Header;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Type</param>
        public BlockHeader(HeaderType type)
        {
            Type = type;
        }

        /// <summary>
        /// Update hash
        /// </summary>
        /// <param name="serializer">Serializer</param>
        /// <param name="crypto">Crypto</param>
        public virtual void UpdateHash(IBinarySerializer serializer, Crypto crypto)
        {
            if (MerkleRoot == null)
            {
                // Compute hash

                MerkleRoot = MerkleTree.ComputeRoot(crypto, TransactionHashes);
            }

            Hash = new UInt256(crypto.Hash256(serializer.Serialize(this, new BinarySerializerSettings()
            {
                Filter = (a) => a != nameof(Witness) && a != nameof(Type) && a != nameof(TransactionHashes)
            })));

            Witness?.UpdateHash(serializer, crypto);
        }

        /// <summary>
        /// Get block header trimmed
        /// </summary>
        /// <returns>Return block header</returns>
        public BlockHeader Trim()
        {
            if (Type == HeaderType.Header && TransactionHashes.Length == 0)
            {
                return this;
            }

            return new BlockHeader(HeaderType.Header)
            {
                ConsensusData = ConsensusData,
                Index = Index,
                Hash = Hash,
                MerkleRoot = MerkleRoot,
                NextConsensus = NextConsensus,
                TransactionHashes = new UInt256[0],
                PreviousBlockHash = PreviousBlockHash,
                Witness = Witness,
                Timestamp = Timestamp,
                Version = Version,
            };
        }

        /// <summary>
        /// Get block
        /// </summary>
        /// <param name="txs">Transactions</param>
        /// <returns>Return block</returns>
        public Block GetBlock(Transaction[] txs)
        {
            return new Block()
            {
                ConsensusData = ConsensusData,
                Index = Index,
                Hash = Hash,
                MerkleRoot = MerkleRoot,
                NextConsensus = NextConsensus,
                TransactionHashes = TransactionHashes,
                PreviousBlockHash = PreviousBlockHash,
                Witness = Witness,
                Timestamp = Timestamp,
                Version = Version,
                Transactions = txs,
            };
        }
    }
}