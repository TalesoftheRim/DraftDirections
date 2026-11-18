using HarmonyLib;
using RimWorld;
using Verse;

namespace DraftDirections
{
    [StaticConstructorOnStartup]
    public static class PatchesMainStatic
    {
        static PatchesMainStatic()
        {
            Harmony harmony = new(id: "rimworld.tqol.patchesmainstatic");

            harmony.Patch(original: AccessTools.Method(typeof(Pawn_RotationTracker), "UpdateRotation"),
                prefix: new HarmonyMethod(typeof(PatchesMainStatic), nameof(Pawn_RotationTracker_UpdateRotation_Prefix)));

            harmony.Patch(original: AccessTools.Method(typeof(PawnRenderNodeWorker_FlipWhenCrawling), "GetMaterial"),
                prefix: new HarmonyMethod(typeof(PatchesMainStatic), nameof(PawnRenderNodeWorker_FlipWhenCrawling_GetMaterial_Prefix)));

            harmony.Patch(original: AccessTools.Method(typeof(PawnRenderNodeWorker_FlipWhenCrawling), "LayerFor"),
                prefix: new HarmonyMethod(typeof(PatchesMainStatic), nameof(PawnRenderNodeWorker_FlipWhenCrawling_LayerFor_Prefix)));

            harmony.Patch(original: AccessTools.Method(typeof(PawnRenderNodeWorker), "CanDrawNow"),
                prefix: new HarmonyMethod(typeof(PatchesMainStatic), nameof(PawnRenderNodeWorker_CanDrawNow_Prefix)));

            harmony.Patch(original: AccessTools.Method(typeof(PawnRenderNodeWorker), "GetFinalizedMaterial"),
                prefix: new HarmonyMethod(typeof(PatchesMainStatic), nameof(PawnRenderNodeWorker_GetFinalizedMaterial_Prefix)));

            harmony.Patch(original: AccessTools.Method(typeof(PawnRenderNodeWorker), "AppendDrawRequests"),
                prefix: new HarmonyMethod(typeof(PatchesMainStatic), nameof(PawnRenderNodeWorker_AppendDrawRequests_Prefix)));

            harmony.Patch(original: AccessTools.Method(typeof(PawnRenderNode), "AppendRequests"),
                prefix: new HarmonyMethod(typeof(PatchesMainStatic), nameof(PawnRenderNode_AppendRequests_Prefix)));
        }
        
        private static bool Pawn_RotationTracker_UpdateRotation_Prefix(ref Pawn_RotationTracker __instance)
        {
            if (!SettingsMain.Activated) return true;

            Pawn pawn = (Pawn)AccessTools.Field(typeof(Pawn_RotationTracker), "pawn")?.GetValue(__instance);
            if (pawn == null) return true;
            
            if (ModsConfig.IsActive("SmashPhil.VehicleFramework") 
                && pawn.GetType().FullName == "Vehicles.VehiclePawn")
            {
                return true;
            }
            
            if (pawn.Destroyed || pawn.jobs.HandlingFacing) return false;
            if (pawn.stances.curStance is Stance_Busy { focusTarg.IsValid: true } stance_Busy)
            {
                if (stance_Busy.focusTarg.HasThing)
                {
                    __instance.Face(stance_Busy.focusTarg.Thing.DrawPos);
                }
                else
                {
                    __instance.FaceCell(stance_Busy.focusTarg.Cell);
                }
                return false;
            }
            
            if (pawn.pather.Moving)
            {
                if (pawn.pather.curPath is { NodesLeftCount: >= 1 })
                {
                    PatchesUtil.FaceAdjacentCell(pawn.pather.nextCell, pawn);
                }
                return false;
            }
            
            if (pawn.jobs.curJob != null)
            {
                LocalTargetInfo target = pawn.CurJob.GetTarget(pawn.jobs.curDriver.rotateToFace);
                __instance.FaceTarget(target);
            }
            
            if (pawn is { Drafted: true })
            {
                var comp = pawn.GetComp<CompDraftDirections>();
                pawn.Rotation = comp is { enableDraftRot: true } ? comp.draftRot : Rot4.South;
            }

            return false;
        }

        public static void PawnRenderNodeWorker_FlipWhenCrawling_GetMaterial_Prefix(ref PawnDrawParms parms)
        {
            if (!SettingsMain.Activated) return;

            var pawn = parms.pawn;
            if (pawn == null || !pawn.Drafted) return;

            var comp = pawn.GetComp<CompDraftDirections>();
            if (comp == null || !comp.enableHeadRot) return;

            parms.facing = comp.currentHeadRot;
        }

        public static void PawnRenderNodeWorker_FlipWhenCrawling_LayerFor_Prefix(ref PawnDrawParms parms)
        {
            if (!SettingsMain.Activated) return;

            var pawn = parms.pawn;
            if (pawn == null || !pawn.Drafted) return;

            var comp = pawn.GetComp<CompDraftDirections>();
            if (comp == null || !comp.enableHeadRot) return;

            parms.facing = comp.currentHeadRot;
        }

        public static void PawnRenderNodeWorker_CanDrawNow_Prefix(PawnRenderNodeWorker __instance, ref PawnDrawParms parms)
        {
            if (!SettingsMain.Activated) return;

            if (__instance is not PawnRenderNodeWorker_FlipWhenCrawling) return;
            var pawn = parms.pawn;
            if (pawn == null || !pawn.Drafted) return;

            var comp = pawn.GetComp<CompDraftDirections>();
            if (comp == null || !comp.enableHeadRot) return;

            parms.facing = comp.currentHeadRot;
        }

        public static void PawnRenderNodeWorker_GetFinalizedMaterial_Prefix(PawnRenderNodeWorker __instance, ref PawnDrawParms parms)
        {
            if (!SettingsMain.Activated) return;

            if (__instance is not PawnRenderNodeWorker_FlipWhenCrawling) return;
            var pawn = parms.pawn;
            if (pawn == null || !pawn.Drafted) return;

            var comp = pawn.GetComp<CompDraftDirections>();
            if (comp == null || !comp.enableHeadRot) return;

            parms.facing = comp.currentHeadRot;
        }

        public static void PawnRenderNodeWorker_AppendDrawRequests_Prefix(PawnRenderNodeWorker __instance, ref PawnDrawParms parms)
        {
            if (!SettingsMain.Activated) return;

            if (__instance is not PawnRenderNodeWorker_FlipWhenCrawling) return;
            var pawn = parms.pawn;
            if (pawn == null || !pawn.Drafted) return;

            var comp = pawn.GetComp<CompDraftDirections>();
            if (comp == null || !comp.enableHeadRot) return;

            parms.facing = comp.currentHeadRot;
        }

        public static void PawnRenderNode_AppendRequests_Prefix(PawnRenderNode __instance, ref PawnDrawParms parms)
        {
            if (!SettingsMain.Activated) return;

            if (__instance.Worker is not PawnRenderNodeWorker_FlipWhenCrawling) return;
            var pawn = parms.pawn;
            if (pawn == null || !pawn.Drafted) return;

            var comp = pawn.GetComp<CompDraftDirections>();
            if (comp == null || !comp.enableHeadRot) return;

            parms.facing = comp.currentHeadRot;
        }
    }
}