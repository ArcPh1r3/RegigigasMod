using EntityStates;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RegigigasMod.SkillStates.Regigigas.HyperBeam
{
    public class FireBeam : BaseState
    {
        public static float baseDuration = 1f;

        private float duration;
        private GameObject chargeInstance;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = FireBeam.baseDuration / this.attackSpeedStat;
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}