using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RegigigasMod.SkillStates.Regigigas
{
    public class SpawnState : BaseState
    {
        public static float minimumSleepDuration = 0.5f;

        private Animator modelAnimator;

        public override void OnEnter()
        {
            base.OnEnter();
            //base.modelLocator.normalizeToFloor = true;
            this.modelAnimator = base.GetModelAnimator();

            if (this.modelAnimator) this.modelAnimator.SetFloat(AnimationParameters.aimWeight, 0f);

            base.PlayAnimation("Body", "Inactive");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.isAuthority && base.fixedAge >= SpawnState.minimumSleepDuration && (base.inputBank.moveVector.sqrMagnitude >= Mathf.Epsilon || base.inputBank.CheckAnyButtonDown()))
            {
                this.outer.SetNextState(new WakeUp());
            }
        }

        public override void OnExit()
        {
            if (this.modelAnimator) this.modelAnimator.SetFloat(AnimationParameters.aimWeight, 1f);

            base.OnExit();
        }
    }
}