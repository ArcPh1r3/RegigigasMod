using EntityStates;
using RoR2;
using UnityEngine;

namespace RegigigasMod.SkillStates.Regigigas
{
    public class GigaImpactOld : BaseRegiSkillState
    {
        public static float baseDuration = 6f;

        public static float impactAttackRadius = 12f;
        public static float impactAttackForce = 500f;
        public static float impactAttackBonusForce = -1000f;
        public static float impactAttackDamageCoefficient = 32f;

        public static float blastAttackRadius = 32f;
        public static float blastAttackForce = 2000f;
        public static float blastAttackBonusForce = 1000f;
        public static float blastAttackDamageCoefficient = 12f;

        private float impactTime;
        private float duration;
        private bool hasFired;
        private bool hasFiredShockwave;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = GigaImpactOld.baseDuration / this.attackSpeedStat;
            this.impactTime = this.duration * 0.55f;
            this.hasFired = false;

            base.PlayAnimation("FullBody, Override", "GigaImpact", "GigaImpact.playbackRate", this.duration);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.impactTime)
            {
                this.Fire();
            }

            if (base.fixedAge >= 0.75f * this.duration)
            {
                this.FireShockwave();
            }

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void Fire()
        {
            if (this.hasFired) return;
            this.hasFired = true;

            Util.PlaySound(EntityStates.ParentMonster.GroundSlam.attackSoundString, base.gameObject);
            if (EntityStates.BeetleGuardMonster.GroundSlam.slamEffectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(EntityStates.BeetleGuardMonster.GroundSlam.slamEffectPrefab, base.gameObject, "HandR", false);
            }

            this.flashController.Flash();

            if (base.isAuthority)
            {
                BlastAttack blastAttack = new BlastAttack();
                blastAttack.attacker = base.gameObject;
                blastAttack.inflictor = base.gameObject;
                blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                blastAttack.position = base.FindModelChild("HandR").position;
                blastAttack.procCoefficient = 0f;
                blastAttack.radius = GigaImpactOld.impactAttackRadius;
                blastAttack.baseForce = GigaImpactOld.impactAttackForce;
                blastAttack.bonusForce = Vector3.up * GigaImpactOld.impactAttackBonusForce;
                blastAttack.baseDamage = GigaImpactOld.impactAttackDamageCoefficient * this.damageStat;
                blastAttack.falloffModel = BlastAttack.FalloffModel.SweetSpot;
                blastAttack.damageColorIndex = DamageColorIndex.Item;
                blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
                blastAttack.Fire();
            }
        }

        private void FireShockwave()
        {
            if (this.hasFiredShockwave) return;
            this.hasFiredShockwave = true;

            if (EntityStates.GrandParentBoss.SpawnState.spawnEffect)
            {
                EffectManager.SimpleMuzzleFlash(Resources.Load<GameObject>("prefabs/effects/impacteffects/GrandparentDeathEffect"), base.gameObject, "HandR", false);
            }
            
            if (base.isAuthority)
            {
                BlastAttack blastAttack = new BlastAttack();
                blastAttack.attacker = base.gameObject;
                blastAttack.inflictor = base.gameObject;
                blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                blastAttack.position = base.characterBody.corePosition;
                blastAttack.procCoefficient = 0f;
                blastAttack.radius = GigaImpactOld.blastAttackRadius;
                blastAttack.baseForce = GigaImpactOld.blastAttackForce;
                blastAttack.bonusForce = Vector3.up * GigaImpactOld.blastAttackBonusForce;
                blastAttack.baseDamage = GigaImpactOld.blastAttackDamageCoefficient * this.damageStat;
                blastAttack.falloffModel = BlastAttack.FalloffModel.Linear;
                blastAttack.damageColorIndex = DamageColorIndex.Item;
                blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
                blastAttack.Fire();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}