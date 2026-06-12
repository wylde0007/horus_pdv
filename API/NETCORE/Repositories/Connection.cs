/**
 * Arquivo: API/NETCORE/Repositories/Connection.cs
 * Objetivo: centraliza a criação de conexões PostgreSQL/Supabase usadas pelos repositórios da API.
 */
using Npgsql;

namespace HORUSPDV_API.Repositories;

public sealed class Connection(IConfiguration configuration)
{
    public string ConnectionString { get; } = ResolveConnectionString(configuration);

    public async Task<NpgsqlConnection> OpenConnectionAsync(
        CancellationToken cancellationToken = default,
        string? database = null)
    {
        var connectionString = string.IsNullOrWhiteSpace(database)
            ? ConnectionString
            : BuildConnectionString(database);

        var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }

    public NpgsqlConnection OpenConnection(string? database = null)
    {
        var connectionString = string.IsNullOrWhiteSpace(database)
            ? ConnectionString
            : BuildConnectionString(database);

        var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        return connection;
    }

    private string BuildConnectionString(string database)
    {
        var builder = new NpgsqlConnectionStringBuilder(ConnectionString)
        {
            Database = database
        };

        return builder.ConnectionString;
    }

    private static string ResolveConnectionString(IConfiguration configuration)
    {
        var connectionString =
            FirstNonEmpty(
                Environment.GetEnvironmentVariable("HORUSPDV_CONNECTION_STRING"),
                configuration.GetConnectionString("HorusPdv"),
                configuration.GetConnectionString("DefaultConnection"));

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string não configurada. Defina HORUSPDV_CONNECTION_STRING ou ConnectionStrings:HorusPdv.");
        }

        return connectionString;
    }

    private static string? FirstNonEmpty(params string?[] values)
        => values.FirstOrDefault(value => !string.IsNullOrWhiteSpace(value));
}
