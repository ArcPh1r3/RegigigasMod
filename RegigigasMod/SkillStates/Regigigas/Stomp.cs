using EntityStates;
using EntityStates.BrotherMonster;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace RegigigasMod.SkillStates.Regigigas
{
    public class Stomp : BaseRegiSkillState
    {
        public static float baseDuration = 4f;
        public static float damageCoefficient = 6f;

        private float stompTime;
        private bool hasStomped;
        private float duration;
        private float flashTime;
        private bool hasFlashed;
        private Animator modelAnimator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = Stomp.baseDuration / this.attackSpeedStat;
            this.stompTime = 0.55f * this.duration;
            this.flashTime = 0.4f * this.duration;
            this.hasStomped = false;
            this.hasFlashed = false;
            this.modelAnimator = base.GetModelAnimator();

            base.PlayAnimation("FullBody, Override", "Stomp", "Stomp.playbackRate", this.duration);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.flashTime && !this.hasFlashed)
            {
                this.hasFlashed = true;
                base.flashController.Flash();
            }

            if (base.fixedAge >= this.stompTime && !this.hasStomped)
            {
                this.PerformStomp();
            }

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
            }
        }

        private void PerformStomp()
        {
            this.hasStomped = true;

            Util.PlaySound("RegigigasStomp", base.gameObject);
            Util.PlaySound("Play_parent_attack1_slam", base.gameObject);

            EffectManager.SimpleMuzzleFlash(Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/PodGroundImpact"), base.gameObject, "FootR", false);

            if (base.isAuthority)
            {
                float baseProjectileCount = (float)(1.5f * UltChannelState.waveProjectileCount);
                if (Modules.Config.nerfedEarthPower) baseProjectileCount = 5f;

                float num = 360f / baseProjectileCount;
                Vector3 normalized = Vector3.ProjectOnPlane(UnityEngine.Random.onUnitSphere, Vector3.up).normalized;
                Vector3 footPosition = this.FindModelChild("FootR").position;

                GameObject prefab = Modules.Projectiles.earthPowerWave;

                for (int i = 0; i < baseProjectileCount; i++)
                {
                    Vector3 forward = Quaternion.AngleAxis(num * (float)i, Vector3.up) * normalized;
                    ProjectileManager.instance.FireProjectile(prefab, footPosition, Util.QuaternionSafeLookRotation(forward), base.gameObject, base.characterBody.damage * Stomp.damageCoefficient, UltChannelState.waveProjectileForce, base.RollCrit(), DamageColorIndex.Default, null, -1f);
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}