using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoSharp.Core.Models.Witnesses;

namespace NeoSharp.Core.Test.Models
{
    [TestClass]
    public class UtWitness
    {
        [TestMethod]
        public void Ctor_WitnessTypeCreated()
        {
            var testee = new Witness();

            testee
                .Should()
                .BeAssignableTo<Witness>();
        }

        [TestMethod]
        public void Sign_SignedTypeReturned()
        {
            //var testee = new Witness
            //{
            //    VerificationScript = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F}
            //};

            //var readOnlyReturn = testee.Sign();

            //readOnlyReturn
            //    .Should()
            //    .BeAssignableTo<ISignedWitnessBase>();
            //readOnlyReturn.Hash
            //    .Should()
            //    .NotBeNull();
            //readOnlyReturn.Hash.ToString()
            //    .Should()
            //    .Be("0xb0c13db07e013a76c84a151b63e31befb6e1917a");
        }
    }
}
