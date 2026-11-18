using System.Collections.Generic;
using TalesUIFramework;
using UnityEngine;
using Verse;

namespace DraftDirections
{
    [StaticConstructorOnStartup]
    public sealed class DraftDirectionsControlProvider : IPawnControlProvider
    {
        public string ProviderId => "TalesoftheRim.DraftDirections";
        public string DisplayName => "Draft Directions";
        public int Order => 200;

        static DraftDirectionsControlProvider()
        {
            TalesUIRegistry.RegisterProvider(new DraftDirectionsControlProvider());
        }

        public void CollectControls(Pawn pawn, List<TalesControlRow> rows)
        {
            if (pawn == null || !pawn.Drafted || !SettingsMain.Activated || !SettingsMain.ShowButtons)
            {
                return;
            }

            CompDraftDirections comp = pawn.TryGetComp<CompDraftDirections>();
            if (comp == null)
            {
                return;
            }

            rows.Add(CreateBodyDirectionRow(comp));

            if (!pawn.IsAnimal && !pawn.RaceProps.IsMechanoid)
            {
                rows.Add(CreateHeadDirectionRow(comp));
            }
        }

        private static TalesControlRow CreateBodyDirectionRow(CompDraftDirections comp)
        {
            TalesControlOption[] options = BuildDirectionOptions(GraphicsUtil.DraftRotationIcon);

            return new TalesControlRow(
                id: "DraftDirections.BodyDirections",
                order: 200f,
                title: "DraftDirections.UI.BodyDirectionsTitle".Translate(),
                toggleIcon: GraphicsUtil.DraftRotationIcon,
                toggleTooltip: "DraftDirections.UI.ToggleBodyDirectionDesc".Translate(),
                isEnabled: () => comp.enableDraftRot,
                setEnabled: value =>
                {
                    if (comp.enableDraftRot != value)
                    {
                        comp.enableDraftRot = value;
                        comp.Refresh();
                    }
                },
                options: options,
                getSelectedOptionId: () => RotToId(comp.draftRot),
                selectOption: id =>
                {
                    if (TryParseRot(id, out Rot4 rot) && comp.draftRot != rot)
                    {
                        comp.draftRot = rot;
                        comp.Refresh();
                    }
                },
                selectionTooltip: "DraftDirections.ChangeDraftDirectionDesc".Translate(),
                selectionDisabledTooltip: "DraftDirections.UI.EnableBodyDirectionsFirst".Translate(),
                fallbackToggleLabel: "DraftDirections.ToggleDraftRotationLabel".Translate(comp.enableDraftRot.ToString()),
                fallbackSelectionLabel: () => "DraftDirections.ChangeDraftDirectionLabel".Translate(comp.draftRot.ToStringHuman()));
        }

        private static TalesControlRow CreateHeadDirectionRow(CompDraftDirections comp)
        {
            TalesControlOption[] options = BuildDirectionOptions(GraphicsUtil.HeadRotationIcon);

            return new TalesControlRow(
                id: "DraftDirections.HeadDirections",
                order: 210f,
                title: "DraftDirections.UI.HeadDirectionsTitle".Translate(),
                toggleIcon: GraphicsUtil.HeadRotEnableIcon,
                toggleTooltip: "DraftDirections.UI.ToggleHeadDirectionDesc".Translate(),
                isEnabled: () => comp.enableHeadRot,
                setEnabled: value =>
                {
                    if (comp.enableHeadRot != value)
                    {
                        comp.enableHeadRot = value;
                        comp.Refresh();
                    }
                },
                options: options,
                getSelectedOptionId: () => RotToId(comp.currentHeadRot),
                selectOption: id =>
                {
                    if (TryParseRot(id, out Rot4 rot) && comp.currentHeadRot != rot)
                    {
                        comp.currentHeadRot = rot;
                        comp.Refresh();
                    }
                },
                selectionTooltip: "DraftDirections.ChangeHeadDirectionDesc".Translate(),
                selectionDisabledTooltip: "DraftDirections.UI.EnableHeadDirectionsFirst".Translate(),
                fallbackToggleLabel: "DraftDirections.ToggleHeadRotationLabel".Translate(),
                fallbackSelectionLabel: () => "DraftDirections.ChangeHeadDirectionLabel".Translate(comp.currentHeadRot.ToStringHuman()));
        }

        private static TalesControlOption[] BuildDirectionOptions(Texture2D icon)
        {
            return
            [
                new("North", Rot4.North.ToStringHuman(), icon, "N"),
                new("East", Rot4.East.ToStringHuman(), icon, "E"),
                new("South", Rot4.South.ToStringHuman(), icon, "S"),
                new("West", Rot4.West.ToStringHuman(), icon, "W")
            ];
        }

        private static string RotToId(Rot4 rot)
        {
            return rot.AsInt switch
            {
                0 => "North",
                1 => "East",
                2 => "South",
                3 => "West",
                _ => "South"
            };
        }

        private static bool TryParseRot(string id, out Rot4 rot)
        {
            switch (id)
            {
                case "North":
                    rot = Rot4.North;
                    return true;
                case "East":
                    rot = Rot4.East;
                    return true;
                case "South":
                    rot = Rot4.South;
                    return true;
                case "West":
                    rot = Rot4.West;
                    return true;
                default:
                    rot = Rot4.South;
                    return false;
            }
        }
    }
}
