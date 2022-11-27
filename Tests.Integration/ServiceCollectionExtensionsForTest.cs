using Microsoft.Extensions.DependencyInjection;

namespace Tests.Integration;

public static class ServiceCollectionExtensionsForTest
{
    public static void ReplaceSingleton<T>(this IServiceCollection services, T instance) where T : class
    {
        var d = services.SingleOrDefault(d => d.ServiceType == typeof(T));
        if (d != null) services.Remove(d);
        services.AddSingleton(instance);
    }
}