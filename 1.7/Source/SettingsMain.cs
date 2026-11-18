using Verse;

namespace DraftDirections
{
    public class SettingsMain : ModSettings
    {
        private static SettingsMain _instance;

        public SettingsMain()
        {
            _instance = this;
        }

        public bool showButtons = true;

        public bool activated = true;

        public static bool Activated => _instance == null || _instance.activated;

        public static bool ShowButtons
        {
            get => _instance == null || _instance.showButtons;
            set
            {
                if (_instance != null && value != _instance.showButtons)
                {
                    _instance.showButtons = value;
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref showButtons, "showButtons", defaultValue: true);
            Scribe_Values.Look(ref activated, "activated", defaultValue: true);
        }
    }
}
