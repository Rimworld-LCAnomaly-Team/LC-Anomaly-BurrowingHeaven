using BurrowingHeaven.Comp;
using LCAnomalyLibrary.Comp;
using LCAnomalyLibrary.Util;
using RimWorld;

namespace BurrowingHeaven.Things
{
    public class LC_BurrowingHeavenPawn : LC_EntityBasePawn
    {
        public CompBurrowingHeaven SelfEntityComp => (CompBurrowingHeaven)EntityComp;

        public bool ShouldCheckInsight = false;
        public bool IsOutOfSight => isOutOfSight;
        protected bool isOutOfSight = false;

        public LC_BurrowingHeavenPawn()
        {
        }

        public override void Tick()
        {
            //收容状态下丢下就出逃
            if (CarriedBy == null && !SelfEntityComp.Escaped)
                SelfEntityComp.HoldingPlaformTargetComp.Escape(true);

            //检查是否在视野内
            if (ShouldCheckInsight)
                isOutOfSight = SightUtil.IsOutsideView(this, Faction.OfPlayer);

            base.Tick();
        }
    }
}
