namespace YaushServer.Url
{
    public static class Endpoints
    {
        public static void RegisterUrlEndpoints(this WebApplication app)
        {
            app.MapGet("/create", async (ICreateUrlService createUrlService) =>
            {
                await createUrlService.CreateUrl("https://google.com");
            });

            app.MapGet("/get/{hash}", async (string hash, IGetUrlService getUrlService) =>
            {
                return await getUrlService.Get(hash);
            });
        }
    }
}
