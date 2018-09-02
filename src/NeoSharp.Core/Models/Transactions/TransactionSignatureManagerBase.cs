using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Models.Witnesses;
using NeoSharp.Core.Types;

namespace NeoSharp.Core.Models.Transactions
{
    public class TransactionSignatureManagerBase
    {
        #region Private Fields 
        private readonly Crypto _crypto;
        private readonly IWitnessSignatureManager _witnessSignatureManager;
        private readonly IBinarySerializer _binarySerializer;
        #endregion

        #region Constructor 
        protected TransactionSignatureManagerBase(Crypto crypto, IWitnessSignatureManager witnessSignatureManager, IBinarySerializer binarySerializer)
        {
            this._crypto = crypto;
            this._witnessSignatureManager = witnessSignatureManager;
            this._binarySerializer = binarySerializer;
        }
        #endregion

        #region Public Methods 
        protected TSigned Sign<TUnsigned, TSigned>(TUnsigned unsignedTransaction)
            where TUnsigned : TransactionBase
            where TSigned : SignedTransactionBase
        {
            var signedWitnesses = unsignedTransaction.Witness
                .Select(unsignedWitness => this._witnessSignatureManager.Sign(unsignedWitness))
                .ToList();

            var func = typeof(TransactionSignatureManagerBase)
                .GetMethod("HashCalculator", BindingFlags.NonPublic | BindingFlags.Instance);
            var funcTypeArgs = new[] {typeof(SignedTransactionBase), typeof(UInt256)};

            var ctrArgs = typeof(Func<,>).MakeGenericType(funcTypeArgs);
            var funcDelegate = Delegate.CreateDelegate(ctrArgs, this, func);

            return (TSigned)Activator.CreateInstance(typeof(TSigned), unsignedTransaction, signedWitnesses, funcDelegate);
        }

        public virtual int SerializeExecusiveData(SignedTransactionBase transactionBase, BinaryWriter binaryWriter, BinarySerializerSettings settings = null)
        {
            return 0;
        }
        #endregion

        #region Private Methods 
        private UInt256 HashCalculator(SignedTransactionBase signedTransactionBase)
        {
            var signingSettings = this.GenerateSigningSettings(signedTransactionBase, new BinarySerializerSettings
            {
                Filter = x => x != nameof(signedTransactionBase.Witness)
            });

            return new UInt256(this._crypto.Hash256(signingSettings));
        }

        private byte[] GenerateSigningSettings(SignedTransactionBase transactionBase, BinarySerializerSettings serializerSettings = null)
        {
            using (var ms = new MemoryStream())
            {
                this.Serialize(transactionBase, ms, serializerSettings);
                return ms.ToArray();
            }
        }

        private int Serialize(SignedTransactionBase transactionBase, Stream stream, BinarySerializerSettings settings = null)
        {
            var serializeResult = 2;

            using (var bw = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                bw.Write((byte)transactionBase.Type);
                bw.Write(transactionBase.Version);

                // Exclusive transaction data
                serializeResult += this.SerializeExecusiveData(transactionBase, bw, settings);

                // Shared transaction data
                serializeResult += this._binarySerializer.Serialize(transactionBase.Attributes.ToArray(), bw, settings);
                serializeResult += this._binarySerializer.Serialize(transactionBase.Inputs.ToArray(), bw, settings);
                serializeResult += this._binarySerializer.Serialize(transactionBase.Outputs.ToArray(), bw, settings);

                // Serialize sign
                if (settings?.Filter?.Invoke(nameof(transactionBase.Witness)) != false)
                {
                    serializeResult += this._binarySerializer.Serialize(transactionBase.Witness, bw, settings);
                }
            }

            return serializeResult;
        }
        #endregion
    }
}
