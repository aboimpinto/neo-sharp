using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoSharp.Core.Blockchain.Processors;
using NeoSharp.TestHelpers;

namespace NeoSharp.Core.Test.Blockchain.Processors
{
    [TestClass]
    public class UtBlockProcessor : TestBase
    {
        [TestMethod]
        public void Ctor_CreateValidBlockProcessorObject()
        {
            var testee = this.AutoMockContainer.Create<BlockProcessor>();

            testee
                .Should()
                .BeOfType<BlockProcessor>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddBlock_BlockParameterIsNull_ThrowArgumentNullException()
        {
            var testee = this.AutoMockContainer.Create<BlockProcessor>();

            testee.AddBlock(null);
        }

        //[TestMethod]
        //public void Process_CallsTransactionProcessorPerTransaction()
        //{
        //    var input = Genesis.GenesisBlock;
        //    var txProcessorMock = AutoMockContainer.GetMock<IProcessor<Transaction>>();
        //    var testee = AutoMockContainer.Create<BlockProcessor>();

        //    testee.AddBlock(input);
        //    txProcessorMock.Verify(m => m.Process(It.Is<Transaction>(t => input.Transactions.Contains(t))),
        //        Times.Exactly(input.Transactions.Length));
        //}


        //[TestMethod]
        //public void Process_PersistsBlockHeaderAndIndex()
        //{
        //    var expectedIndex = (uint) RandomInt();
        //    var input = new Block
        //    {
        //        Hash = UInt256.Parse(RandomInt().ToString("X64")),
        //        Index = expectedIndex,
        //        Transactions = new Transaction[0]
        //    };
        //    var repositoryMock = AutoMockContainer.GetMock<IRepository>();
        //    var testee = AutoMockContainer.Create<BlockProcessor>();

        //    testee.AddBlock(input);
        //    repositoryMock.Verify(m => m.SetTotalBlockHeight(expectedIndex));
        //    repositoryMock.Verify(m =>
        //        m.AddBlockHeader(It.Is<BlockHeader>(bh => bh.Index == input.Index && bh.Hash.Equals(input.Hash))));
        //}
    }
}