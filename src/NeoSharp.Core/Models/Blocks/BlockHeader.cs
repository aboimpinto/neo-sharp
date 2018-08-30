namespace NeoSharp.Core.Models.Blocks
{
    public class BlockHeader : BlockBase
    {
        #region Constructor
        public BlockHeader()
        {
            this.Type = HeaderType.Header;
        }
        #endregion
    }
}
