using System;
using System.Collections.Generic;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Types;

namespace NeoSharp.Core.Models.Transactions
{
    [Serializable]
    public class SignedRegisterTransaction : SignedTransactionBase
    {
        #region Private Fields 
        private readonly RegisterTransaction _registerTransaction;
        #endregion

        #region Public Properties 
        /// <summary>
        /// Asset Type
        /// </summary>
        public AssetType AssetType => this._registerTransaction.AssetType;

        /// <summary>
        /// Name
        /// </summary>
        public string Name => this._registerTransaction.Name;

        /// <summary>
        /// The total number of issues, a total of two modes:
        ///   1. Limited Mode: When Amount is positive, the maximum total amount of the current asset is Amount, and cannot be modified (Equities may support expansion or additional issuance in the future, and will consider the company’s signature or a certain proportion of shareholders Signature recognition).
        ///   2. Unlimited mode: When Amount is equal to -1, the current asset can be issued by the creator indefinitely. This model has the greatest degree of freedom, but it has the lowest credibility and is not recommended for use.
        /// </summary>
        public Fixed8 Amount => this._registerTransaction.Amount;

        /// <summary>
        /// Precision
        /// </summary>
        public byte Precision => this._registerTransaction.Precision;

        /// <summary>
        /// Publisher's public key
        /// </summary>
        public ECPoint Owner => this._registerTransaction.Owner;

        /// <summary>
        /// Asset Manager Contract Hash Value
        /// </summary>
        public UInt160 Admin => this._registerTransaction.Admin;
        #endregion

        #region Constructor 
        public SignedRegisterTransaction(
            RegisterTransaction registerTransaction, 
            IEnumerable<Witnesses.SignedWitness> witnesses,
            Func<SignedTransactionBase, UInt256> hashCalculatorMethod) 
            : base(registerTransaction, witnesses, hashCalculatorMethod)
        {
            this._registerTransaction = registerTransaction;
            this.Sign();
        }
        #endregion
    }
}
