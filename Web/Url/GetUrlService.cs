namespace YaushServer.Url
{
    public interface IGetUrlService
    {
        Task<ShortenedUrl?> Get(string hash);
    }

    public class GetUrlService(ILogger<GetUrlService> logger, IUrlRepository urlRepository) : IGetUrlService
    {
        private readonly ILogger<GetUrlService> _logger = logger;
        private readonly IUrlRepository _urlRepository = urlRepository;

        public async Task<ShortenedUrl?> Get(string hash)
        {
            _logger.LogInformation("Getting shortened url for {hash}",  hash);
            return await _urlRepository.Get(hash);
        }
    }
}
