using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Models.Blocks;
using NeoSharp.TestHelpers;

namespace NeoSharp.Core.Test.Models
{
    [TestClass]
    public class UtBlockSignatureManager : TestBase
    {
        [TestMethod]
        public void DeserializeBlockHeader_GenesisBlockHeaderByteArray_ReturnSignedBlockHeader()
        {
            this.AutoMockContainer.Register(Crypto.Default);

            var rawGenesisBlockHeader = new byte[]
            {
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 244, 27, 192, 54, 227, 155, 13, 107, 5, 121, 200, 81, 198, 253, 232, 58, 248, 2, 250, 78, 87, 190,
                192, 188, 51, 101, 234, 227, 171, 244, 63, 128, 101, 252, 136, 87, 0, 0, 0, 0, 29, 172, 43, 124, 0, 0,
                0, 0, 89, 231, 93, 101, 43, 93, 56, 39, 191, 4, 193, 101, 187, 233, 239, 149, 204, 164, 191, 85, 1, 0,
                1, 81, 4, 214, 87, 42, 69, 155, 149, 217, 19, 107, 122, 113, 60, 84, 133, 202, 112, 159, 158, 250, 79,
                8, 241, 194, 93, 215, 146, 103, 45, 43, 215, 91, 251, 155, 124, 255, 218, 166, 116, 190, 174, 15, 147,
                14, 190, 96, 133, 175, 144, 147, 229, 254, 86, 179, 74, 92, 34, 12, 205, 207, 110, 252, 51, 111, 197,
                231, 45, 40, 105, 121, 238, 108, 177, 183, 230, 93, 253, 223, 178, 227, 132, 16, 11, 141, 20, 142, 119,
                88, 222, 66, 228, 22, 139, 113, 121, 44, 96, 218, 124, 18, 79, 51, 176, 81, 251, 4, 249, 90, 2, 48, 72,
                55, 67, 52, 153, 235, 9, 8, 126, 61, 3, 91, 111, 202, 36, 96, 246, 49, 54,
            };

            var testee = this.AutoMockContainer.Create<BlockSignatureManager>();
            var signedGenesisBlockHeader = testee.Deserialize(rawGenesisBlockHeader);

            signedGenesisBlockHeader
                .Should()
                .BeOfType(typeof(SignedBlockHeader));
            signedGenesisBlockHeader.Hash.ToString(true)
                .Should()
                .Be("0xd42561e3d30e15be6400b6df2f328e02d2bf6354c41dce433bc57687c82144bf");
            signedGenesisBlockHeader.TransactionHashes.Length
                .Should()
                .Be(4);
        }
    }
}
