using Content.Shared.Paint;
using Robust.Shared.Serialization;

namespace Content.Shared._DEN.Paint;

public abstract partial class SharedMagicPaintSystem : EntitySystem
{
    [Dependency] private readonly SharedUserInterfaceSystem _ui = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MagicPaintComponent, BoundUIOpenedEvent>(OnBoundUIOpened);
    }

    private void OnBoundUIOpened(Entity<MagicPaintComponent> ent, ref BoundUIOpenedEvent args)
    {
        if (args.UiKey is not MagicPaintUiKey key
            || !TryComp<PaintComponent>(ent.Owner, out var paint))
            return;

        var state = new MagicPaintInterfaceState(paint.Color);
        _ui.SetUiState(ent.Owner, key, state);
    }
}

[Serializable, NetSerializable]
public enum MagicPaintUiKey : byte
{
    Key,
}

[Serializable, NetSerializable]
public sealed class MagicPaintColorMessage(Color color) : BoundUserInterfaceMessage
{
    public readonly Color Color = color;
}

[Serializable, NetSerializable]
public sealed class MagicPaintInterfaceState(Color color) : BoundUserInterfaceState
{
    public readonly Color Color = color;
}
