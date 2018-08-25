using System.IO;
using System.Linq;
using System.Text;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Models.Witnesses;
using NeoSharp.Core.Types;

namespace NeoSharp.Core.Models.Transactions
{
    public class RegisterTransactionSignatureManager : IRegisterTransactionSignatureManager
    {
        #region Private Fields 
        private readonly Crypto _crypto;
        private readonly IWitnessSignatureManager _witnessSignatureManager;
        private readonly IBinarySerializer _binarySerializer;
        #endregion

        #region Constructor 
        public RegisterTransactionSignatureManager(Crypto crypto, IWitnessSignatureManager witnessSignatureManager, IBinarySerializer binarySerializer)
        {
            this._crypto = crypto;
            this._witnessSignatureManager = witnessSignatureManager;
            this._binarySerializer = binarySerializer;
        }
        #endregion

        #region IRegisterTransactionSignatureManager Implementation 
        public SignedRegisterTransaction Sign(RegisterTransaction registerTransaction)
        {
            var signedWitnesses = registerTransaction.Witness
                .Select(unsignedWitness => this._witnessSignatureManager.SignWitness(unsignedWitness))
                .ToList();

            var signingSettings = this.GenerateSigningSettings(registerTransaction, new BinarySerializerSettings
            {
                Filter = x => x != nameof(registerTransaction.Witness)
            });

            var hash = new UInt256(this._crypto.Hash256(signingSettings));

            return new SignedRegisterTransaction(registerTransaction, hash, signedWitnesses);
        }
        #endregion

        #region Private Methods 

        private byte[] GenerateSigningSettings(RegisterTransaction registerTransaction, BinarySerializerSettings serializerSettings = null)
        {
            using (var ms = new MemoryStream())
            {
                this.Serialize(registerTransaction, ms, serializerSettings);
                return ms.ToArray();
            }
        }

        private int Serialize(RegisterTransaction registerTransaction, Stream stream, BinarySerializerSettings settings = null)
        {
            var serializeResult = 2;

            using (var bw = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                bw.Write((byte)registerTransaction.Type);
                bw.Write(registerTransaction.Version);

                // Exclusive transaction data
                serializeResult += this.SerializeExecusiveData(registerTransaction, bw, settings);

                // Shared transaction data
                serializeResult += this._binarySerializer.Serialize(registerTransaction.Attributes.ToArray(), bw, settings);
                serializeResult += this._binarySerializer.Serialize(registerTransaction.Inputs.ToArray(), bw, settings);
                serializeResult += this._binarySerializer.Serialize(registerTransaction.Outputs.ToArray(), bw, settings);

                // Serialize sign
                if (settings?.Filter?.Invoke(nameof(registerTransaction.Witness)) != false)
                {
                    serializeResult += this._binarySerializer.Serialize(registerTransaction.Witness, bw, settings);
                }
            }

            return serializeResult;
        }

        private int SerializeExecusiveData(RegisterTransaction registerTransaction, BinaryWriter bw, BinarySerializerSettings settings = null)
        {
            var serializeReturn = 1;

            bw.Write((byte)registerTransaction.AssetType);
            serializeReturn += bw.WriteVarString(registerTransaction.Name);

            bw.Write(registerTransaction.Amount.Value);
            serializeReturn += Fixed8.Size;

            bw.Write(registerTransaction.Precision);
            serializeReturn++;

            serializeReturn += this._binarySerializer.Serialize(registerTransaction.Owner, bw, settings);
            serializeReturn += this._binarySerializer.Serialize(registerTransaction.Admin, bw, settings);

            return serializeReturn;
        }
        #endregion
    }
}