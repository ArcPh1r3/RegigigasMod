using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using RegigigasMod.Modules.Components;
using System.Linq;
using UnityEngine.AddressableAssets;

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
        private GameObject crushEffectPrefab;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = GrabSuccess.baseDuration / this.attackSpeedStat;
            this.grabThrowTime = 0.7f * this.duration;
            this.grabSqueezeTime = 0.3f * this.duration;
            this.hasSqueezed = false;
            this.grabTransform = base.FindModelChild(GrabSuccess.grabTransformString);

            this.crushEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Loader/OmniImpactVFXLoader.prefab").WaitForCompletion();

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

            if (base.fixedAge >= this.grabThrowTime)
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
                    attacker = this.gameObject,
                    crit = false,
                    damage = GrabSuccess.damagePercentage * this.target.healthComponent.fullCombinedHealth,
                    damageColorIndex = DamageColorIndex.WeakPoint,
                    damageType = DamageType.BypassArmor,
                    force = Vector3.zero,
                    inflictor = this.gameObject,
                    position = this.target.transform.position,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = 1f,
                };

                target.healthComponent.TakeDamage(info);

                EffectManager.SpawnEffect(this.crushEffectPrefab, new EffectData
                {
                    origin = this.target.transform.position,
                    scale = 1f
                }, true);
            }

            // why did this never have a sound?
            if (this.target) Util.PlaySound("RegigigasPunchImpact", this.gameObject);
        }

        private void Throw()
        {
            if (this.grabController)
            {
                if (NetworkServer.active) this.grabController.Throw(base.GetAimRay().direction * GrabSuccess.throwForce);
                Destroy(this.grabController);
                this.grabController = null;
                this.target = null;
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            // release if you kill during the grab
            if (this.grabController)
            {
                if (NetworkServer.active) this.grabController.Release();
                Destroy(this.grabController);
                this.grabController = null;
                this.target = null;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}