using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Types;

namespace NeoSharp.Core.Models.Witnesses
{
    public class WitnessSignatureManager : IWitnessSignatureManager
    {
        #region Private Fields 
        private readonly Crypto _crypto;
        #endregion

        #region Constructor 
        public WitnessSignatureManager(Crypto crypto)
        {
            this._crypto = crypto;
        }
        #endregion

        #region IWitnessSignatureManager Implementation 
        public SignedWitness SignWitness(Witness witness)
        {
            var hash = new UInt160(this._crypto.Hash160(witness.VerificationScript));

            return new SignedWitness(witness, hash);
        }
        #endregion
    }
}
