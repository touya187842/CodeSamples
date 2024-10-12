using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using System.Threading.Tasks;
using TestServerAndTUnit;

namespace TestServerBenchmarks;

public abstract class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<Benchmark>();
    }
}

public class Benchmark
{
    private TestServer? TestServer;
    private HttpClient? HttpClient;

    [GlobalSetup]
    public void GlobalSetup()
    {
        TestServer = TestServerAndTUnits.GetMirrorTestServer();
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        TestServer?.Dispose();
    }

    [IterationSetup]
    public void IterationSetup()
    {
        HttpClient = TestServer!.CreateClient();
    }

    [Benchmark]
    public async Task Method()
    {
        var httpClient = HttpClient!;
        _ = await httpClient.GetAsync("/api");
    }
}
