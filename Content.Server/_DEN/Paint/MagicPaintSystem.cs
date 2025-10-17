using Content.Server.Paint;
using Content.Shared._DEN.Paint;
using Content.Shared.Paint;

namespace Content.Server._DEN.Paint;

public sealed partial class MagicPaintSystem : SharedMagicPaintSystem
{
    [Dependency] private readonly PaintSystem _paint = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MagicPaintComponent, MagicPaintColorMessage>(OnMagicPaintColored);
    }

    private void OnMagicPaintColored(Entity<MagicPaintComponent> ent, ref MagicPaintColorMessage args)
    {
        if (!TryComp<PaintComponent>(ent.Owner, out var paint)
            || paint.Color == args.Color)
            return;

        _paint.SetColor((ent.Owner, paint), args.Color);
    }
}
