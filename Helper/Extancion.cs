﻿namespace Frontend.Helper
{
    public static class Extancion
    {
        public static HttpClient Client { get; set; }
        //public static IConfiguration _configuration{get; }

        public static HttpClient InitializeClientBaseAddress(this IServiceCollection services, IConfiguration configuration)
        {
            var result = "http://78.111.111.81";
            Client = new HttpClient();

            Client.BaseAddress = new Uri(result.ToString());

            return Client;
        }
        public static string UriAddress(string uri)
        {
            Client = new HttpClient();
            string url = Client.BaseAddress + uri;
            //string baseAddress = await Client.GetStringAsync(url);

            //Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")); 
            return url;
        }
    }
}
