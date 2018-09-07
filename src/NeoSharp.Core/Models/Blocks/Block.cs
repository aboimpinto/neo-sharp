using System.Collections.Generic;
using NeoSharp.Core.Models.Transactions;

namespace NeoSharp.Core.Models.Blocks
{
    public class Block : BlockBase
    {
        #region Public Properties 
        public IList<TransactionBase> Transactions { get; set; }
        #endregion

        #region Constructor 
        public Block()
        {
            this.Type = HeaderType.Extended;
        }
        #endregion
    }
}
