using System;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Converters;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Types;

namespace NeoSharp.Core.Models
{
    [Serializable]
    [BinaryTypeSerializer(typeof(TransactionSerializer))]
    public class SignedRegisterTransaction : SignedTransaction
    {
        #region Public Properties 
        /// <summary>
        /// Asset Type
        /// </summary>
        public AssetType AssetType { get; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The total number of issues, a total of two modes:
        ///   1. Limited Mode: When Amount is positive, the maximum total amount of the current asset is Amount, and cannot be modified (Equities may support expansion or additional issuance in the future, and will consider the company’s signature or a certain proportion of shareholders Signature recognition).
        ///   2. Unlimited mode: When Amount is equal to -1, the current asset can be issued by the creator indefinitely. This model has the greatest degree of freedom, but it has the lowest credibility and is not recommended for use.
        /// </summary>
        public Fixed8 Amount { get; }

        /// <summary>
        /// Precision
        /// </summary>
        public byte Precision { get; }

        /// <summary>
        /// Publisher's public key
        /// </summary>
        public ECPoint Owner { get; }

        /// <summary>
        /// Asset Manager Contract Hash Value
        /// </summary>
        public UInt160 Admin { get; }
        #endregion

        #region Constructor 
        public SignedRegisterTransaction(UnsignedTransaction unsignedTransaction) : base(unsignedTransaction)
        {
            var unsignedRegisterTransaction = (UnsignedRegisterTransaction) unsignedTransaction;
            if (unsignedRegisterTransaction == null)
            {
                throw new ArgumentException("The unsigned transaction is not UnsignedRegisterTransaction.");
            }

            this.AssetType = unsignedRegisterTransaction.AssetType;
            this.Name = unsignedRegisterTransaction.Name;
            this.Amount = unsignedRegisterTransaction.Amount;
            this.Precision = unsignedRegisterTransaction.Precision;
            this.Owner = unsignedRegisterTransaction.Owner;
            this.Admin = unsignedRegisterTransaction.Admin;
        }
        #endregion
    }
}
