namespace CartridgeBuilder2.Lib.Crt
{
    /// <summary>
    ///     Contains constants pertinent to the CRT file format.
    /// </summary>
    public static class CrtConstants
    {
        /// <summary>
        ///     Header string for a CRT file.
        /// </summary>
        public static string FileId => "C64 CARTRIDGE   ";

        /// <summary>
        ///     Binary representation of CHIP.
        /// </summary>
        public static int ChipId => 0x50494843;
    }
}