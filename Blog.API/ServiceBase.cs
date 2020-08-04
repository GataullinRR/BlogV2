using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace BlogService.API
{
    internal abstract class ServiceBase
    {
        protected readonly JsonSerializerSettings _json_settings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.All,
        };

        readonly HttpClient _client;

        public ServiceBase(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        protected async Task<TResponse> MakeRequestAsync<TRequest, TResponse>(HttpMethod method, string url, TRequest request, JsonSerializerSettings? settings = null)
        {
            var json = JsonConvert.SerializeObject(request, settings ?? _json_settings);
            var stringContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
            var requestMessage = new HttpRequestMessage(method, url)
            {
                Content = stringContent
            };
            var response = await _client.SendAsync(requestMessage);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResponse>(jsonResponse, settings ?? _json_settings);
        }
    }
}
