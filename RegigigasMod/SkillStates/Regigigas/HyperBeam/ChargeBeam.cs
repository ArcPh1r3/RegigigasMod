using EntityStates;
using EntityStates.LemurianBruiserMonster;
using RoR2;
using UnityEngine;

namespace RegigigasMod.SkillStates.Regigigas.HyperBeam
{
    public class ChargeBeam : BaseState
    {
        public static float baseDuration = 4f;

        private float duration;
        private GameObject chargeInstance;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = ChargeBeam.baseDuration / this.attackSpeedStat;
            Animator modelAnimator = base.GetModelAnimator();
            Transform modelTransform = base.GetModelTransform();

            if (modelTransform)
            {
                ChildLocator component = modelTransform.GetComponent<ChildLocator>();
                if (component)
                {
                    Transform transform = component.FindChild("Head");
                    if (transform && ChargeMegaFireball.chargeEffectPrefab)
                    {
                        this.chargeInstance = UnityEngine.Object.Instantiate<GameObject>(ChargeMegaFireball.chargeEffectPrefab, transform.position, transform.rotation);
                        this.chargeInstance.transform.parent = transform;
                        ScaleParticleSystemDuration component2 = this.chargeInstance.GetComponent<ScaleParticleSystemDuration>();
                        if (component2)
                        {
                            component2.newDuration = this.duration;
                        }
                    }
                }
            }
            if (modelAnimator)
            {
                base.PlayCrossfade("Gesture, Additive", "ChargeMegaFireball", "ChargeMegaFireball.playbackRate", this.duration, 0.1f);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if (this.chargeInstance)
            {
                EntityState.Destroy(this.chargeInstance);
            }
        }

        public override void Update()
        {
            base.Update();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                FireMegaFireball nextState = new FireMegaFireball();
                this.outer.SetNextState(nextState);
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}