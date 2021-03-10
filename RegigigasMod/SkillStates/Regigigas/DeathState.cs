using EntityStates;
using UnityEngine;

namespace RegigigasMod.SkillStates.Regigigas
{
    public class DeathState : GenericCharacterDeath
    {
        public override void OnEnter()
        {
            Animator anim = base.GetModelAnimator();
            anim.SetLayerWeight(anim.GetLayerIndex("Body, Smooth"), 0f);
            base.PlayAnimation("FullBody, Override", "BufferEmpty");

            base.OnEnter();
        }
    }
}