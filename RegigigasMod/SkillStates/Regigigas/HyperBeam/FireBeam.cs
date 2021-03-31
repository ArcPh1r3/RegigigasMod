using EntityStates;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RegigigasMod.SkillStates.Regigigas.HyperBeam
{
    public class ChargeBeam : BaseState
    {
        public static float baseDuration = 1f;

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
                    Transform transform = component.FindChild("MuzzleMouth");
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

        // Token: 0x060039FC RID: 14844 RVA: 0x000EDBFC File Offset: 0x000EBDFC
        public override void OnExit()
        {
            base.OnExit();
            if (this.chargeInstance)
            {
                EntityState.Destroy(this.chargeInstance);
            }
        }

        // Token: 0x060039FD RID: 14845 RVA: 0x000D44F8 File Offset: 0x000D26F8
        public override void Update()
        {
            base.Update();
        }

        // Token: 0x060039FE RID: 14846 RVA: 0x000EDC1C File Offset: 0x000EBE1C
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

        // Token: 0x060039FF RID: 14847 RVA: 0x0000CFF7 File Offset: 0x0000B1F7
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}