using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Models.Blocks;
using NeoSharp.Core.Models.Builders;
using NeoSharp.Core.Models.Transactions;

namespace NeoSharp.Core.Test.Models.Builders
{
    [TestClass]
    public class UtBlockBuilder
    {
        [TestMethod]
        public void BuildSignedGenesisBlock_BuildRightGenesisBlock()
        {
            BinarySerializer.RegisterTypes(typeof(RegisterTransaction).Assembly);

            var testee = new BlockBuilder();

            var signedGenisisBlock = testee.BuildSignedGenesisBlock();

            signedGenisisBlock
                .Should()
                .BeOfType<SignedBlock>();
            signedGenisisBlock.Hash.ToString(true)
                .Should()
                .Be("0xd42561e3d30e15be6400b6df2f328e02d2bf6354c41dce433bc57687c82144bf");
        }
    }
}
