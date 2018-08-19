using System;

namespace NeoSharp.Core.Models
{
    [Serializable]
    public class SignedWitness : WitnessBase
    {
        #region Constructor 
        public SignedWitness(UnsignedWitness unsignedWitness)
        {
            this.Sign(unsignedWitness);
        }
        #endregion
    }
}
