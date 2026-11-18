using Verse;

namespace DraftDirections
{
    public class CompProperties_DraftDirections : CompProperties
    {
        public CompProperties_DraftDirections()
        {
            compClass = typeof(CompDraftDirections);
        }
    }

    public class CompDraftDirections : ThingComp
    {
        public CompProperties_DraftDirections Props => (CompProperties_DraftDirections)props;

        public bool enableDraftRot = false;
        public Rot4 draftRot = Rot4.South;

        public bool enableHeadRot = false;
        public Rot4 currentHeadRot = Rot4.South;

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref currentHeadRot, "currentHeadRot", Rot4.South);
            Scribe_Values.Look(ref enableHeadRot, "enableHeadRot", defaultValue: false);
            Scribe_Values.Look(ref draftRot, "draftRot", Rot4.South);
            Scribe_Values.Look(ref enableDraftRot, "currentDraftRot", defaultValue: false);
        }

        public void Refresh()
        {
            if (parent is Pawn pawn)
            {
                pawn.Drawer?.renderer?.SetAllGraphicsDirty();
            }
        }
    }
}
