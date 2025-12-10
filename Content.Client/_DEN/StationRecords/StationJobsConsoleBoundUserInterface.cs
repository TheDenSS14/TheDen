using Content.Client.StationRecords;
using Content.Shared._DEN.StationRecords;
using Content.Shared.Roles;
using Content.Shared.StationRecords;
using Robust.Client.UserInterface;
using Robust.Shared.Prototypes;


namespace Content.Client._DEN.StationRecords;


public sealed class StationJobsConsoleBoundUserInterface : BoundUserInterface
{
    [ViewVariables]
    private StationJobsConsoleWindow? _window = default!;

    public StationJobsConsoleBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey) { }

    protected override void Open()
    {
        base.Open();

        _window = this.CreateWindow<StationJobsConsoleWindow>();
        _window.OnJobAdd += OnJobsAdd; // Frontier: job modification buttons
        _window.OnJobSubtract += OnJobsSubtract; // Frontier: job modification buttons
    }

    // Frontier: job modification buttons, ship advertisements
    private void OnJobsAdd(string job)
    {
        SendMessage(new AdjustStationJobMsg(job, 1));
    }
    private void OnJobsSubtract(string job)
    {
        SendMessage(new AdjustStationJobMsg(job, -1));
    }
    // End Frontier
    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (state is not StationJobsConsoleState cast)
            return;

        _window?.UpdateState(cast);
    }
}
