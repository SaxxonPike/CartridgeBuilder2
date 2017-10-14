using System.Collections.Generic;
using CartridgeBuilder2.Lib.Prg;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <summary>
    /// Packs files into a ROM.
    /// </summary>
    public interface IFilePacker
    {
        /// <summary>
        /// Pack the specified files into the ROM.
        /// </summary>
        IEnumerable<IPackedFile> Pack(IRomSpace romSpace, IEnumerable<IFile> files);
    }
}