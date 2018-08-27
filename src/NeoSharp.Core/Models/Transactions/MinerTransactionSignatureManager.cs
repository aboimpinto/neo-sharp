using System.IO;
using System.Linq;
using System.Text;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Models.Witnesses;
using NeoSharp.Core.Types;

namespace NeoSharp.Core.Models.Transactions
{
    public class MinerTransactionSignatureManager : IMinerTransactionSignatureManager
    {
        #region Private Fields 
        private readonly Crypto _crypto;
        private readonly IWitnessSignatureManager _witnessSignatureManager;
        private readonly IBinarySerializer _binarySerializer;
        #endregion

        #region Constructor 
        public MinerTransactionSignatureManager(Crypto crypto, IWitnessSignatureManager witnessSignatureManager, IBinarySerializer binarySerializer)
        {
            this._crypto = crypto;
            this._witnessSignatureManager = witnessSignatureManager;
            this._binarySerializer = binarySerializer;
        }
        #endregion

        #region IMinerTransactionSignatureManager implementation 
        public SignedMinerTransaction Sign(MinerTransaction minerTransaction)
        {
            var signedWitnesses = minerTransaction.Witness
                .Select(unsignedWitness => this._witnessSignatureManager.SignWitness(unsignedWitness))
                .ToList();

            var signingSettings = this.GenerateSigningSettings(minerTransaction, new BinarySerializerSettings
            {
                Filter = x => x != nameof(minerTransaction.Witness)
            });

            var hash = new UInt256(this._crypto.Hash256(signingSettings));

            return new SignedMinerTransaction(minerTransaction, hash, signedWitnesses);
        }
        #endregion

        #region Private Methods 
        private byte[] GenerateSigningSettings(MinerTransaction minerTransaction, BinarySerializerSettings serializerSettings = null)
        {
            using (var ms = new MemoryStream())
            {
                this.Serialize(minerTransaction, ms, serializerSettings);
                return ms.ToArray();
            }
        }

        private int Serialize(MinerTransaction minerTransaction, Stream stream, BinarySerializerSettings settings = null)
        {
            var serializeResult = 2;

            using (var bw = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                bw.Write((byte)minerTransaction.Type);
                bw.Write(minerTransaction.Version);

                // Exclusive transaction data
                serializeResult += this.SerializeExecusiveData(minerTransaction, bw, settings);

                // Shared transaction data
                serializeResult += this._binarySerializer.Serialize(minerTransaction.Attributes.ToArray(), bw, settings);
                serializeResult += this._binarySerializer.Serialize(minerTransaction.Inputs.ToArray(), bw, settings);
                serializeResult += this._binarySerializer.Serialize(minerTransaction.Outputs.ToArray(), bw, settings);

                // Serialize sign
                if (settings?.Filter?.Invoke(nameof(minerTransaction.Witness)) != false)
                {
                    serializeResult += this._binarySerializer.Serialize(minerTransaction.Witness, bw, settings);
                }
            }

            return serializeResult;
        }

        private int SerializeExecusiveData(MinerTransaction minerTransaction, BinaryWriter bw, BinarySerializerSettings settings = null)
        {
            bw.Write(minerTransaction.Nonce);
            return 4;
        }
        #endregion
    }
}
