using Npgsql;

namespace YaushServer.Url
{
    public record UrlShortenedUrl(string OriginalUrl, string Hash);

    public interface IUrlRepository
    {
        public Task<Url?> Get(string hash);
    }

    public class UrlRepository(NpgsqlDataSource _dataSource) : IUrlRepository
    {
        public async Task<Url?> Get(string hash)
        {
            await using var command = _dataSource.CreateCommand("SELECT url, hash FROM links WHERE hash = $1;");

            command.Parameters.Add(new NpgsqlParameter() { Value = hash });
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                string urlStr = reader.GetString(0);
                string hashStr = reader.GetString(1);
                return new(urlStr, hashStr);
            }

            return default;
        }
    }
}
