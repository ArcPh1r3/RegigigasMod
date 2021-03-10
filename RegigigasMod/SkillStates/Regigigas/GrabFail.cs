using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using RegigigasMod.Modules.Components;
using System.Linq;

namespace RegigigasMod.SkillStates.Regigigas
{
    public class GrabFail : BaseSkillState
    {
        public static float baseDuration = 1.5f;

        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = GrabFail.baseDuration / this.attackSpeedStat;

            base.PlayAnimation("FullBody, Override", "GrabFail", "Grab.playbackRate", this.duration);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }
    }
}