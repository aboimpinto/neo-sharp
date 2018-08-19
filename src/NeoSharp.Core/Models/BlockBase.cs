using NeoSharp.BinarySerialization;
using NeoSharp.Core.Types;
using Newtonsoft.Json;

namespace NeoSharp.Core.Models
{
    public abstract class BlockBase
    {
        #region Public Properties 

        [BinaryProperty(0)]
        [JsonProperty("version")]
        public uint Version { get; private set; }

        [BinaryProperty(1)]
        [JsonProperty("previousblockhash")]
        public UInt256 PreviousBlockHash { get; private set; }

        [BinaryProperty(2)]
        [JsonProperty("merkleroot")]
        public UInt256 MerkleRoot { get; private set; }

        [BinaryProperty(3)]
        [JsonProperty("time")]
        public uint Timestamp { get; private set; }

        [BinaryProperty(4)]
        [JsonProperty("index")]
        public uint Index { get; private set; }

        [BinaryProperty(5)]
        [JsonProperty("nonce")]
        public ulong ConsensusData { get; private set; }

        [BinaryProperty(6)]
        [JsonProperty("nextconsensus")]
        public UInt160 NextConsensus { get; private set; }

        [BinaryProperty(7)]
        public HeaderType Type { get; private set; }

        [BinaryProperty(100)]
        [JsonProperty("txhashes")]
        public UInt256[] TransactionHashes { get; set; }

        [JsonProperty("txcount")] public virtual int TransactionCount => TransactionHashes?.Length ?? 0;

        [JsonProperty("hash")] public UInt256 Hash { get; set; }
        #endregion

        #region Protected Methods 
        protected void Sign(BlockBase blockBase)
        {
            this.Version = blockBase.Version;
            this.PreviousBlockHash = blockBase.PreviousBlockHash;
            this.MerkleRoot = blockBase.MerkleRoot;
            this.Timestamp = blockBase.Timestamp;
            this.Index = blockBase.Index;
            this.ConsensusData = blockBase.ConsensusData;
            this.NextConsensus = blockBase.NextConsensus;
            this.Type = blockBase.Type;
            this.TransactionHashes = blockBase.TransactionHashes;
            this.Hash = blockBase.Hash;
        }
        #endregion

    }
}
