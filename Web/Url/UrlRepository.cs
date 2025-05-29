using Microsoft.EntityFrameworkCore;
using Npgsql;
using YaushServer.Infrastructure;

namespace YaushServer.Url
{
    public interface IUrlRepository
    {
        public Task<ShortenedUrl?> Get(string hash);
    }

    //public class UrlRepository(NpgsqlDataSource _dataSource) : IUrlRepository
    //{
    //    public async Task<Url?> Get(string hash)
    //    {
    //        await using var command = _dataSource.CreateCommand("SELECT url, hash FROM urls WHERE hash = $1;");

    //        command.Parameters.Add(new NpgsqlParameter() { Value = hash });
    //        using var reader = await command.ExecuteReaderAsync();
    //        while (await reader.ReadAsync())
    //        {
    //            string urlStr = reader.GetString(0);
    //            string hashStr = reader.GetString(1);
    //            return new(urlStr, hashStr);
    //        }

    //        return default;
    //    }
    //}

    public class UrlRepository(YaushDbContext _dbContext) : IUrlRepository
    {
        public async Task<ShortenedUrl?> Get(string hash)
        {
            return await _dbContext.Urls.FirstOrDefaultAsync(x => x.Hash == hash);
        }
    }
}
