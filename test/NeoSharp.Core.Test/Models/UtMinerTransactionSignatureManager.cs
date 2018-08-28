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
    public class UtMinerTransactionSignatureManager : TestBase
    {
        [TestMethod]
        public void Ctor_MinerTransactionSignatureManagerCreated()
        {
            var testee = this.AutoMockContainer.Create<MinerTransactionSignatureManager>();

            testee
                .Should()
                .BeOfType<MinerTransactionSignatureManager>();
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

            var testee = new MinerTransactionSignatureManager(crypto, witnessSignatureManager, binarySerializer);
            var signedMinerTransaction = testee.Sign(unsignedMinerTransaction);

            signedMinerTransaction
                .Should()
                .BeOfType<SignedMinerTransaction>();
            signedMinerTransaction.Hash.ToString(true)
                .Should()
                .Be("0xfb5bd72b2d6792d75dc2f1084ffa9e9f70ca85543c717a6b13d9959b452a57d6");
        }
    }
}
