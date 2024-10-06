using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace TestServerAndTUnit;

public abstract class TestServerAndTUnits
{
    public static TestServer GetMirrorTestServer()
        => new TestServer(
            new WebHostBuilder()
            .ConfigureServices(services =>
            {
            })
            .Configure(app =>
            {
                app.UseMiddleware<MirrorMiddleware>();
            }));
}
