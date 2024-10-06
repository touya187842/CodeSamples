
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using TestServerAndTUnit;
using TUnit.Core.Interfaces;

namespace TestServerTUnitTests;

public class MirrorTestServerClassConstructor : IClassConstructor
{
    private readonly IServiceProvider ServiceProvider;

    public MirrorTestServerClassConstructor()
    {
        ServiceProvider = new ServiceCollection()
            .AddScoped<MirrorServerTests>()
            .AddSingleton(provider => TestServerAndTUnits.GetMirrorTestServer())
            .AddTransient(provider => provider.GetRequiredService<TestServer>().CreateClient())
            .BuildServiceProvider();
    }
    public T Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>() where T : class
    {
        return ServiceProvider.GetRequiredService<T>();
    }

    public Task DisposeAsync<T>(T t)
    {
        return Task.CompletedTask;
    }
}
