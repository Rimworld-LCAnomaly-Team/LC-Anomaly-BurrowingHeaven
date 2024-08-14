using BurrowingHeaven.Comp;
using LCAnomalyLibrary.Comp;

namespace BurrowingHeaven.Things
{
    public class LC_BurrowingHeavenPawn : LC_EntityBasePawn
    {
        public CompBurrowingHeaven SelfEntityComp => (CompBurrowingHeaven)EntityComp;

        public LC_BurrowingHeavenPawn()
        {
        }

        public override void Tick()
        {
            //收容状态下丢下就出逃
            if (CarriedBy == null && !SelfEntityComp.Escaped)
            {
                SelfEntityComp.HoldingPlaformTargetComp.Escape(true);
            }

            base.Tick();
        }
    }
}
