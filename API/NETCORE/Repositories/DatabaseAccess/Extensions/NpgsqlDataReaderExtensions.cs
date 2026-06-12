using System.Globalization;
using Npgsql;

namespace HORUSPDV_API.Repositories.DatabaseAccess;

public static class NpgsqlDataReaderExtensions
{
    public static DateTimeOffset GetDateTimeOffset(this NpgsqlDataReader reader, int ordinal)
    {
        if (reader.IsDBNull(ordinal))
        {
            return default;
        }

        var value = reader.GetValue(ordinal);

        return value switch
        {
            DateTimeOffset dto => dto,

            DateTime dt => dt.Kind switch
            {
                DateTimeKind.Utc => new DateTimeOffset(dt),
                DateTimeKind.Local => new DateTimeOffset(dt),
                _ => new DateTimeOffset(DateTime.SpecifyKind(dt, DateTimeKind.Utc))
            },

            string text when DateTimeOffset.TryParse(
                text,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal,
                out var parsed
            ) => parsed,

            _ => ConvertToDateTimeOffset(value)
        };
    }

    private static DateTimeOffset ConvertToDateTimeOffset(object value)
    {
        var dateTime = Convert.ToDateTime(value, CultureInfo.InvariantCulture);

        if (dateTime.Kind == DateTimeKind.Unspecified)
        {
            dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }

        return new DateTimeOffset(dateTime);
    }
}
