using System.Collections.Generic;

namespace NeoSharp.Core.Models.Transactions
{
    public class TransactionBase 
    {
        public TransactionType Type { get; set; }

        public byte Version { get; set; }

        public IList<TransactionAttribute> Attributes { get; set; }

        public IList<CoinReference> Inputs { get; set; }

        public IList<TransactionOutput> Outputs { get; set; }

        public IList<Witnesses.Witness> Witness { get; set; }
    }
}
