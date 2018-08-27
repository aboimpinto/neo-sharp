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

            var unsignedRegisterTransaction = new TransactionBuilder()
                .BuildGenesisGoverningTokenRegisterTransaction();

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
            BinarySerializer.RegisterTypes(typeof(RegisterTransaction).Assembly);

            var unsignedRegisterTransaction = new TransactionBuilder()
                .BuildGenesisUtilityTokenRegisterTransaction();

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
