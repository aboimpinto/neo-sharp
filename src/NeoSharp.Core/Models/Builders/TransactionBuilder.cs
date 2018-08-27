using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Extensions;
using NeoSharp.Core.Network;
using NeoSharp.Core.Types;
using NeoSharp.VM;

namespace NeoSharp.Core.Models.Builders
{
    public class TransactionBuilder
    {
        #region Private Fields 
        private readonly NetworkConfig _networkConfig;
        #endregion

        #region Constructor 
        public TransactionBuilder()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);

            var configuration = builder.Build();

            this._networkConfig = new NetworkConfig(configuration);
        }
        #endregion

        #region Public Methods 
        public Transactions.RegisterTransaction BuildGoverningTokenRegisterTransaction()
        {
            var governingTokenRegisterTransaction = new Transactions.RegisterTransaction
            {
                AssetType = AssetType.GoverningToken,
                Name = "[{\"lang\":\"zh-CN\",\"name\":\"小蚁股\"},{\"lang\":\"en\",\"name\":\"AntShare\"}]",
                Amount = Fixed8.FromDecimal(100000000),
                Precision = 0,
                Owner = ECPoint.Infinity,
                Admin = new[] { (byte)EVMOpCode.PUSH1 }.ToScriptHash(),       //TODO: Why this? Check with people
                Attributes = new List<TransactionAttribute>(),
                Inputs = new List<CoinReference>(),
                Outputs = new List<TransactionOutput>(),
                Witness = new List<Witnesses.Witness>()
            };

            return governingTokenRegisterTransaction;
        }

        public Transactions.RegisterTransaction BuildUtilityTokenRegisterTransaction()
        {
            const uint decrementInterval = 2000000;
            uint[] gasGenerationPerBlock = { 8, 7, 6, 5, 4, 3, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            var utilityTokenRegisterTransaction = new Transactions.RegisterTransaction
            {
                AssetType = AssetType.UtilityToken,
                Name = "[{\"lang\":\"zh-CN\",\"name\":\"小蚁币\"},{\"lang\":\"en\",\"name\":\"AntCoin\"}]",
                Amount = Fixed8.FromDecimal(gasGenerationPerBlock.Sum(p => p) * decrementInterval),
                Precision = 8,
                Owner = ECPoint.Infinity,
                Admin = new[] { (byte)EVMOpCode.PUSH0 }.ToScriptHash(),     //TODO: Why this? Check with people
                Attributes = new List<TransactionAttribute>(),
                Inputs = new List<CoinReference>(),
                Outputs = new List<TransactionOutput>(),
                Witness = new List<Witnesses.Witness>()
            };

            return utilityTokenRegisterTransaction;
        }

        //public UnsignedMinerTransaction BuildUnsignedGenesisMinerTransaction()
        //{
        //    const uint genesisMinerNonce = 2083236893;
        //    var genesisMinerTransaction = new UnsignedMinerTransaction
        //    {
        //        Nonce = genesisMinerNonce,
        //        Attributes = new TransactionAttribute[0],
        //        Inputs = new CoinReference[0],
        //        Outputs = new TransactionOutput[0],
        //        Witness = new UnsignedWitness[0]
        //    };

        //    return genesisMinerTransaction;
        //}

        //public UnsignedIssueTransaction BuildUnsignedGenesisIssueTransaction(UInt256 hash, Fixed8 amount)
        //{
        //    var transactionOutput = this.GenesisGoverningTokenTransactionOutput(hash, amount);
        //    var genesisWitness = new WitnessBuilder().BuildUnsignedGenesisWitness();

        //    var issueTransaction = new UnsignedIssueTransaction
        //    {
        //        Attributes = new TransactionAttribute[0],
        //        Inputs = new CoinReference[0],
        //        Outputs = new[] { transactionOutput },
        //        Witness = new[] { genesisWitness }

        //    };

        //    return issueTransaction;
        //}
        #endregion

        //#region Private Methods 
        //private TransactionOutput GenesisGoverningTokenTransactionOutput(UInt256 hash, Fixed8 amount)
        //{
        //    var genesisContract = GenesisValidatorsContract();

        //    var transactionOutput = new TransactionOutput
        //    {
        //        AssetId = hash,
        //        Value = amount,
        //        ScriptHash = genesisContract.ScriptHash
        //    };

        //    return transactionOutput;
        //}

        //private Contract GenesisValidatorsContract()
        //{
        //    var genesisValidators = this.GenesisStandByValidators();
        //    var genesisContract = ContractFactory.CreateMultiplePublicKeyRedeemContract(genesisValidators.Length / 2 + 1, genesisValidators);

        //    return genesisContract;
        //}

        //private ECPoint[] GenesisStandByValidators()
        //{
        //    return this._networkConfig.StandByValidator.Select(u => new ECPoint(u.HexToBytes())).ToArray();
        //}
        //#endregion
    }
}
