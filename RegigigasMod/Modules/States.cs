using R2API;
using RegigigasMod.SkillStates.Regigigas;
using RegigigasMod.SkillStates.BaseStates;

namespace RegigigasMod.Modules
{
    public static class States
    {
        public static void RegisterStates()
        {
            LoadoutAPI.AddSkill(typeof(BaseRegiSkillState));

            LoadoutAPI.AddSkill(typeof(SpawnState));
            LoadoutAPI.AddSkill(typeof(WakeUp));

            LoadoutAPI.AddSkill(typeof(DeathState));

            LoadoutAPI.AddSkill(typeof(BaseMeleeAttack));
            LoadoutAPI.AddSkill(typeof(PunchCombo));

            LoadoutAPI.AddSkill(typeof(GrabAttempt));
            LoadoutAPI.AddSkill(typeof(GrabFail));
            LoadoutAPI.AddSkill(typeof(GrabSuccess));

            LoadoutAPI.AddSkill(typeof(Stomp));

            LoadoutAPI.AddSkill(typeof(Revenge));
            LoadoutAPI.AddSkill(typeof(RevengeEnd));
        }
    }
}