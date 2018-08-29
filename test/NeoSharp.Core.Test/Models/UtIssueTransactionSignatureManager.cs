using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Models.Builders;
using NeoSharp.Core.Models.Transactions;
using NeoSharp.Core.Models.Witnesses;
using NeoSharp.TestHelpers;

namespace NeoSharp.Core.Test.Models
{
    [TestClass]
    public class UtIssueTransactionSignatureManager : TestBase
    {
        [TestMethod]
        public void Ctor_IssueTransactionSignatureManagerCreated()
        {
            var testee = this.AutoMockContainer.Create<IssueTransactionSignatureManager>();

            testee
                .Should()
                .BeOfType<IssueTransactionSignatureManager>();
        }

        [TestMethod]
        public void SignGenesisIssueTransaction_SignedTypeReturnedWithThrRightHash()
        {
            BinarySerializer.RegisterTypes(typeof(RegisterTransaction).Assembly);

            var genesisGoverningTokenRegisterTransaction = new TransactionBuilder()
                .BuildGenesisGoverningTokenRegisterTransaction();

            var crypto = Crypto.Default;
            var witnessSignatureManager = new WitnessSignatureManager(crypto);
            var binarySerializer = this.AutoMockContainer.Create<BinarySerializer>();

            var registerTransactionSignatureManager = new RegisterTransactionSignatureManager(crypto, witnessSignatureManager, binarySerializer);
            var signedRegisterTransaction = registerTransactionSignatureManager.Sign(genesisGoverningTokenRegisterTransaction);

            var issueTransaction = new TransactionBuilder()
                .BuildIssueTransaction(signedRegisterTransaction);

            var testee = new IssueTransactionSignatureManager(crypto, witnessSignatureManager, binarySerializer);
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
