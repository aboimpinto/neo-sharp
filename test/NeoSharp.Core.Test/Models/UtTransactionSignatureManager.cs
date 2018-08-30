using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Models.Builders;
using NeoSharp.Core.Models.Transactions;
using NeoSharp.Core.Models.Witnesses;
using NeoSharp.TestHelpers;
using RegisterTransaction = NeoSharp.Core.Models.Transactions.RegisterTransaction;
using SignedRegisterTransaction = NeoSharp.Core.Models.Transactions.SignedRegisterTransaction;

namespace NeoSharp.Core.Test.Models
{
    [TestClass]
    public class UtTransactionSignatureManager : TestBase
    {
        [TestMethod]
        public void Ctor_RegisterTransactionCreated()
        {
            var testee = this.AutoMockContainer.Create<RegisterTransactionSignatureManager>();

            testee
                .Should()
                .BeOfType<RegisterTransactionSignatureManager>();
        }

        [TestMethod]
        public void Sign_GenesisGoverningTokenTransaction_SignedTypeReturnedWithTheRightHash()
        {
            BinarySerializer.RegisterTypes(typeof(RegisterTransaction).Assembly);

            var unsignedRegisterTransaction = new TransactionBuilder()
                .BuildGenesisGoverningTokenRegisterTransaction();

            var crypto = Crypto.Default;
            var witnessSignatureManager = this.AutoMockContainer.Create<WitnessSignatureManager>();
            var binarySerializer = this.AutoMockContainer.Create<BinarySerializer>();

            var testee = new TransactionSignatureManager(crypto, witnessSignatureManager, binarySerializer);
            var signedRegisterTransaction = testee.Sign(unsignedRegisterTransaction);

            signedRegisterTransaction
                .Should()
                .BeOfType<SignedRegisterTransaction>();
            signedRegisterTransaction.Hash.ToString(true)
                .Should()
                .Be("0xc56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b");
        }

        [TestMethod]
        public void Sign_GenesisUtilityTokenTransaction_SignedTypeReturnedWithTheRightHash()
        {
            BinarySerializer.RegisterTypes(typeof(RegisterTransaction).Assembly);

            var unsignedRegisterTransaction = new TransactionBuilder()
                .BuildGenesisUtilityTokenRegisterTransaction();

            var crypto = Crypto.Default;
            var witnessSignatureManager = this.AutoMockContainer.Create<WitnessSignatureManager>();
            var binarySerializer = this.AutoMockContainer.Create<BinarySerializer>();

            var testee = new TransactionSignatureManager(crypto, witnessSignatureManager, binarySerializer);
            var signedRegisterTransaction = testee.Sign(unsignedRegisterTransaction);

            signedRegisterTransaction
                .Should()
                .BeOfType<SignedRegisterTransaction>();
            signedRegisterTransaction.Hash.ToString(true)
                .Should()
                .Be("0x602c79718b16e442de58778e148d0b1084e3b2dffd5de6b7b16cee7969282de7");
        }

        [TestMethod]
        public void Sign_GenesisMinerTransaction_SignedTypeReturnedWithThrRightHash()
        {
            BinarySerializer.RegisterTypes(typeof(RegisterTransaction).Assembly);

            var unsignedMinerTransaction = new TransactionBuilder()
                .BuildGenesisMinerTransaction();

            var crypto = Crypto.Default;
            var witnessSignatureManager = this.AutoMockContainer.Create<WitnessSignatureManager>();
            var binarySerializer = this.AutoMockContainer.Create<BinarySerializer>();

            var testee = new TransactionSignatureManager(crypto, witnessSignatureManager, binarySerializer);
            var signedMinerTransaction = testee.Sign(unsignedMinerTransaction);

            signedMinerTransaction
                .Should()
                .BeOfType<SignedMinerTransaction>();
            signedMinerTransaction.Hash.ToString(true)
                .Should()
                .Be("0xfb5bd72b2d6792d75dc2f1084ffa9e9f70ca85543c717a6b13d9959b452a57d6");
        }

        [TestMethod]
        public void SignGenesisIssueTransaction_SignedTypeReturnedWithTheRightHash()
        {
            BinarySerializer.RegisterTypes(typeof(RegisterTransaction).Assembly);

            var genesisGoverningTokenRegisterTransaction = new TransactionBuilder()
                .BuildGenesisGoverningTokenRegisterTransaction();

            var crypto = Crypto.Default;
            var witnessSignatureManager = new WitnessSignatureManager(crypto);
            var binarySerializer = this.AutoMockContainer.Create<BinarySerializer>();

            var testee = new TransactionSignatureManager(crypto, witnessSignatureManager, binarySerializer);
            var signedRegisterTransaction = testee.Sign(genesisGoverningTokenRegisterTransaction);

            var issueTransaction = new TransactionBuilder()
                .BuildIssueTransaction((SignedRegisterTransaction)signedRegisterTransaction);

            var signedIssuedTransaction = testee.Sign(issueTransaction);

            signedIssuedTransaction
                .Should()
                .BeOfType<SignedIssueTransaction>();
            signedIssuedTransaction.Hash.ToString()
                .Should()
                .Be("0x3631f66024ca6f5b033d7e0809eb993443374830025af904fb51b0334f127cda");
        }
    }
}
