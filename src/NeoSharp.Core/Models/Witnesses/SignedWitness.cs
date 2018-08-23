using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Types;

namespace NeoSharp.Core.Models.Witnesses
{
    public class SignedWitness : ISignedWitnessBase
    {
        #region Private Fields 
        private readonly IWitnessBase _readOnlyWitnessBase;
        #endregion

        #region ISignedWitnessBase implementation 
        public UInt160 Hash { get; private set; }

        public byte[] InvocationScript => this._readOnlyWitnessBase.InvocationScript;

        public byte[] VerificationScript => this._readOnlyWitnessBase.VerificationScript;
        #endregion

        #region Constructor 
        public SignedWitness(IWitnessBase readOnlyWitnessBase)
        {
            this._readOnlyWitnessBase = readOnlyWitnessBase;

            this.Hash = new UInt160(Crypto.Default.Hash160(this.VerificationScript));
        }
        #endregion
    }
}
