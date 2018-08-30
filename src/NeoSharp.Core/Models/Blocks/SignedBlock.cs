using System;
using System.Collections.Generic;
using NeoSharp.Core.Models.Transactions;
using NeoSharp.Core.Models.Witnesses;
using NeoSharp.Core.Types;

namespace NeoSharp.Core.Models.Blocks
{
    public class SignedBlock : SignedBlockBase
    {
        #region Constructor
        public SignedBlock(
            Block block, 
            SignedWitness witness, 
            IReadOnlyList<SignedTransactionBase> transactions, 
            Func<SignedBlockBase, UInt256> hashCalculateMethod) 
            : base(block, witness, transactions, hashCalculateMethod)
        {
        }
        #endregion
    }
}
