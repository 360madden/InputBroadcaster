using System.Text.Json;

namespace InputBroadcaster.Configuration;

public sealed class JsonConfigurationStore
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true,
    };

    public AppConfiguration LoadOrDefault(string json)
    {
        return JsonSerializer.Deserialize<AppConfiguration>(json, SerializerOptions) ?? AppConfiguration.Default;
    }

    public string Save(AppConfiguration configuration)
    {
        return JsonSerializer.Serialize(configuration, SerializerOptions);
    }
}
