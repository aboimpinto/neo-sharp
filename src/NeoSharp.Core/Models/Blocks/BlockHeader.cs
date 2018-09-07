using System.Collections.Generic;
using NeoSharp.Core.Types;

namespace NeoSharp.Core.Models.Blocks
{
    public class BlockHeader : BlockBase
    {
        #region Public Propeties 
        public IList<UInt256> TransactionHashes { get; set; }
        #endregion

        #region Constructor
        public BlockHeader()
        {
            this.Type = HeaderType.Header;
        }
        #endregion
    }
}
