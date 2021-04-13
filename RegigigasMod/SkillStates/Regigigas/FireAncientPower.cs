using EntityStates;
using RoR2;
using UnityEngine;
using RoR2.Projectile;

namespace RegigigasMod.SkillStates.Regigigas
{
    public class FireAncientPower : BaseRegiSkillState
    {
        public int rockCount;

        public static float baseDuration = 0.6f;
        public static float baseTimeBetweenRocks = 0.15f;

        public static float damageCoefficient = 4f;
        public static float throwForce = 100f;
        public static float projectileForce = 5f;

        private float throwStopwatch;
        private float timeBetweenRocks;
        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = FireAncientPower.baseDuration / this.attackSpeedStat;
            this.timeBetweenRocks = FireAncientPower.baseTimeBetweenRocks / this.attackSpeedStat;
            this.throwStopwatch = this.timeBetweenRocks;

            base.PlayAnimation("Gesture, Override", "FireAncientPower", "AncientPower.playbackRate", FireAncientPower.baseDuration);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.StartAimMode(0.5f);
            this.throwStopwatch -= Time.fixedDeltaTime;

            if (this.throwStopwatch <= 0f)
            {
                this.ThrowRock();
            }

            if (base.isAuthority && base.fixedAge >= this.duration && this.rockCount <= 0)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            base.cameraTargetParams.cameraParams = Modules.CameraParams.defaultCameraParams;
            base.characterBody.hideCrosshair = true;
        }

        private void ThrowRock()
        {
            if (this.rockCount <= 0) return;

            this.rockCount--;
            this.throwStopwatch = this.timeBetweenRocks;

            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();

                ProjectileManager.instance.FireProjectile(Resources.Load<GameObject>("Prefabs/Projectiles/GrandparentMiniBoulder"), 
                    aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), 
                    base.gameObject, 
                    this.damageStat * FireAncientPower.damageCoefficient, 
                    FireAncientPower.projectileForce, 
                    Util.CheckRoll(this.critStat, base.characterBody.master), 
                    DamageColorIndex.Default, 
                    null, 
                    FireAncientPower.throwForce);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}