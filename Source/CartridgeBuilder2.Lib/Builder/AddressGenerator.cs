using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <inheritdoc />
    [Service(typeof(IAddressGenerator))]
    public class AddressGenerator : IAddressGenerator
    {
        /// <inheritdoc />
        public int GetAddress(int offset) => 
            (offset & 0x3FFF) | 0x8000;

        /// <inheritdoc />
        public int GetBank(int offset) => 
            offset >> 14;
    }
}