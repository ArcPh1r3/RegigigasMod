using RegigigasMod.SkillStates.BaseStates;
using RoR2;
using UnityEngine;

namespace RegigigasMod.SkillStates.Regigigas
{
    public class PunchCombo : BaseMeleeAttack
    {
        internal static float damageCoefficientOverride = 4.2f;
        private bool sprintBuffered;

        public override void OnEnter()
        {
            this.hitboxName = "Punch";

            this.damageType = DamageType.Stun1s;
            this.damageCoefficient = PunchCombo.damageCoefficientOverride;
            this.procCoefficient = 1f;
            this.pushForce = 3500f;
            this.bonusForce = Vector3.zero;
            this.attackStartTime = 0.37f;
            this.attackEndTime = 0.5f;
            this.baseEarlyExitTime = 0.58f;
            this.hitStopDuration = 0.18f;
            this.attackRecoil = 1.5f;
            this.hitHopVelocity = 8f;

            this.swingSoundString = "RegigigasPunchSwing";
            this.hitSoundString = "";
            this.muzzleString = swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight";
            //this.swingEffectPrefab = Modules.Assets.punchSwingEffect;
            this.hitEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/PodGroundImpact");

            this.impactSound = Modules.Assets.punchSoundDef.index;

            base.OnEnter();

            PrimarySkillShurikenBehavior shurikenComponent = this.GetComponent<PrimarySkillShurikenBehavior>();
            if (shurikenComponent) shurikenComponent.OnSkillActivated(this.skillLocator.primary);
        }

        protected override void PlayAttackAnimation()
        {
            base.PlayAnimation("Gesture, Override", "Punch" + (1 + swingIndex), "Punch.playbackRate", this.duration);
        }

        protected override void PlaySwingEffect()
        {
            //base.PlaySwingEffect();
        }

        protected override void SetNextState()
        {
            int index = this.swingIndex + 1;
            if (index == 3) index = 1;

            this.outer.SetNextState(new PunchCombo
            {
                swingIndex = index
            });
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.characterBody.isSprinting = false;

            if (this.inputBank && this.inputBank.sprint.down) this.sprintBuffered = true;
        }

        public override void OnExit()
        {
            base.OnExit();
            if ((this.inputBank && this.inputBank.sprint.down) || this.sprintBuffered) this.characterBody.isSprinting = true;
        }
    }
}