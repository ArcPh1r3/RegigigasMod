using RegigigasMod.SkillStates.Regigigas;
using RegigigasMod.SkillStates.BaseStates;
using System.Collections.Generic;
using System;

namespace RegigigasMod.Modules
{
    public static class States
    {
        internal static List<Type> entityStates = new List<Type>();

        internal static void AddSkill(Type t)
        {
            entityStates.Add(t);
        }

        public static void RegisterStates()
        {
            AddSkill(typeof(BaseRegiSkillState));

            AddSkill(typeof(SpawnState));
            AddSkill(typeof(WakeUp));
            AddSkill(typeof(MainState));
            AddSkill(typeof(JumpState));
            AddSkill(typeof(DeathState));

            AddSkill(typeof(BaseMeleeAttack));
            AddSkill(typeof(PunchCombo));
            AddSkill(typeof(DrainPunch));

            AddSkill(typeof(GrabAttempt));
            AddSkill(typeof(GrabFail));
            AddSkill(typeof(GrabSuccess));

            AddSkill(typeof(Stomp));
            AddSkill(typeof(ChargeAncientPower));
            AddSkill(typeof(FireAncientPower));

            AddSkill(typeof(Revenge));
            AddSkill(typeof(RevengeEnd));

            AddSkill(typeof(GigaImpact));
        }
    }
}