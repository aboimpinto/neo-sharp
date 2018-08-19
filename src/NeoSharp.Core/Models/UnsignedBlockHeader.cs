using NeoSharp.Core.Types;

namespace NeoSharp.Core.Models
{
    public class UnsignedBlockHeader : BlockBase
    {
        #region Public Properties 
        public new uint Version { get; set; }

        public new UInt256 PreviousBlockHash { get; set; }

        public new UInt256 MerkleRoot { get; set; }

        public new uint Timestamp { get; set; }

        public new uint Index { get; set; }

        public new ulong ConsensusData { get; set; }

        public new UInt160 NextConsensus { get; set; }

        public new HeaderType Type { get; set; }

        public new UInt256[] TransactionHashes { get; set; }

        public UnsignedWitness Witness { get; set; }
        #endregion

        #region Constructor
        public UnsignedBlockHeader()
        {
            this.Type = HeaderType.Header;
        }
        #endregion
    }
}
