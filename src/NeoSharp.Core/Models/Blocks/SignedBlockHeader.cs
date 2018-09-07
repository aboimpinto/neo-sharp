using System;
using System.Collections.Generic;
using NeoSharp.Core.Models.Witnesses;
using NeoSharp.Core.Types;

namespace NeoSharp.Core.Models.Blocks
{
    public class SignedBlockHeader : SignedBlockBase
    {
        #region Constructor
        public SignedBlockHeader() : base()
        {
            // this is need! :(
        }

        public SignedBlockHeader(
            BlockBase blockHeader,
            SignedWitness witness,
            IEnumerable<UInt256> transactionsHashes,
            Func<SignedBlockHeader, UInt256> signedBlockHashCalculatorMethod)
            : base(blockHeader, witness, transactionsHashes, signedBlockHashCalculatorMethod)
        {
            this.SignedBlockHeaderHashCalculator();
        }
        #endregion
    }
}
