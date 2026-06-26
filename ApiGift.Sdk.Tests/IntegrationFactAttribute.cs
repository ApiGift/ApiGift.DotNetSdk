using Xunit;

namespace ApiGift.Sdk.Tests;

[AttributeUsage(AttributeTargets.Method)]
public sealed class IntegrationFactAttribute : FactAttribute
{
    private static readonly string[] RequiredVariables =
    [
        "APIGIFT_BASE_URL",
        "APIGIFT_ACCESS_KEY",
        "APIGIFT_SECRET_KEY"
    ];

    public IntegrationFactAttribute()
    {
        bool enabled = string.Equals(
            Environment.GetEnvironmentVariable("APIGIFT_RUN_INTEGRATION_TESTS"),
            "true",
            StringComparison.OrdinalIgnoreCase);
        bool configured = RequiredVariables.All(
            variable => !string.IsNullOrWhiteSpace(
                Environment.GetEnvironmentVariable(variable)));

        if (!enabled || !configured)
        {
            Skip =
                "Set APIGIFT_RUN_INTEGRATION_TESTS=true and all ApiGift credentials to run integration tests.";
        }
    }
}
