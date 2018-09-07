using NeoSharp.Core.Types;

namespace NeoSharp.Core.Models.Blocks
{
    public class BlockBase
    {
        public uint Version { get; set; }

        public UInt256 PreviousBlockHash { get; set; }

        public uint Timestamp { get; set; }

        public uint Index { get; set; }
        
        public ulong ConsensusData { get; set; }

        public UInt160 NextConsensus { get; set; }

        public HeaderType Type { get; set; }

        public Witnesses.Witness Witness { get; set; }
    }
}
