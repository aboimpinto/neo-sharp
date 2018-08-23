using System;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Types;
using Newtonsoft.Json;

namespace NeoSharp.Core.Models.Witnesses
{
    [Serializable]
    public class SignedWitness 
    {
        #region Private Fields 
        private readonly Witness _witness;
        #endregion

        #region Public Properties 
        [JsonProperty("txid")]
        public UInt160 Hash { get; }

        [BinaryProperty(0, MaxLength = 65536)]
        [JsonProperty("invocation")]
        public byte[] InvocationScript => this._witness.InvocationScript;

        [BinaryProperty(1, MaxLength = 65536)]
        [JsonProperty("verification")]
        public byte[] VerificationScript => this._witness.VerificationScript;
        #endregion

        #region Constructor 
        public SignedWitness(Witness witness, UInt160 hash)
        {
            this._witness = witness;

            this.Hash = hash;
        }
        #endregion
    }
}
