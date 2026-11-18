using UnityEngine;
using Verse;

namespace DraftDirections
{
    public class DraftDirectionsMod : Mod
    {
        private readonly SettingsMain _settingsMain;
        private float _halfWidth;
        private Vector2 _leftScrollPos = Vector2.zero;

        public override string SettingsCategory() => "DraftDirections.SettingCategory".Translate();

        public DraftDirectionsMod(ModContentPack pack) : base(pack)
        {
            _settingsMain = GetSettings<SettingsMain>();

            Log.Message("[Draft Directions] Assembly loaded, ready for a draft!");
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            _halfWidth = (inRect.width - 30f) / 2;
            LeftSideScrollViewHandler(new Rect(inRect.x, inRect.y, _halfWidth, inRect.height));
        }

        private void LeftSideScrollViewHandler(Rect inRect)
        {
            Listing_Standard listLeft = new();
            Rect viewRectLeft = new(inRect.x, inRect.y, inRect.width, inRect.height);
            Rect vROffsetLeft = new(0, 0, inRect.width - 20, inRect.height);

            Widgets.BeginScrollView(viewRectLeft, ref _leftScrollPos, vROffsetLeft);
            listLeft.Begin(vROffsetLeft);
            listLeft.Gap();

            listLeft.CheckboxLabeled(
                "DraftDirections.Settings.ShowButtons".Translate(),
                ref _settingsMain.activated);

            listLeft.End();
            Widgets.EndScrollView();
        }
    }
}