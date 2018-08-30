using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NeoSharp.BinarySerialization;
using NeoSharp.BinarySerialization.Serializers;
using NeoSharp.Core.Converters;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Models.Transactions;
using NeoSharp.Core.Models.Witnesses;
using NeoSharp.Core.Types;

namespace NeoSharp.Core.Models.Blocks
{
    public class BlockSignatureManager : IBlockSignatureManager
    {
        #region Private Fields 
        private readonly Crypto _crypto;
        private readonly ITransactionSignatureManager _transactionSignatureManager;
        private readonly IWitnessSignatureManager _witnessSignatureManager;
        private readonly IBinarySerializer _binarySerializer;
        #endregion

        #region Constructor 
        public BlockSignatureManager(
            Crypto crypto, 
            ITransactionSignatureManager transactionSignatureManager, 
            IWitnessSignatureManager witnessSignatureManager, 
            IBinarySerializer binarySerializer)
        {
            this._crypto = crypto;
            this._transactionSignatureManager = transactionSignatureManager;
            this._witnessSignatureManager = witnessSignatureManager;
            this._binarySerializer = binarySerializer;
        }
        #endregion

        #region IBlockSignatureManager implementation 
        public SignedBlock Sign(Block unsignedBlock)
        {
            var signedBlockTransactions = unsignedBlock.Transactions
                .Select(blockTransaction => this._transactionSignatureManager.Sign(blockTransaction))
                .ToList()
                .AsReadOnly();

            var signedWitness = this._witnessSignatureManager.Sign(unsignedBlock.Witness);

            return new SignedBlock(unsignedBlock, signedWitness, signedBlockTransactions, this.HashCalculator);
        }

        public SignedBlock Sign(Block unsignedBlock, IReadOnlyList<SignedTransactionBase> signedTransactions)
        {
            var signedWitness = this._witnessSignatureManager.Sign(unsignedBlock.Witness);

            return new SignedBlock(unsignedBlock, signedWitness, signedTransactions, this.HashCalculator);
        }
        #endregion

        #region Private Methods 
        private UInt256 HashCalculator(SignedBlockBase signedBlockBase)
        {
            var signingSettings = this.GenerateSigningSettings(signedBlockBase, new BinarySerializerSettings
            {
                Filter = a =>
                    a != nameof(signedBlockBase.Witness) &&
                    a != nameof(signedBlockBase.Transactions) &&
                    a != nameof(signedBlockBase.TransactionHashes) &&
                    a != nameof(signedBlockBase.Type)
            });

            return new UInt256(this._crypto.Hash256(signingSettings));
        }

        private byte[] GenerateSigningSettings(SignedBlockBase blockBase, BinarySerializerSettings serializerSettings = null)
        {
            using (var ms = new MemoryStream())
            {
                this.Serialize(blockBase, ms, serializerSettings);
                return ms.ToArray();
            }
        }

        private int Serialize(SignedBlockBase blockBase, Stream stream, BinarySerializerSettings settings = null)
        {
            var serializeResult = 0;

            using (var bw = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                serializeResult += new BinaryUInt32Serializer().Serialize(this._binarySerializer, bw, blockBase.Version, settings);
                serializeResult += new UInt256Converter().Serialize(this._binarySerializer, bw, blockBase.PreviousBlockHash, settings);
                serializeResult += new UInt256Converter().Serialize(this._binarySerializer, bw, blockBase.MerkleRoot, settings);
                serializeResult += new BinaryUInt32Serializer().Serialize(this._binarySerializer, bw, blockBase.Timestamp, settings);
                serializeResult += new BinaryUInt32Serializer().Serialize(this._binarySerializer, bw, blockBase.Index, settings);
                serializeResult += new BinaryUInt64Serializer().Serialize(this._binarySerializer, bw, blockBase.ConsensusData, settings);
                serializeResult += new UInt160Converter().Serialize(this._binarySerializer, bw, blockBase.NextConsensus, settings);

                if (settings?.Filter?.Invoke(nameof(blockBase.Witness)) != false)
                {
                    this._binarySerializer.Serialize(blockBase.Witness, bw, settings);
                }

                if (settings?.Filter?.Invoke(nameof(blockBase.Transactions)) != false)
                {
                    this._binarySerializer.Serialize(blockBase.Transactions, bw, settings);
                }
            }

            return serializeResult;
        }
        #endregion
   }
}