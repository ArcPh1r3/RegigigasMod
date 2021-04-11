using R2API;
using R2API.Utils;
using RoR2;
using System;

namespace RegigigasMod.Modules.Achievements
{
    public class MasteryAchievement : ModdedUnlockableAndAchievement<CustomSpriteProvider>
    {
        public override String AchievementIdentifier { get; } = RegigigasPlugin.developerPrefix + "_REGIGIGAS_BODY_MONSOONUNLOCKABLE_ACHIEVEMENT_ID";
        public override String UnlockableIdentifier { get; } = RegigigasPlugin.developerPrefix + "_REGIGIGAS_BODY_MONSOONUNLOCKABLE_REWARD_ID";
        public override String PrerequisiteUnlockableIdentifier { get; } = RegigigasPlugin.developerPrefix + "_REGIGIGAS_BODY_UNLOCKABLE_REWARD_ID";
        public override String AchievementNameToken { get; } = RegigigasPlugin.developerPrefix + "_REGIGIGAS_BODY_MONSOONUNLOCKABLE_ACHIEVEMENT_NAME";
        public override String AchievementDescToken { get; } = RegigigasPlugin.developerPrefix + "_REGIGIGAS_BODY_MONSOONUNLOCKABLE_ACHIEVEMENT_DESC";
        public override String UnlockableNameToken { get; } = RegigigasPlugin.developerPrefix + "_REGIGIGAS_BODY_MONSOONUNLOCKABLE_UNLOCKABLE_NAME";
        protected override CustomSpriteProvider SpriteProvider { get; } = new CustomSpriteProvider("@Regigigas:Assets/Regigigas/Icons/texMasteryAchievement.png");

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(Modules.Enemies.Regigigas.bodyName);
        }

        public void ClearCheck(Run run, RunReport runReport)
        {
            if (run is null) return;
            if (runReport is null) return;

            if (!runReport.gameEnding) return;

            if (runReport.gameEnding.isWin)
            {
                DifficultyDef difficultyDef = DifficultyCatalog.GetDifficultyDef(runReport.ruleBook.FindDifficulty());

                if (difficultyDef != null && difficultyDef.countsAsHardMode)
                {
                    if (base.meetsBodyRequirement)
                    {
                        base.Grant();
                    }
                }
            }
        }

        public override void OnInstall()
        {
            base.OnInstall();

            Run.onClientGameOverGlobal += this.ClearCheck;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            Run.onClientGameOverGlobal -= this.ClearCheck;
        }
    }
}