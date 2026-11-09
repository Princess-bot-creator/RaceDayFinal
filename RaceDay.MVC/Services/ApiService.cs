using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RaceDay.MVC.Services;
public class ApiService
{
    private readonly IHttpClientFactory _factory;
    private readonly IHttpContextAccessor _http;
    public ApiService(IHttpClientFactory f, IHttpContextAccessor h) { _factory = f; _http = h; }
    private HttpClient Client()
    {
        var c = _factory.CreateClient("RaceDayApi");
        var token = _http.HttpContext?.Session.GetString("JWT");
        if (!string.IsNullOrEmpty(token))
            c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return c;
    }
    public async Task<T?> GetAsync<T>(string url)
    {
        var res = await Client().GetAsync(url);
        if (!res.IsSuccessStatusCode) return default;
        return JsonSerializer.Deserialize<T>(await res.Content.ReadAsStringAsync(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
    public async Task<(bool ok, string body)> PostJsonAsync(string url, object data)
    {
        var json = JsonSerializer.Serialize(data);
        var res = await Client().PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
        return (res.IsSuccessStatusCode, await res.Content.ReadAsStringAsync());
    }
    public async Task<bool> PostFormAsync(string url, MultipartFormDataContent form)
        => (await Client().PostAsync(url, form)).IsSuccessStatusCode;
    public async Task<bool> PutFormAsync(string url, MultipartFormDataContent form)
        => (await Client().PutAsync(url, form)).IsSuccessStatusCode;
    public async Task<bool> DeleteAsync(string url)
        => (await Client().DeleteAsync(url)).IsSuccessStatusCode;
}