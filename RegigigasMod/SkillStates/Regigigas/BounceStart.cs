using System;
using RoR2;
using UnityEngine;
using EntityStates;

namespace RegigigasMod.SkillStates.Regigigas
{
    public class BounceStart : BaseState
    {
        private float duration;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = base.GetModelAnimator();

            if (this.animator)
            {
                int layerIndex = this.animator.GetLayerIndex("Body");

                this.animator.CrossFadeInFixedTime("AnimatedJump", 0.25f);
                this.animator.Update(0f);

                this.duration = this.animator.GetNextAnimatorStateInfo(layerIndex).length;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (this.animator) this.animator.SetBool("isGrounded", false);

            if (base.fixedAge >= this.duration / 2f && base.isAuthority)
            {
                this.outer.SetNextState(new Bounce());
                return;
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextState(new Bounce());
                return;
            }
        }
    }
}