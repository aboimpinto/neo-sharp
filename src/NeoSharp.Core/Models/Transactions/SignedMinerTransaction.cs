using System.Collections.Generic;
using NeoSharp.Core.Types;

namespace NeoSharp.Core.Models.Transactions
{
    public class SignedMinerTransaction : SignedTransactionBase
    {
        #region Private Fields 
        private readonly MinerTransaction _minerTransaction;
        #endregion

        #region Public Properties 
        /// <summary>
        /// Random number
        /// </summary>
        public uint Nonce => this._minerTransaction.Nonce;
        #endregion

        #region Constructor 
        public SignedMinerTransaction(MinerTransaction minerTransaction, UInt256 hash, IEnumerable<Witnesses.SignedWitness> witnesses) 
            : base(minerTransaction, hash, witnesses)
        {
            this._minerTransaction = minerTransaction;
        }
        #endregion
    }
}
