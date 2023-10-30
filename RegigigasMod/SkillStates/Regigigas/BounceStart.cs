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

                this.animator.CrossFadeInFixedTime("AnimatedJump", 0.1f);
                this.animator.Update(0f);

                if (!this.characterBody.HasBuff(Modules.Buffs.slowStartBuff)) this.duration = this.animator.GetNextAnimatorStateInfo(layerIndex).length * 0.25f;
                else this.duration = this.animator.GetNextAnimatorStateInfo(layerIndex).length;
            }

            if (!this.characterBody.HasBuff(Modules.Buffs.slowStartBuff)) this.animator.speed = 4f;
        }

        public override void OnExit()
        {
            base.OnExit();
            if (!this.characterBody.HasBuff(Modules.Buffs.slowStartBuff)) this.animator.speed = 1f;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (this.animator) this.animator.SetBool("isGrounded", false);

            this.characterMotor.velocity.y = 0f;

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