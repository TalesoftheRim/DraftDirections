using System.Linq;
using Verse;
using System.Collections.Generic;

namespace DraftDirections
{
    [StaticConstructorOnStartup]
    public static class DraftDirectionsTracker
    {
        static DraftDirectionsTracker()
        {
            foreach (var thingDef in DefDatabase<ThingDef>.AllDefs.Where(x => (x.race?.Humanlike == true || x.race?.Animal == true || x.race?.IsMechanoid == true || x.race?.IsAnomalyEntity == true) && x.IsCorpse is false))
            {
                if (thingDef.comps == null)
                {
                    thingDef.comps = new List<CompProperties>();
                }
                thingDef.comps.Add(new CompProperties_DraftDirections());
            }
        }
    }
}
