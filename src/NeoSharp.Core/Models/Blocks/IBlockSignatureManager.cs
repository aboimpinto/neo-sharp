using System.Collections.Generic;
using NeoSharp.Core.Models.Transactions;

namespace NeoSharp.Core.Models.Blocks
{
    public interface IBlockSignatureManager
    {
        SignedBlock Sign(Block unsignedBlock);

        SignedBlock Sign(Block unsignedBlock, IReadOnlyList<SignedTransactionBase> signedTransactions);
    }
}
