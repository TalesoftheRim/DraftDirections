using UnityEngine;
using Verse;

namespace DraftDirections
{
    [StaticConstructorOnStartup]
    public static class GraphicsUtil
    {
        public static Texture2D DraftRotationIcon;
        public static Texture2D HeadRotationIcon;
        public static Texture2D HeadRotEnableIcon;
        public static Texture2D ToggleDisplayIcon;

        static GraphicsUtil()
        {
            DraftRotationIcon = ContentFinder<Texture2D>.Get("Icons/DraftDirections/Rotation");
            HeadRotationIcon = ContentFinder<Texture2D>.Get("Icons/DraftDirections/HeadTurnerDirection");
            HeadRotEnableIcon = ContentFinder<Texture2D>.Get("Icons/DraftDirections/HeadTurnerToggle");
            ToggleDisplayIcon = ContentFinder<Texture2D>.Get("Icons/DraftDirections/Rotation");
        }
    }
}