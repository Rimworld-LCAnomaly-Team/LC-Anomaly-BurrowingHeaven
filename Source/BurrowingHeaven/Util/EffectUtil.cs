using BurrowingHeaven.Effect;
using RimWorld;
using Verse;

namespace BurrowingHeaven.Util
{
    public static class EffectUtil
    {
        /// <summary>
        /// 穿刺乐园死后操作
        /// </summary>
        /// <param name="pawn">单位</param>
        /// <param name="map">地图</param>
        public static void OnBurrowingHeavenDeath(Pawn pawn, Map map)
        {
            Find.LetterStack.ReceiveLetter("LetterLabelBurrowingHeavenKilled".Translate()
                , "LetterBurrowingHeavenKilled".Translate()
                , LetterDefOf.PositiveEvent
                , new LookTargets(pawn.PositionHeld, pawn.MapHeld));

            ((DyingBurrowingHeaven)GenSpawn
                .Spawn(Def.ThingDefOf.DyingBurrowingHeaven, pawn.Position, map))
                .InitWith(pawn);
        }
    }
}
