namespace Content.Shared.Chat;

public static class ChatChannelExtensions
{
    private static Color _subtleColor = Color.FromHex("#d3d3ff");
    private static Color _subtleOOCColor = Color.FromHex("#ff7782");

    public static Color TextColor(this ChatChannel channel)
    {
        return channel switch
        {
            ChatChannel.Server => Color.Orange,
            ChatChannel.Radio => Color.LimeGreen,
            ChatChannel.LOOC => Color.MediumTurquoise,
            ChatChannel.OOC => Color.LightSkyBlue,
            ChatChannel.Dead => Color.MediumPurple,
            ChatChannel.Admin => Color.Red,
            ChatChannel.AdminAlert => Color.Red,
            ChatChannel.AdminChat => Color.HotPink,
            ChatChannel.Whisper => Color.DarkGray,
            ChatChannel.Subtle => _subtleColor,
            ChatChannel.SubtleOOC => _subtleOOCColor,
            _ => Color.LightGray
        };
    }
}
