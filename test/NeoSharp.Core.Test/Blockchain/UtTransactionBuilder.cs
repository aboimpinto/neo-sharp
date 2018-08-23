using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Models;
using NeoSharp.Core.Models.Builders;

namespace NeoSharp.Core.Test.Blockchain
{
    [TestClass]
    public class UtTransactionBuilder
    {
        [TestMethod]
        public void BuildSignedGoverningTokenRegisterTransaction_HashIsCorrect()
        {
            BinarySerializer.RegisterTypes(typeof(TransactionBase).Assembly, typeof(BlockHeader).Assembly);

            var testee = new TransactionBuilder();
            var governingToken = testee.BuildSignedGoverningTokenRegisterTransaction();

            governingToken.Hash.ToString(true)
                .Should()
                .Be("0xc56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b");
        }
    }
}
