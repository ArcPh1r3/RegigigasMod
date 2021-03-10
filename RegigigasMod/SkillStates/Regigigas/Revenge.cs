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

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = Revenge.baseDuration / this.attackSpeedStat;
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
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            float diff = this.lastHealth - base.healthComponent.combinedHealth;
            if (diff > 0) this.storedDamage += diff;
            this.lastHealth = base.healthComponent.combinedHealth;

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextState(new RevengeEnd()
                {
                    storedDamage = 2f * this.storedDamage
                });
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}