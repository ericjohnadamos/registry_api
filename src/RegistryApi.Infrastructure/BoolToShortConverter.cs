namespace RegistryApi.Infrastructure;

using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
public class BoolToShortConverter(ConverterMappingHints mappingHints = null) : ValueConverter<bool, int>(
        v => Convert.ToInt16(v),
        v => Convert.ToBoolean(v),
        mappingHints)
{
    public static ValueConverterInfo DefaultInfo { get; }
        = new ValueConverterInfo(typeof(bool), typeof(int), i => new BoolToShortConverter(i.MappingHints));
}
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.