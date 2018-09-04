using System;
using System.Collections.Generic;
using System.Linq;
using NeoSharp.Core.Models.Transactions;
using NeoSharp.Core.Models.Witnesses;
using NeoSharp.Core.Types;

namespace NeoSharp.Core.Models.Blocks
{
    public class SignedBlockHeader : SignedBlockBase
    {
        #region Constructor
        public SignedBlockHeader(
            BlockBase blockHeader,
            SignedWitness witness,
            IEnumerable<SignedTransactionBase> transactions,
            Func<SignedBlockHeader, UInt256> signedBlockHashCalculatorMethod)
            : base(blockHeader, witness, transactions.Select(x => x.Hash), signedBlockHashCalculatorMethod)
        {
            this.SignedBlockHeaderHashCalculator();
        }
        #endregion
    }
}
