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
            var witnessSignatureManager = this.AutoMockContainer.Create<WitnessSignatureManager>();
            var binarySerializer = this.AutoMockContainer.Create<BinarySerializer>();

            var registerTransactionSignatureManager = new RegisterTransactionSignatureManager(crypto, witnessSignatureManager, binarySerializer);
            var signedRegisterTransaction = registerTransactionSignatureManager.Sign(genesisGoverningTokenRegisterTransaction);

            var issueTransaction = new TransactionBuilder()
                .BuildIssueTransaction(signedRegisterTransaction);

            var testee = this.AutoMockContainer.Create<IssueTransactionSignatureManager>();
            var signedIssuedTransaction = testee.Sign(issueTransaction);

            signedRegisterTransaction
                .Should()
                .BeOfType<SignedIssueTransaction>();
        }
    }
}
