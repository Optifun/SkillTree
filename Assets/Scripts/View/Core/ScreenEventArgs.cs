namespace SkillTree.UI.Core
{
    public class ScreenEventArgs
    {
        public readonly BaseScreen Screen;

        public ScreenEventArgs(BaseScreen screen)
        {
            Screen = screen;
        }
    }
}