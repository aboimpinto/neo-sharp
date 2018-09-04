using System;
using System.Collections.Generic;
using System.Linq;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Models.Transactions;
using NeoSharp.Core.Models.Witnesses;
using NeoSharp.Core.Types;

namespace NeoSharp.Core.Models.Blocks
{
    [Serializable]
    public class SignedBlock : SignedBlockBase
    {
        #region Public Properties 
        [BinaryProperty(100, MaxLength = 0x10000, Override = true)]
        public SignedTransactionBase[] Transactions { get; }
        #endregion

        #region Constructor
        public SignedBlock(
            BlockBase block, 
            SignedWitness witness, 
            IReadOnlyList<SignedTransactionBase> transactions, 
            Func<SignedBlock, UInt256> signedBlockHashCalculatorMethod) 
            : base(block, witness, transactions.Select(x => x.Hash), signedBlockHashCalculatorMethod)
        {
            this.Transactions = transactions.ToArray();

            this.SignedBlockHashCalculator();
        }
        #endregion
    }
}
