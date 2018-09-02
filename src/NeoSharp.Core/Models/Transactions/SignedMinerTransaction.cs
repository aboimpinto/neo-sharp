using System;
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
        public SignedMinerTransaction(
            MinerTransaction minerTransaction, 
            IEnumerable<Witnesses.SignedWitness> witnesses,
            Func<SignedTransactionBase, UInt256> hashCalculatorMethod) 
            : base(minerTransaction, witnesses, hashCalculatorMethod)
        {
            this._minerTransaction = minerTransaction;
            this.Sign();
        }
        #endregion
    }
}
