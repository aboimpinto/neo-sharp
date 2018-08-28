using System.IO;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Models.Witnesses;
using NeoSharp.Core.Types;

namespace NeoSharp.Core.Models.Transactions
{
    public class RegisterTransactionSignatureManager : TransactionSignatureManagerBase, IRegisterTransactionSignatureManager
    {
        #region Private Fields 
        private readonly IBinarySerializer _binarySerializer;
        #endregion

        #region Constructor 
        public RegisterTransactionSignatureManager(Crypto crypto, IWitnessSignatureManager witnessSignatureManager, IBinarySerializer binarySerializer)
            : base(crypto, witnessSignatureManager, binarySerializer)
        {
            this._binarySerializer = binarySerializer;
        }
        #endregion

        #region IRegisterTransactionSignatureManager Implementation 
        public SignedRegisterTransaction Sign(RegisterTransaction registerTransaction)
        {
            return this.Sign<RegisterTransaction, SignedRegisterTransaction>(registerTransaction);
        }
        #endregion

        #region Override Methods
        public override int SerializeExecusiveData(TransactionBase transactionBase, BinaryWriter binaryWriter, BinarySerializerSettings settings = null)
        {
            var registerTransaction = (RegisterTransaction) transactionBase;

            var serializeReturn = 1;

            binaryWriter.Write((byte)registerTransaction.AssetType);
            serializeReturn += binaryWriter.WriteVarString(registerTransaction.Name);

            binaryWriter.Write(registerTransaction.Amount.Value);
            serializeReturn += Fixed8.Size;

            binaryWriter.Write(registerTransaction.Precision);
            serializeReturn++;

            serializeReturn += this._binarySerializer.Serialize(registerTransaction.Owner, binaryWriter, settings);
            serializeReturn += this._binarySerializer.Serialize(registerTransaction.Admin, binaryWriter, settings);

            return serializeReturn;
        }
        #endregion
    }
}