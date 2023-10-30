using UnityEngine;
using RegigigasMod.Modules.Components;
using RoR2;
using RoR2.Orbs;

namespace RegigigasMod.Modules.Misc
{
    public class SlowStartOrb : Orb
    {
        private SlowStartController slowStartController;

        public override void Begin()
        {
            base.duration = base.distanceToTarget / Random.Range(15f, 30f);

            EffectData effectData = new EffectData
            {
                origin = this.origin,
                genericFloat = base.duration
            };

            effectData.SetHurtBoxReference(this.target);

            EffectManager.SpawnEffect(Modules.Enemies.Regigigas.slowStartOrb, effectData, true);

            HurtBox hurtBox = this.target.GetComponent<HurtBox>();
            if (hurtBox)
            {
                this.slowStartController = hurtBox.healthComponent.GetComponent<SlowStartController>();
            }
        }

        public override void OnArrival()
        {
            if (this.slowStartController)
            {
                this.slowStartController.GrantKill();
            }
        }
    }
}