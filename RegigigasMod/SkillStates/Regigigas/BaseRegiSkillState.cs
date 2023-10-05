using EntityStates;
using RegigigasMod.Modules.Components;
using UnityEngine;

namespace RegigigasMod.SkillStates.Regigigas
{
    public class BaseRegiSkillState : BaseSkillState
    {
        protected RegigigasController regigigasController;
        protected RegigigasFlashController flashController;
        protected Animator anim;

        public override void OnEnter()
        {
            this.regigigasController = this.GetComponent<RegigigasController>();
            this.flashController = base.GetComponent<RegigigasFlashController>();
            this.anim = base.GetModelAnimator();
            if (this.anim) this.anim.SetBool("isSprinting", false);

            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (this.anim) this.anim.SetBool("isGrounded", this.isGrounded);
        }
    }
}