﻿using System;
using System.Collections.Generic;
using System.Linq;
using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Lib.Builder
{
    [Service(typeof(ITableBuilder))]
    public class TableBuilder : ITableBuilder
    {
        public byte[] Build(ITable table, IEnumerable<IPackedFile> files)
        {
            var values = BuildTableInternal(table, files)
                .Select(i =>
                {
                    if (table.Mask != null)
                        i &= table.Mask.Value;

                    switch (table.Case)
                    {
                        case CaseType.Upper:
                            if (i >= 0x61 && i <= 0x7A)
                                i ^= 0x20;
                            break;
                        case CaseType.Lower:
                            if (i >= 0x41 && i <= 0x5A)
                                i ^= 0x20;
                            break;
                    }

                    return unchecked((byte) i);
                })
                .ToArray();

            var output = new byte[table.Length];
            Array.Copy(values, output, Math.Min(values.Length, table.Length));

            return output;
        }

        private static IEnumerable<int> BuildTableInternal(ITable table, IEnumerable<IPackedFile> files)
        {
            var fileList = files as IList<IPackedFile> ?? files.ToList();
            switch (table.Type)
            {
                case TableType.Bank:
                    return fileList.Select(f => f.StartBank & 0xFF);
                case TableType.Compressed:
                    return fileList.Select(f => (int)f.CompressionMethod & 0xFF);
                case TableType.LengthHigh:
                    return fileList.Select(f => (f.Length >> 8) & 0xFF);
                case TableType.LengthLow:
                    return fileList.Select(f => f.Length & 0xFF);
                case TableType.LoadAddressHigh:
                    if (fileList.Any(f => f.LoadAddress == null))
                        throw new CartridgeBuilderException("Null load addresses can't be used in a table.");
                    // ReSharper disable once PossibleInvalidOperationException
                    return fileList.Select(f => (f.LoadAddress.Value >> 8) & 0xFF);
                case TableType.LoadAddressLow:
                    if (fileList.Any(f => f.LoadAddress == null))
                        throw new CartridgeBuilderException("Null load addresses can't be used in a table.");
                    // ReSharper disable once PossibleInvalidOperationException
                    return fileList.Select(f => f.LoadAddress.Value & 0xFF);
                case TableType.Name:
                    return fileList.Select(f => (int) f.Name.Skip(table.Index).Take(1).FirstOrDefault());
                case TableType.OffsetHigh:
                    return fileList.Select(f => (f.StartAddress >> 8) & 0x7F);
                case TableType.OffsetLow:
                    return fileList.Select(f => f.StartAddress & 0xFF);
                case TableType.OffsetReset:
                case TableType.BankStart:
                    return fileList.Select(f => f.WrapStrategy == WrapStrategy.High ? 0xA0 : 0x80);
                case TableType.OffsetMatch:
                case TableType.BankStop:
                    return fileList.Select(f => f.WrapStrategy != WrapStrategy.Low ? 0xC0 : 0xA0);
                case TableType.StartAddressHigh:
                    return fileList.Select(f => (f.StartAddress >> 8) & 0xFF);
                case TableType.StartAddressLow:
                    return fileList.Select(f => f.StartAddress & 0xFF);
                case TableType.Index:
                    return fileList.Select(f => f.Index & 0xFF);
                case TableType.NameNumber:
                    return fileList.Select(f =>
                    {
                        var result = 0;
                        for (var i = table.Index; i < f.Name.Length; i++)
                        {
                            var value = f.Name[i];
                            if (!(value >= 0x30 && value <= 0x39))
                                break;
                            result *= 10;
                            result += value & 0xF;
                        }

                        return result;
                    });
                default:
                    return Enumerable.Empty<int>();
            }
        }
    }
}