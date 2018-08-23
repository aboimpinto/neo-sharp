using System.Collections.Generic;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Types;

namespace NeoSharp.Core.Models.Transactions
{
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
        public SignedRegisterTransaction(RegisterTransaction registerTransaction, UInt256 hash, IEnumerable<Witnesses.SignedWitness> witnesses) 
            : base(registerTransaction, hash, witnesses)
        {
            this._registerTransaction = registerTransaction;

            //this.Sign();
        }
        #endregion

        #region Override Methods
        //public override int SerializeExecusiveData(IBinarySerializer serializer, BinaryWriter writer, BinarySerializerSettings settings = null)
        //{
        //    var serializeReturn = 1;

        //    writer.Write((byte)this.AssetType);
        //    serializeReturn += writer.WriteVarString(this.Name);

        //    writer.Write(this.Amount.Value);
        //    serializeReturn += Fixed8.Size;

        //    writer.Write(Precision);
        //    serializeReturn++;

        //    serializeReturn += serializer.Serialize(Owner, writer, settings);
        //    serializeReturn += serializer.Serialize(Admin, writer, settings);

        //    return serializeReturn;
        //}
        #endregion
    }
}
