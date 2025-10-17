using Content.Shared.Paint;

namespace Content.Server.Paint;

public sealed partial class PaintSystem
{
    public void SetColor(Entity<PaintComponent> ent, Color color)
    {
        ent.Comp.Color = color;
        Dirty(ent);
    }
}
