using System.Net.Http;
using System.Threading.Tasks;
using Content.Server._DEN.Redial;
using Robust.Server.ServerStatus;

// ReSharper disable once CheckNamespace
namespace Content.Server.Administration;

public sealed partial class ServerApi
{
    [Dependency] private readonly RedialManager _redialManager = null!;

    public void InitializeDen() => RegisterHandler(HttpMethod.Post, "/admin/actions/setredial", ActionSetRedial);

    /// <summary>
    /// Tells the server to either send a message to connected clients or attempt to redial them
    /// to the specified server.
    /// </summary>
    private async Task ActionSetRedial(IStatusHandlerContext context)
    {
        var body = await ReadJson<ActionSetRedialBody>(context);

        if (body == null)
        {
            await RespondBadRequest(context, "Body not provided");
            return;
        }

        await RunOnMainThread(() =>
        {
            var redialSystem = _entityManager.System<RedialSystem>();

            if (string.IsNullOrEmpty(body.Address) || string.IsNullOrEmpty(body.Message))
            {
                _redialManager.ClearSavedRedial();
                return;
            }
            
            redialSystem.SetSavedRedial(body.Address, body.Message);
        });

        await RespondOk(context);
    }
}

public sealed class ActionSetRedialBody
{
    public required string Address { get; init; }
    public required string Message { get; init; }
}
