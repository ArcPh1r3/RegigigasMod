using RegigigasMod.SkillStates.BaseStates;
using RoR2;
using UnityEngine;

namespace RegigigasMod.SkillStates.Regigigas
{
    public class DrainPunch : PunchCombo
    {
        internal new static float damageCoefficientOverride = 2.8f;

        public override void OnEnter()
        {
            base.OnEnter();

            this.attack.damage = DrainPunch.damageCoefficientOverride * this.damageStat;
            this.attack.damageType = DamageType.BlightOnHit;

            if (this.swingIndex == 1) EffectManager.SimpleMuzzleFlash(Modules.Assets.drainPunchChargeEffect, base.gameObject, "HandR", false);
            else EffectManager.SimpleMuzzleFlash(Modules.Assets.drainPunchChargeEffect, base.gameObject, "HandL", false);
        }

        protected override void SetNextState()
        {
            int index = this.swingIndex + 1;
            if (index == 3) index = 1;

            this.outer.SetNextState(new DrainPunch
            {
                swingIndex = index
            });
        }
    }
}