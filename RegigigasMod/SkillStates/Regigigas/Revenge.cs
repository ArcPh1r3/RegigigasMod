using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using RegigigasMod.Modules.Components;
using System.Linq;

namespace RegigigasMod.SkillStates.Regigigas
{
    public class Revenge : BaseRegiSkillState
    {
        public static float baseDuration = 8f;

        private float lastHealth;
        private float storedDamage;
        private float duration;
        private Animator modelAnimator;
        private GameObject chargeEffectInstance;
        private Transform areaIndicator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = Revenge.baseDuration;// / this.attackSpeedStat;
            this.lastHealth = base.healthComponent.combinedHealth;
            this.modelAnimator = base.GetModelAnimator();

            base.PlayAnimation("FullBody, Override", "RevengeEntry", "Revenge.playbackRate", this.duration * 0.1f);

            if (NetworkServer.active) base.characterBody.AddBuff(Modules.Buffs.armorBuff);

            Transform modelTransform = base.GetModelTransform();
            if (modelTransform)
            {
                TemporaryOverlay temporaryOverlay = modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                temporaryOverlay.duration = this.duration;
                temporaryOverlay.animateShaderAlpha = true;
                temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlay.destroyComponentOnEnd = true;
                temporaryOverlay.originalMaterial = Resources.Load<Material>("Materials/matDoppelganger");
                temporaryOverlay.AddToCharacerModel(modelTransform.GetComponent<CharacterModel>());
            }

            if (this.modelAnimator) this.modelAnimator.SetFloat(AnimationParameters.aimWeight, 0f);

            this.chargeEffectInstance = GameObject.Instantiate(new EntityStates.ImpBossMonster.BlinkState().blinkDestinationPrefab, base.gameObject.transform);
            this.chargeEffectInstance.transform.position = base.characterBody.corePosition;
            this.chargeEffectInstance.GetComponent<ScaleParticleSystemDuration>().newDuration = this.duration;
            this.areaIndicator = this.chargeEffectInstance.transform.Find("Particles").Find("AreaIndicator");

            base.cameraTargetParams.cameraParams = Modules.CameraParams.chargeCameraParams;
            this.chargeEffectInstance.GetComponentInChildren<PostProcessDuration>().maxDuration = this.duration;
        }

        private void UpdateRadius()
        {
            float healthPercentage = this.storedDamage / base.healthComponent.fullCombinedHealth;
            float radius = Util.Remap(healthPercentage, 0f, 1f, RevengeEnd.minRadius, RevengeEnd.maxRadius);
            this.areaIndicator.localScale = Vector3.one * radius;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            float diff = this.lastHealth - base.healthComponent.combinedHealth;
            if (diff > 0) this.storedDamage += diff;
            this.lastHealth = base.healthComponent.combinedHealth;

            if (this.areaIndicator) this.UpdateRadius();

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextState(new RevengeEnd()
                {
                    storedDamage = 3f * this.storedDamage
                });
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            if (this.chargeEffectInstance) EntityState.Destroy(this.chargeEffectInstance);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}