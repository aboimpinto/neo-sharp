using System.Collections.Generic;

namespace NeoSharp.Core.Models.Transactions
{
    public interface ITransactionBase : ISignable<ISignedTransactionBase>
    {
        TransactionType Type { get; set; }

        byte Version { get; set; }

        IList<TransactionAttribute> Attributes { get; set; }

        IList<CoinReference> Inputs { get; set; }

        IList<TransactionOutput> Outputs { get; set; }

        IList<Witnesses.Witness> Witness { get; set; }
    }
}
