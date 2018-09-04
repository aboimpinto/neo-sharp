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

            return new SignedBlock(unsignedBlock, signedWitness, signedBlockTransactions, this.SignedBlockHashCalculator);
        }

        public SignedBlock Sign(Block unsignedBlock, IReadOnlyList<SignedTransactionBase> signedTransactions)
        {
            var signedWitness = this._witnessSignatureManager.Sign(unsignedBlock.Witness);

            return new SignedBlock(unsignedBlock, signedWitness, signedTransactions, this.SignedBlockHashCalculator);
        }

        public SignedBlockHeader Sign(BlockHeader unsighedBlockHeader, IReadOnlyList<SignedTransactionBase> signedTransactions)
        {
            var signedWitness = this._witnessSignatureManager.Sign(unsighedBlockHeader.Witness);

            return new SignedBlockHeader(unsighedBlockHeader, signedWitness, signedTransactions, this.SignedBlockHeaderHashCalculator);
        }
        #endregion

        #region Private Methods 
        private UInt256 SignedBlockHashCalculator(SignedBlock signedBlock)
        {
            var signingSettings = this.GenerateBlockSigningSettings(signedBlock, new BinarySerializerSettings
            {
                Filter = a =>
                    a != nameof(signedBlock.Witness) &&
                    a != nameof(signedBlock.Transactions) &&
                    a != nameof(signedBlock.TransactionHashes) &&
                    a != nameof(signedBlock.Type)
            });

            return new UInt256(this._crypto.Hash256(signingSettings));
        }

        private UInt256 SignedBlockHeaderHashCalculator(SignedBlockHeader signedBlockHeader)
        {
            var signingSettings = this.GenerateBlockHeaderSigningSettings(signedBlockHeader, new BinarySerializerSettings
            {
                Filter = a =>
                    a != nameof(signedBlockHeader.Witness) &&
                    a != nameof(signedBlockHeader.TransactionHashes) &&
                    a != nameof(signedBlockHeader.Type)
            });

            return new UInt256(this._crypto.Hash256(signingSettings));
        }

        private byte[] GenerateBlockSigningSettings(SignedBlock signedBlock, BinarySerializerSettings serializerSettings = null)
        {
            using (var ms = new MemoryStream())
            {
                this.SerializeSignedBlock(signedBlock, ms, serializerSettings);
                return ms.ToArray();
            }
        }

        private int SerializeSignedBlock(SignedBlock signedBlock, Stream stream, BinarySerializerSettings settings = null)
        {
            var serializeResult = 0;

            using (var bw = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                serializeResult += new BinaryUInt32Serializer().Serialize(this._binarySerializer, bw, signedBlock.Version, settings);
                serializeResult += new UInt256Converter().Serialize(this._binarySerializer, bw, signedBlock.PreviousBlockHash, settings);
                serializeResult += new UInt256Converter().Serialize(this._binarySerializer, bw, signedBlock.MerkleRoot, settings);
                serializeResult += new BinaryUInt32Serializer().Serialize(this._binarySerializer, bw, signedBlock.Timestamp, settings);
                serializeResult += new BinaryUInt32Serializer().Serialize(this._binarySerializer, bw, signedBlock.Index, settings);
                serializeResult += new BinaryUInt64Serializer().Serialize(this._binarySerializer, bw, signedBlock.ConsensusData, settings);
                serializeResult += new UInt160Converter().Serialize(this._binarySerializer, bw, signedBlock.NextConsensus, settings);

                if (settings?.Filter?.Invoke(nameof(signedBlock.Witness)) != false)
                {
                    this._binarySerializer.Serialize(signedBlock.Witness, bw, settings);
                }

                if (settings?.Filter?.Invoke(nameof(signedBlock.Transactions)) != false)
                {
                    this._binarySerializer.Serialize(signedBlock.Transactions, bw, settings);
                }
            }

            return serializeResult;
        }

        private byte[] GenerateBlockHeaderSigningSettings(SignedBlockHeader signedBlockHeader, BinarySerializerSettings serializerSettings = null)
        {
            using (var ms = new MemoryStream())
            {
                this.SerializeSignedBlockHeader(signedBlockHeader, ms, serializerSettings);
                return ms.ToArray();
            }
        }

        private int SerializeSignedBlockHeader(SignedBlockHeader signedBlockHeader, Stream stream, BinarySerializerSettings settings = null)
        {
            var serializeResult = 0;

            using (var bw = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                serializeResult += new BinaryUInt32Serializer().Serialize(this._binarySerializer, bw, signedBlockHeader.Version, settings);
                serializeResult += new UInt256Converter().Serialize(this._binarySerializer, bw, signedBlockHeader.PreviousBlockHash, settings);
                serializeResult += new UInt256Converter().Serialize(this._binarySerializer, bw, signedBlockHeader.MerkleRoot, settings);
                serializeResult += new BinaryUInt32Serializer().Serialize(this._binarySerializer, bw, signedBlockHeader.Timestamp, settings);
                serializeResult += new BinaryUInt32Serializer().Serialize(this._binarySerializer, bw, signedBlockHeader.Index, settings);
                serializeResult += new BinaryUInt64Serializer().Serialize(this._binarySerializer, bw, signedBlockHeader.ConsensusData, settings);
                serializeResult += new UInt160Converter().Serialize(this._binarySerializer, bw, signedBlockHeader.NextConsensus, settings);

                if (settings?.Filter?.Invoke(nameof(signedBlockHeader.Witness)) != false)
                {
                    this._binarySerializer.Serialize(signedBlockHeader.Witness, bw, settings);
                }
            }

            return serializeResult;
        }
        #endregion
    }
}