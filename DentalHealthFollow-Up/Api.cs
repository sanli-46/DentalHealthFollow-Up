namespace DentalHealthFollow_Up.MAUI;

public static class Api
{
    public static HttpClient Client()
        => ServiceHelper.Resolve<IHttpClientFactory>().CreateClient("API");
}
