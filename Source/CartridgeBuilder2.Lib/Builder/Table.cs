using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Lib.Builder
{
    [Model]
    public class Table : ITable
    {
        public TableType Type { get; set; }
        public int Bank { get; set; }
        public int Offset { get; set; }
        public int Length { get; set; }
        public int Index { get; set; }
        public WrapStrategy WrapStrategy { get; set; }
        public int? Mask { get; set; }
        public CaseType? Case { get; set; }
    }
}