using Verse;

namespace DraftDirections
{
    public static class PatchesUtil
    {
        public static void FaceAdjacentCell(IntVec3 c, Pawn pawn)
        {
            if (c == pawn.Position) return;
            IntVec3 delta = c - pawn.Position;
            
            pawn.Rotation = (delta.x, delta.z) switch
            {
                var (x, _) when x > 0 => Rot4.East,
                var (x, _) when x < 0 => Rot4.West,
                var (_, z) when z > 0 => Rot4.North,
                _ => Rot4.South
            };
        }
    }
}