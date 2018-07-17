using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Extensions;
using NeoSharp.Core.Messaging.Messages;
using NeoSharp.Core.Network.Protocols;
using NeoSharp.TestHelpers;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NeoSharp.Core.Test.Network.Protocols
{
    [TestClass]
    public class UtProtocolV1 : TestBase
    {
        [TestInitialize]
        public void WarmSerializer()
        {
            AutoMockContainer.Register<IBinaryConverter>(new BinaryConverter(typeof(VersionMessage).Assembly));
        }

        [TestMethod]
        public async Task Can_serialize_and_deserialize_messages()
        {
            // Arrange 
            var tcpProtocol = AutoMockContainer.Create<ProtocolV1>();
            var expectedVerAckMessage = new VerAckMessage();
            VerAckMessage actualVerAckMessage;

            // Act
            using (var memory = new MemoryStream())
            {
                await tcpProtocol.SendMessageAsync(memory, expectedVerAckMessage, CancellationToken.None);
                memory.Seek(0, SeekOrigin.Begin);
                actualVerAckMessage = (VerAckMessage)await tcpProtocol.ReceiveMessageAsync(memory, CancellationToken.None);
            }

            // Assert
            actualVerAckMessage
                .Should()
                .NotBeNull();
            actualVerAckMessage.Command
                .Should()
                .Be(expectedVerAckMessage.Command);
        }

        [TestMethod]
        public async Task Can_serialize_and_deserialize_messages_with_payload()
        {
            // Arrange 
            var versionPayload = new VersionPayload
            {
                Version = (uint) Rand.Next(0, int.MaxValue),
                Services = (ulong) Rand.Next(0, int.MaxValue),
                Timestamp = DateTime.UtcNow.ToTimestamp(),
                Port = (ushort) Rand.Next(0, short.MaxValue),
                Nonce = (uint) Rand.Next(0, int.MaxValue),
                UserAgent = $"/NEO:{Rand.Next(1, 10)}.{Rand.Next(1, 100)}.{Rand.Next(1, 1000)}/",
                CurrentBlockIndex = (uint) Rand.Next(0, int.MaxValue),
                Relay = false
            };

            var tcpProtocol = AutoMockContainer.Create<ProtocolV1>();
            var expectedVersionMessage = new VersionMessage(versionPayload);
            VersionMessage actualVersionMessage;

            // Act
            using (var memory = new MemoryStream())
            {
                await tcpProtocol.SendMessageAsync(memory, expectedVersionMessage, CancellationToken.None);
                memory.Seek(0, SeekOrigin.Begin);
                actualVersionMessage = (VersionMessage)await tcpProtocol.ReceiveMessageAsync(memory, CancellationToken.None);
            }

            // Assert
            actualVersionMessage
                .Should()
                .NotBeNull();

            actualVersionMessage.Command
                .Should()
                .Be(expectedVersionMessage.Command);

            actualVersionMessage.Payload
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(versionPayload);
        }
    }
}