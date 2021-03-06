using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using RegigigasMod.Modules.Components;
using System.Linq;

namespace RegigigasMod.SkillStates.Regigigas
{
    public class GrabSuccess : BaseRegiSkillState
    {
        public HurtBox target;
        public RegigigasGrabController grabController;

        public static float baseDuration = 6f;
        public static string grabTransformString = "HandR";
        public static float throwForce = 120f;
        public static float damagePercentage = 0.55f;

        private bool hasSqueezed;
        private float grabSqueezeTime;
        private float grabThrowTime;
        private Transform grabTransform;
        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = GrabSuccess.baseDuration / this.attackSpeedStat;
            this.grabThrowTime = 0.7f * this.duration;
            this.grabSqueezeTime = 0.3f * this.duration;
            this.hasSqueezed = false;
            this.grabTransform = base.FindModelChild(GrabSuccess.grabTransformString);

            base.PlayAnimation("FullBody, Override", "GrabSuccess", "Grab.playbackRate", this.duration);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.grabSqueezeTime)
            {
                this.SqueezeTarget();

                base.StartAimMode(0.5f, false);
            }

            if (NetworkServer.active && base.fixedAge >= this.grabThrowTime)
            {
                this.Throw();
            }

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        private void SqueezeTarget()
        {
            if (this.hasSqueezed) return;

            this.hasSqueezed = true;

            base.flashController.Flash();

            if (base.isAuthority && this.target)
            {
                DamageInfo info = new DamageInfo
                {
                    attacker = base.gameObject,
                    crit = false,
                    damage = GrabSuccess.damagePercentage * target.healthComponent.fullCombinedHealth,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.BypassArmor,
                    force = Vector3.zero,
                    inflictor = base.gameObject,
                    position = base.transform.position,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = 1f,
                };

                target.healthComponent.TakeDamage(info);
            }
        }

        private void Throw()
        {
            if (this.grabController)
            {
                this.grabController.Throw(base.GetAimRay().direction * GrabSuccess.throwForce);
                this.grabController = null;
                this.target = null;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}