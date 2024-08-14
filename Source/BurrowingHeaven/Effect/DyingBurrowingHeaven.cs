using BurrowingHeaven.Comp;
using LCAnomalyLibrary.Misc;
using RimWorld;
using Verse;

namespace BurrowingHeaven.Effect
{
    /// <summary>
    /// 蕾蒂希娅死亡特效对象
    /// </summary>
    public class DyingBurrowingHeaven : LC_FX_Standard
    {
        public override void InitWith(Pawn targetPawn)
        {
            var comp = targetPawn.TryGetComp<CompBurrowingHeaven>();

            //传递生物特征，播放effecter特效，记录动画播放完的时间
            bioSignature = comp.biosignature;

            Effecter effecter = EffecterDefOf.MeatExplosionExtraLarge.SpawnMaintained(base.Position, base.Map);
            completeTick = base.TickSpawned + effecter.ticksLeft + 60;

            hasInited = true;
        }

        public override void Complete()
        {
            if (!hasInited)
            {
                //Log.Warning($"特效：在未初始化时尝试生成特效对象{Def.ThingDefOf.DyingMeatLantern.label.Translate()}，对象即将被销毁以避免错误。");
                Destroy();
                return;
            }
            //生成扭曲血肉脏污
            if (FilthMaker.TryMakeFilth(base.PositionHeld, base.Map, RimWorld.ThingDefOf.Filth_TwistedFlesh))
            {
                //清除1格以内的植物
                foreach (IntVec3 item in CellRect.CenteredOn(base.PositionHeld, 1))
                {
                    Plant plant = item.GetPlant(base.Map);
                    if (plant != null && plant.MaxHitPoints < 100)
                    {
                        plant.Destroy();
                    }
                }
            }

            //生成蛋，销毁自己
            Thing thing = ThingMaker.MakeThing(Def.ThingDefOf.BurrowingHeavenEgg);
            thing.TryGetComp<CompBiosignatureOwner>().biosignature = bioSignature;
            GenSpawn.Spawn(thing, base.PositionHeld, base.MapHeld);
            Destroy();
        }
    }
}
