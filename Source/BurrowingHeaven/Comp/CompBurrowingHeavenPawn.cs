using LCAnomalyLibrary.Comp;
using LCAnomalyLibrary.Util;
using RimWorld;
using Verse;

namespace BurrowingHeaven.Comp
{
    public class CompBurrowingHeaven : LC_CompEntity
    {
        #region 变量

        public new CompProperties_BurrowingHeaven Props => (CompProperties_BurrowingHeaven)props;

        #endregion 变量

        #region 触发事件

        public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
        {
            Util.EffectUtil.OnBurrowingHeavenDeath((Pawn)parent, prevMap);
        }

        public override void Notify_Escaped()
        {
            base.Notify_Escaped();
        }

        /// <summary>
        /// 绑到收容平台上后的操作
        /// </summary>
        public override void Notify_Holded()
        {
            CheckIsDiscovered();
        }

        public override void Notify_Studying(Pawn studier)
        {
            Log.Warning("穿刺乐园正在被工作，请注意");
        }

        #endregion 触发事件

        #region 研究与图鉴

        protected override LC_StudyResult CheckFinalStudyQuality(Pawn studier, EAnomalyWorkType workType)
        {
            //每级智力提供2%成功率，20级智力提供40%成功率
            float successRate_Intellectual = studier.skills.GetSkill(SkillDefOf.Intellectual).Level * 0.02f;
            //叠加基础成功率，此处是20%，叠加完应是60%
            float finalSuccessRate = successRate_Intellectual + Props.studySucessRateBase;

            switch (workType)
            {
                case EAnomalyWorkType.Instinct:
                    finalSuccessRate -= 0.2f;
                    break;

                case EAnomalyWorkType.Insight:
                    finalSuccessRate += 0.1f;
                    break;

                case EAnomalyWorkType.Attachment:
                    finalSuccessRate += 0.2f;
                    break;

                case EAnomalyWorkType.Repression:
                    finalSuccessRate += 0.1f;
                    break;
            }

            //成功率不能超过80%
            if (finalSuccessRate >= 1f)
                finalSuccessRate = 0.8f;

            return Rand.Chance(finalSuccessRate) ? LC_StudyResult.Good : LC_StudyResult.Normal;
        }

        public override bool CheckStudierSkillRequire(Pawn studier)
        {
            if (studier.skills.GetSkill(SkillDefOf.Intellectual).Level < 10)
            {
                //Log.Message($"工作：{studier.Name}的技能{SkillDefOf.Intellectual.label.Translate()}等级不足10，工作固定无法成功");
                return false;
            }

            return true;
        }

        protected override void StudyEvent_Good(Pawn studier)
        {
            //穿刺乐园研究为优 +2点计数器
            QliphothCountCurrent += 2;
            AccessoryableComp?.CheckGiveAccessory(studier);
        }

        protected override void StudyEvent_Normal(Pawn studier)
        {
            //穿刺乐园研究为良 +1点计数器
            QliphothCountCurrent++;
        }

        protected override void StudyEvent_Bad(Pawn studier)
        {
            //穿刺乐园研究为差不减计数器
        }

        /// <summary>
        /// 检查是否已在图鉴中被解锁
        /// </summary>
        private void CheckIsDiscovered()
        {
            if (AnomalyUtility.ShouldNotifyCodex((Pawn)parent, EntityDiscoveryType.BecameVisible, out var entries))
            {
                Find.EntityCodex.SetDiscovered(entries, Def.PawnKindDefOf.BurrowingHeaven.race, (Pawn)parent);
            }
        }

        #endregion 研究与图鉴
    }
}
