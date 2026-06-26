using System.Reflection;

namespace ApiGift.Sdk.V2;

internal static class ApiGiftSdkVersion
{
    public static readonly string Current = GetCurrentVersion();

    private static string GetCurrentVersion()
    {
        Assembly assembly = typeof(ApiGiftClient).Assembly;
        string? informationalVersion = assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion;

        if (!string.IsNullOrWhiteSpace(informationalVersion))
        {
            return informationalVersion.Split('+')[0];
        }

        return assembly.GetName().Version?.ToString(3) ?? "1.0.0";
    }
}
