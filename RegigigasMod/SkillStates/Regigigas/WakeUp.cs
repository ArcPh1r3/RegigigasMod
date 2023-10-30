using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RegigigasMod.SkillStates.Regigigas
{
    class WakeUp : BaseRegiSkillState
    {
        public static float duration = 8f;
        public static float delayBeforeAimAnimatorWeight = 6.5f;

        private float cryTime;
        private bool hasCried;
        private Animator modelAnimator;

        public override void OnEnter()
        {
            base.OnEnter();
            //base.modelLocator.normalizeToFloor = true;
            this.modelAnimator = base.GetModelAnimator();
            this.cryTime = WakeUp.duration * 0.65f;
            this.hasCried = false;

            if (this.modelAnimator) this.modelAnimator.SetFloat(AnimationParameters.aimWeight, 0f);

            base.PlayAnimation("Body", "Spawn");
            Util.PlaySound("RegigigasSpawn", base.gameObject);

            base.flashController.Flash();

            if (NetworkServer.active) base.characterBody.AddTimedBuff(Modules.Buffs.armorBuff, WakeUp.duration);
        }

        public override void Update()
        {
            base.Update();

            if (this.modelAnimator) this.modelAnimator.SetFloat(AnimationParameters.aimWeight, Mathf.Clamp01((base.age - WakeUp.delayBeforeAimAnimatorWeight) / WakeUp.duration));
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.cryTime && !this.hasCried)
            {
                this.hasCried = true;

                string soundString = "RegigigasCry";
                if (Modules.Config.loreFriendly) soundString = "sfx_regigigas_altcry"; // it's the regirock cry lmao

                Util.PlaySound(soundString, this.gameObject);
            }
            
            if (base.fixedAge >= WakeUp.duration)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            if (this.modelAnimator) this.modelAnimator.SetFloat(AnimationParameters.aimWeight, 1f);

            base.OnExit();
        }
    }
}