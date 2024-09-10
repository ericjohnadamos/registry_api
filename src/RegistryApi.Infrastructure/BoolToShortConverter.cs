using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace RegistryApi.Infrastructure;

public class BoolToShortConverter : ValueConverter<bool, int>
{
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    public BoolToShortConverter(ConverterMappingHints mappingHints = null)
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        : base(
            v => Convert.ToInt16(v),
            v => Convert.ToBoolean(v),
            mappingHints)
    {
    }

    public static ValueConverterInfo DefaultInfo { get; }
        = new ValueConverterInfo(typeof(bool), typeof(int), i => new BoolToShortConverter(i.MappingHints));
}