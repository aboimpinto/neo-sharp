namespace NeoSharp.Core.Models
{
    public class UnsignedMinerTransaction : UnsignedTransaction
    {
        #region Public Properties 
        /// <summary>
        /// Random number
        /// </summary>
        public uint Nonce { get; set; }
        #endregion

        #region Constructor 

        public UnsignedMinerTransaction()
        {
            this.Type = TransactionType.MinerTransaction;
        }
        #endregion
    }
}
