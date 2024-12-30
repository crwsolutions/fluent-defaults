using FluentDefaults;
using FluentDetails.Sample.Model;

namespace FluentDetails.Sample.Defaults;

public class PersonAsyncDefaulter : AbstractAsyncDefaulter<Person>
{
    private readonly HttpClient _httpClient;

    public PersonAsyncDefaulter(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("DefaultsClient");

        DefaultFor(x => x.Id).Is(() => Guid.NewGuid());
        DefaultFor(x => x.IsVip).Is(false);
        DefaultFor(x => x.Discount).IsAsync(GetDefaultDiscount);
    }

    private async Task<decimal?> GetDefaultDiscount()
    {
        var response = await _httpClient.GetAsync("/default-discount");
        response.EnsureSuccessStatusCode();
        var n = await response.Content.ReadAsStringAsync();
        return decimal.Parse(n);
    }
}

