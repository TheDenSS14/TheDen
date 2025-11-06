using System.Net;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Content.Shared._DEN.Hub;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;

namespace Content.Server._DEN.Hub;

public sealed class HubManager
{
    [Dependency] private readonly IHttpClientHolder _holder = null!;
    [Dependency] private readonly IPrototypeManager _protoManager = null!;
    [Dependency] private readonly ILogManager _logManager = null!;

    public event Action? OnServersRefreshed;

    private const string StatusRoute = "/status";

    private Dictionary<string, HubServer> _servers = new();
    private ISawmill _sawmill = null!;

    public void Initialize()
    {
        _sawmill = _logManager.GetSawmill("hub-manager");

        _protoManager.PrototypesReloaded += OnPrototypesReloaded;
        PopulateServers();
    }

    private void OnPrototypesReloaded(PrototypesReloadedEventArgs args)
    {
        if (!args.WasModified<HubServerPrototype>())
            return;

        PopulateServers();
    }

    public void Refresh() => Task.Run(async () => await RefreshServers());

    public Dictionary<string, HubServer> Servers => _servers;

    private async Task RefreshServers()
    {
        foreach (var serverPair in _servers)
            await RefreshServer(serverPair.Value);
    }

    private async Task RefreshServer(HubServer server)
    {
        var connectionString = server.ConnectAddress;

        if (string.IsNullOrWhiteSpace(connectionString))
            return;

        try
        {
            var response = await _holder.Client.GetAsync($"{connectionString}{StatusRoute}");
            var statusJson = await response.Content.ReadAsStringAsync();
            var status = JsonNode.Parse(statusJson);
            var updatedServer = TryParseJson(server, status);

            if (updatedServer == null)
                return;

            _servers[server.ServerId] = updatedServer;
            OnServersRefreshed?.Invoke();
        }
        catch (HttpRequestException httpException)
        {
            if (httpException.StatusCode != HttpStatusCode.NotFound)
                return;

            var newStatus = server with
            {
                MaxPlayers = null,
                Players = null,
                IsOnline = false
            };

            _servers[server.ServerId] = newStatus;
            OnServersRefreshed?.Invoke();
        }
        catch (Exception e)
        {
            _sawmill.Error($"Failed to refresh server:\n{e}");
        }
    }

    private HubServer? TryParseJson(HubServer server, JsonNode? node)
    {
        if (node == null || node["players"] == null || node["soft_max_players"] == null)
            return null;

        var players = node["players"]?.GetValue<int>() ?? 0;
        var maxPlayers = node["soft_max_players"]?.GetValue<int>() ?? 0;

        return server with
        {
            Players = players,
            MaxPlayers = maxPlayers,
            IsOnline = true
        };
    }

    private string SanitizeConnectAddress(string address)
    {
        if (address.EndsWith('/'))
            address = address.Substring(0, address.Length - 1);

        if (!address.StartsWith("http://") && !address.StartsWith("https://"))
            throw new InvalidOperationException("Connect address must start with an HTTP protocol!");

        return address;
    }

    private void PopulateServers()
    {
        _servers.Clear();

        foreach (var server in _protoManager.EnumeratePrototypes<HubServerPrototype>())
        {
            var displayName = Loc.GetString(server.DisplayName);
            var connectAddress = SanitizeConnectAddress(server.ConnectionString);
            var entry = new HubServer(
                server.ID,
                connectAddress,
                displayName,
                0,
                0,
                false,
                server.CanConnect);

            _servers[entry.ServerId] = entry;
        }

        Refresh();
    }
}
