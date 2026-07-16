public static class CharacterSelection
{
    public static PlayableWing SelectedWing { get; private set; }
        = PlayableWing.None;

    public static void SelectWing(PlayableWing wing)
    {
        SelectedWing = wing;
    }

    public static bool HasSelectedWing()
    {
        return SelectedWing != PlayableWing.None;
    }
}