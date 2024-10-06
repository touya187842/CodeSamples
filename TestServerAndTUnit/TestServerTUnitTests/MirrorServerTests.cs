using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TestServerTUnitTests;

[ClassConstructor<MirrorTestServerClassConstructor>]
public class MirrorServerTests
{
    private readonly HttpClient HttpClient;

    public MirrorServerTests(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }

    [Before(Test)]
    public async Task BeforeTest()
    {
        await Assert.That(HttpClient).IsNotNull();
    }

    [Test]
    [Arguments("/v1/resource",
        """
        GET /v1/resource
        Host: localhost

        """)]
    [Arguments("/v1a2/resource?id=42",
        """
        GET /v1a2/resource?id=42
        Host: localhost

        """)]
    public async Task Get_Response_Equal(string path, string expectedContent)
    {
        var httpClient = HttpClient;

        var response = await httpClient.GetAsync(path);
        await Assert.That(response).HasMember(x => x.StatusCode).EqualTo(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        await Assert.That(content).IsEqualTo(expectedContent);
    }

    [Test]
    [Arguments("/v1/resource",
        """
        {
          "name": "Touya"
        }
        """,
        """
        POST /v1/resource
        Content-Type: application/json; charset=utf-8
        Content-Length: 23
        Host: localhost

        ew0KICAibmFtZSI6ICJUb3V5YSINCn0=

        """)]
    [Arguments("/v1b1/resource",
        """
        {
          "name": "Touya",
          "value": 42
        }
        """,
        """
        POST /v1b1/resource
        Content-Type: application/json; charset=utf-8
        Content-Length: 39
        Host: localhost

        ew0KICAibmFtZSI6ICJUb3V5YSIsDQogICJ2YWx1ZSI6IDQyDQp9

        """)]
    public async Task PostJson_Response_Equal(string path, string body, string expectedContent)
    {
        var httpClient = HttpClient;

        using var requestContent = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync(path, requestContent);
        await Assert.That(response).HasMember(x => x.StatusCode).EqualTo(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        await Assert.That(content).IsEqualTo(expectedContent);
    }

    [Test]
    [Arguments("/v1/resource",
        new[] { "name", "Touya" },
        """
        POST /v1/resource
        Content-Type: application/x-www-form-urlencoded
        Content-Length: 10
        Host: localhost

        bmFtZT1Ub3V5YQ==

        """)]
    [Arguments("/v2b1/resource",
        new[] { "name", "Touya", "value", "69" },
        """
        POST /v2b1/resource
        Content-Type: application/x-www-form-urlencoded
        Content-Length: 19
        Host: localhost

        bmFtZT1Ub3V5YSZ2YWx1ZT02OQ==

        """)]
    public async Task PostForm_Response_Equal(string path, string[] kvs, string expectedContent)
    {
        var httpClient = HttpClient;
        
        using var requestContent = new FormUrlEncodedContent(kvs.Chunk(2).Select(a => new KeyValuePair<string, string>(a[0], a[1])));
        var response = await httpClient.PostAsync(path, requestContent);
        await Assert.That(response).HasMember(x => x.StatusCode).EqualTo(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        await Assert.That(content).IsEqualTo(expectedContent);
    }
}
