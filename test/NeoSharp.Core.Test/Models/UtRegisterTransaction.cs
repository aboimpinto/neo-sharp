using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Extensions;
using NeoSharp.Core.Models;
using NeoSharp.Core.Types;
using NeoSharp.VM;
using RegisterTransaction = NeoSharp.Core.Models.Transactions.RegisterTransaction;
using SignedRegisterTransaction = NeoSharp.Core.Models.Transactions.SignedRegisterTransaction;

namespace NeoSharp.Core.Test.Models
{
    [TestClass]
    public class UtRegisterTransaction
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
        public void Sign_ProvideGoverningToken_SignedTypeReturned()
        {
            BinarySerializer.RegisterTypes(typeof(TransactionBase).Assembly, typeof(BlockHeader).Assembly);

            var testee = new RegisterTransaction
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

            var signedResult = testee.Sign();

            signedResult
                .Should()
                .BeOfType<SignedRegisterTransaction>();
            signedResult.Hash.ToString(true)
                .Should()
                .Be("0xc56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b");
        }
    }
}
