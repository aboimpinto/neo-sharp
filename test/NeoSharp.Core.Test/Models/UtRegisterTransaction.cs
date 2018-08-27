using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Extensions;
using NeoSharp.Core.Models;
using NeoSharp.Core.Models.Transactions;
using NeoSharp.Core.Models.Witnesses;
using NeoSharp.Core.Types;
using NeoSharp.TestHelpers;
using NeoSharp.VM;
using RegisterTransaction = NeoSharp.Core.Models.Transactions.RegisterTransaction;
using SignedRegisterTransaction = NeoSharp.Core.Models.Transactions.SignedRegisterTransaction;

namespace NeoSharp.Core.Test.Models
{
    [TestClass]
    public class UtRegisterTransaction : TestBase
    {
        [TestMethod]
        public void Ctor_RegisterTransactionCreated()
        {
            var testee = new RegisterTransaction();

            testee
                .Should()
                .BeOfType<RegisterTransaction>();
        }

        [TestMethod]
        public void Sign_ProvideGoverningToken_SignedTypeReturnedWithTheRightHash()
        {
            BinarySerializer.RegisterTypes(typeof(RegisterTransaction).Assembly);

            var unsignedRegisterTransaction = new RegisterTransaction
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
                Witness = new List<Core.Models.Witnesses.Witness>()
            };

            var crypto = Crypto.Default;
            var witnessSignatureManager = this.AutoMockContainer.Create<WitnessSignatureManager>();
            var binarySerializer = this.AutoMockContainer.Create<BinarySerializer>();

            var testee = new RegisterTransactionSignatureManager(crypto, witnessSignatureManager, binarySerializer);
            var signedRegisterTransaction = testee.Sign(unsignedRegisterTransaction);

            signedRegisterTransaction
                .Should()
                .BeOfType<SignedRegisterTransaction>();
            signedRegisterTransaction.Hash.ToString(true)
                .Should()
                .Be("0xc56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b");
        }

        [TestMethod]
        public void Sign_ProvideUtilityToken_SignedTypeReturnedWithTheRightHash()
        {
            const uint decrementInterval = 2000000;
            uint[] gasGenerationPerBlock = { 8, 7, 6, 5, 4, 3, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            BinarySerializer.RegisterTypes(typeof(RegisterTransaction).Assembly);

            var unsignedRegisterTransaction = new RegisterTransaction
            {
                AssetType = AssetType.UtilityToken,
                Name = "[{\"lang\":\"zh-CN\",\"name\":\"小蚁币\"},{\"lang\":\"en\",\"name\":\"AntCoin\"}]",
                Amount = Fixed8.FromDecimal(gasGenerationPerBlock.Sum(p => p) * decrementInterval),
                Precision = 8,
                Owner = ECPoint.Infinity,
                Admin = new[] { (byte)EVMOpCode.PUSH0 }.ToScriptHash(),         //TODO: Why this? Check with people
                Attributes = new TransactionAttribute[0],
                Inputs = new List<CoinReference>(),
                Outputs = new List<TransactionOutput>(),
                Witness = new List<Core.Models.Witnesses.Witness>()
            };

            var crypto = Crypto.Default;
            var witnessSignatureManager = this.AutoMockContainer.Create<WitnessSignatureManager>();
            var binarySerializer = this.AutoMockContainer.Create<BinarySerializer>();

            var testee = new RegisterTransactionSignatureManager(crypto, witnessSignatureManager, binarySerializer);
            var signedRegisterTransaction = testee.Sign(unsignedRegisterTransaction);

            signedRegisterTransaction
                .Should()
                .BeOfType<SignedRegisterTransaction>();
            signedRegisterTransaction.Hash.ToString(true)
                .Should()
                .Be("0x602c79718b16e442de58778e148d0b1084e3b2dffd5de6b7b16cee7969282de7");
        }
    }
}
