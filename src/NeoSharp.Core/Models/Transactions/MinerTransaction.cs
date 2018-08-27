namespace NeoSharp.Core.Models.Transactions
{
    public class MinerTransaction : TransactionBase
    {
        #region Public Properties 
        /// <summary>
        /// Random number
        /// </summary>
        public uint Nonce { get; set; }
        #endregion

        #region Construtor 
        public MinerTransaction()
        {
            this.Type = TransactionType.MinerTransaction;
        }
        #endregion
    }
}
