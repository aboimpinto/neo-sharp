using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Extensions;
using NeoSharp.Core.Models.Blocks;
using NeoSharp.Core.Models.Transactions;
using NeoSharp.Core.Models.Witnesses;
using NeoSharp.Core.Network;
using NeoSharp.Core.SmartContract;
using NeoSharp.Core.Types;

namespace NeoSharp.Core.Models.Builders
{
    public class BlockBuilder
    {
        #region Private fields 
        private readonly NetworkConfig _networkConfig;
        #endregion

        #region Constructor
        public BlockBuilder()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);

            var configuration = builder.Build();

            this._networkConfig = new NetworkConfig(configuration);
        }
        #endregion

        #region Public Methods 
        public SignedBlock BuildSignedGenesisBlock()
        {
            var transactionBuilder = new TransactionBuilder();

            var governingToken = transactionBuilder.BuildGenesisGoverningTokenRegisterTransaction();
            var utilityToken = transactionBuilder.BuildGenesisUtilityTokenRegisterTransaction();

            var genesisMinerTransaction = transactionBuilder.BuildGenesisMinerTransaction();

            var genesisWitness = new WitnessBuilder().BuildGenesisWitness(); 
            var genesisTimestamp = new DateTime(2016, 7, 15, 15, 8, 21, DateTimeKind.Utc).ToTimestamp();
            const ulong genesisConsensusData = 2083236893; //向比特币致敬

            var nextConsensusAddress = this.GetGenesisNextConsensusAddress();

            var genesisBlock = new Blocks.Block
            {
                PreviousBlockHash = UInt256.Zero,
                Timestamp = genesisTimestamp,
                Index = 0,
                ConsensusData = genesisConsensusData,
                NextConsensus = nextConsensusAddress,
                Witness = genesisWitness
            };

            var witnessSignatureManager = new WitnessSignatureManager(Crypto.Default);
            var transactionSignatureManager = new TransactionSignatureManager(
                Crypto.Default, 
                witnessSignatureManager, 
                BinarySerializer.Default, 
                BinaryDeserializer.Default);

            var signedGoverningTokenRegisterTransaction = transactionSignatureManager.Sign(governingToken);
            var genesisIssueTransaction = transactionBuilder
                .BuildIssueTransaction((SignedRegisterTransaction)signedGoverningTokenRegisterTransaction);

            var signedTransactions = new List<SignedTransactionBase>
            {
                transactionSignatureManager.Sign(genesisMinerTransaction),      //First transaction is always a miner transaction
                signedGoverningTokenRegisterTransaction,                        //Creates NEO 
                transactionSignatureManager.Sign(utilityToken),                 //Creates GAS
                transactionSignatureManager.Sign(genesisIssueTransaction)       //Send all NEO to seed contract
            };

            var blockSignatureManager = new BlockSignatureManager(
                Crypto.Default, 
                transactionSignatureManager, 
                witnessSignatureManager, 
                BinarySerializer.Default,
                BinaryDeserializer.Default);
            return blockSignatureManager.Sign(genesisBlock, signedTransactions);
        }
        #endregion

        #region Private Methods 
        private UInt160 GetGenesisNextConsensusAddress()
        {
            var genesisValidators = this.GenesisStandByValidators();
            return ContractFactory.CreateMultiplePublicKeyRedeemContract(genesisValidators.Length - (genesisValidators.Length - 1) / 3, genesisValidators).Code.ScriptHash;
        }

        private ECPoint[] GenesisStandByValidators()
        {
            return _networkConfig.StandByValidator
                .Select(u => new ECPoint(u.HexToBytes()))
                .ToArray();
        }
        #endregion
    }
}
