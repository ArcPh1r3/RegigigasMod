using EntityStates;
using EntityStates.VagrantMonster;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using RegigigasMod.Modules.Components;
using System.Linq;

namespace RegigigasMod.SkillStates.Regigigas
{
    public class RevengeEnd : BaseRegiSkillState
    {
        public static float baseDuration = 6f;
        public float storedDamage;

        public static float maxDamagePercentage = 0.4f;
        public static float minDamagePercentage = 0f;

        public static float maxRadius = 80f;
        public static float minRadius = 12f;

        private float shockwaveRadius;
        private float shockwaveTime;
        private bool hasFired;
        private float duration;
        private GameObject chargeEffectInstance;
        private uint soundID;
        private Animator modelAnimator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = RevengeEnd.baseDuration / this.attackSpeedStat;
            this.shockwaveTime = 0.3f * this.duration;
            this.hasFired = false;
            this.modelAnimator = base.GetModelAnimator();

            float healthPercentage = this.storedDamage / base.healthComponent.fullCombinedHealth;

            this.shockwaveRadius = Util.Remap(healthPercentage, 0f, 1f, RevengeEnd.minRadius, RevengeEnd.maxRadius);

            base.PlayAnimation("FullBody, Override", "RevengeEnd", "Revenge.playbackRate", this.duration);
            this.soundID = Util.PlayAttackSpeedSound(ChargeMegaNova.chargingSoundString, base.gameObject, this.attackSpeedStat);

            ChildLocator childLocator = base.GetModelChildLocator();
            if (childLocator)
            {
                Transform pivot = childLocator.FindChild("Chest");

                if (pivot && ChargeMegaNova.chargingEffectPrefab)
                {
                    this.chargeEffectInstance = UnityEngine.Object.Instantiate<GameObject>(ChargeMegaNova.chargingEffectPrefab, pivot.position, pivot.rotation);
                    this.chargeEffectInstance.transform.localScale = new Vector3(this.shockwaveRadius, this.shockwaveRadius, this.shockwaveRadius);
                    this.chargeEffectInstance.transform.parent = pivot;
                    this.chargeEffectInstance.GetComponent<ScaleParticleSystemDuration>().newDuration = 0.3f * this.duration;
                }
            }

            if (NetworkServer.active) base.characterBody.RemoveBuff(Modules.Buffs.armorBuff);

            base.flashController.Flash();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.shockwaveTime)
            {
                this.FireShockwave();
            }

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        private void FireShockwave()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                AkSoundEngine.StopPlayingID(this.soundID);

                Vector3 position = base.transform.position;
                Util.PlaySound("UNUNUN", base.gameObject);

                if (FireMegaNova.novaEffectPrefab) EffectManager.SimpleMuzzleFlash(FireMegaNova.novaEffectPrefab, base.gameObject, "Chest", false);

                Transform modelTransform = base.GetModelTransform();
                if (modelTransform)
                {
                    TemporaryOverlay temporaryOverlay = modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                    temporaryOverlay.duration = 3f;
                    temporaryOverlay.animateShaderAlpha = true;
                    temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    temporaryOverlay.destroyComponentOnEnd = true;
                    temporaryOverlay.originalMaterial = Resources.Load<Material>("Materials/matVagrantEnergized");
                    temporaryOverlay.AddToCharacerModel(modelTransform.GetComponent<CharacterModel>());
                }

                if (base.isAuthority)
                {
                    new BlastAttack
                    {
                        attacker = base.gameObject,
                        baseDamage = this.storedDamage,
                        baseForce = FireMegaNova.novaForce,
                        bonusForce = Vector3.zero,
                        attackerFiltering = AttackerFiltering.NeverHit,
                        crit = base.RollCrit(),
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        falloffModel = BlastAttack.FalloffModel.None,
                        inflictor = base.gameObject,
                        position = position,
                        procChainMask = default(ProcChainMask),
                        procCoefficient = 1f,
                        radius = this.shockwaveRadius,
                        losType = BlastAttack.LoSType.NearestHit,
                        teamIndex = base.teamComponent.teamIndex,
                        impactEffect = EffectCatalog.FindEffectIndexFromPrefab(FireMegaNova.novaImpactEffectPrefab)
                    }.Fire();
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            if (this.modelAnimator) this.modelAnimator.SetFloat(AnimationParameters.aimWeight, 1f);
            if (this.chargeEffectInstance) EntityState.Destroy(this.chargeEffectInstance);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}