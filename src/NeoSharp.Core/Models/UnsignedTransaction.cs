using NeoSharp.Core.Types;

namespace NeoSharp.Core.Models
{
    public class UnsignedTransaction : TransactionBase
    {
        #region Public Properties 
        public new UInt256 Hash { get; set; }

        public new TransactionType Type { get; set; }

        public new byte Version { get; set; }

        public new TransactionAttribute[] Attributes = new TransactionAttribute[0];

        public new CoinReference[] Inputs = new CoinReference[0];

        public new TransactionOutput[] Outputs = new TransactionOutput[0];

        public UnsignedWitness[] Witness;
        #endregion 
    }
}
