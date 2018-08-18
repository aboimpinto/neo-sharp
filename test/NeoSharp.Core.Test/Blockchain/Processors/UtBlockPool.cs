using System;
using System.Reflection;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NeoSharp.Core.Blockchain.Processors;
using NeoSharp.Core.Models;
using NeoSharp.TestHelpers;

namespace NeoSharp.Core.Test.Blockchain.Processors
{
    [TestClass]
    public class UtBlockPool : TestBase
    {
        [TestMethod]
        public void Add_AddValidBlock_PoolHasOneElementAndOnAddedEventFired()
        {
            // Arrange
            var addedBlockMock = new Mock<Block>();
            var testee = this.AutoMockContainer.Create<BlockPool>();

            var onAddedFired = false;
            testee.OnAdded += (sender, e) => { onAddedFired = true; };

            // Act 
            testee.Add(addedBlockMock.Object);

            // Assert
            testee.Size
                .Should()
                .Be(1);
            onAddedFired
                .Should()
                .BeTrue();
            addedBlockMock.Verify(x => x.UpdateHash());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Add_NullBlock_ArgumetNullExceptionThrown()
        {
            // Arrange
            var testee = this.AutoMockContainer.Create<BlockPool>();

            // Act 
            testee.Add(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Add_TwiceSameBlock_InvalidOperationExceptionThrown()
        {
            // Arrange
            var addedBlockMock = new Mock<Block>();
            var testee = this.AutoMockContainer.Create<BlockPool>();

            // Act 
            testee.Add(addedBlockMock.Object);
            testee.Add(addedBlockMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Add_GenesisBlockWithIndexDifferentFromZero_InvalidOperationExceptionThrown()
        {
            // Arrange
            var addedBlockMock = new Mock<Block>();

            var indexField = typeof(Block).GetField("Index", BindingFlags.Instance | BindingFlags.Public);
            indexField.SetValue(addedBlockMock.Object, (uint)1);

            var testee = this.AutoMockContainer.Create<BlockPool>();

            // Act 
            testee.Add(addedBlockMock.Object);
        }
    }
}
