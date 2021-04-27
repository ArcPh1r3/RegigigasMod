using R2API;
using System;

namespace RegigigasMod.Modules
{
    internal static class Tokens
    {
        internal static void AddTokens()
        {
            string prefix = RegigigasPlugin.developerPrefix + "_REGIGIGAS_BODY_";

            string desc = "Regigigas.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > ." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > ." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > ." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > ." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so it left, leaving irreparable damage in its wake.";
            string outroFailure = "..and so it vanished, returning to its eternal slumber.";

            string lore = "There is an enduring legend that states this Pokémon towed continents with ropes.";

            LanguageAPI.Add(prefix + "NAME", "Regigigas");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "Weary Colossus");
            LanguageAPI.Add(prefix + "LORE", lore);
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MONSOON_SKIN_NAME", "Shiny");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Slow Start");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", $"<style=cIsHealth>Stats are halved</style> upon spawning. Defeating <style=cIsUtility>10 enemies</style> will restore Regigigas to <style=cIsDamage>full strength</style>.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_GRAB_NAME", "Crush Grip");
            LanguageAPI.Add(prefix + "PRIMARY_GRAB_DESCRIPTION", $"Grab a nearby enemy and crush them for <style=cIsHealth>50% of their max health</style>, then throw them away.");

            LanguageAPI.Add(prefix + "PRIMARY_PUNCH_NAME", "Brick Break");
            LanguageAPI.Add(prefix + "PRIMARY_PUNCH_DESCRIPTION", $"Punch for <style=cIsDamage>{SkillStates.Regigigas.PunchCombo.damageCoefficientOverride * 100f}% damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_DRAINPUNCH_NAME", "Drain Punch");
            LanguageAPI.Add(prefix + "PRIMARY_DRAINPUNCH_DESCRIPTION", $"Punch for <style=cIsDamage>{SkillStates.Regigigas.DrainPunch.damageCoefficientOverride * 100f}% damage</style>, <style=cIsHealing>healing for 50% of damage dealt</style>.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_EARTHQUAKE_NAME", "Earth Power");
            LanguageAPI.Add(prefix + "SECONDARY_EARTHQUAKE_DESCRIPTION", $"Stomp with all your might, summoning pillars of <style=cIsDamage>flame</style> that deal <style=cIsDamage>{100f * SkillStates.Regigigas.Stomp.damageCoefficient}% damage</style>. <style=cIsDamage>Inflicts burn.</style>");

            LanguageAPI.Add(prefix + "SECONDARY_ANCIENTPOWER_NAME", "Ancient Power");
            LanguageAPI.Add(prefix + "SECONDARY_ANCIENTPOWER_DESCRIPTION", $"Charge up a barrage of rocks for <style=cIsDamage>{100f * SkillStates.Regigigas.FireAncientPower.damageCoefficient}% damage</style> each. Costs <style=cIsHealth>10% max health</style> for each rock if out of stock.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_REVENGE_NAME", "Revenge");
            LanguageAPI.Add(prefix + "UTILITY_REVENGE_DESCRIPTION", "Channel for a duration, gaining <style=cIsUtility>500 armor</style>. Then unleash a shockwave that returns 3x all damage you receieved during this time.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_IMPACT_NAME", "Giga Impact");
            LanguageAPI.Add(prefix + "SPECIAL_IMPACT_DESCRIPTION", $"Do something cool for <style=cIsDamage>{100f * 80}% damage</style>.");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MONSOONUNLOCKABLE_ACHIEVEMENT_NAME", "Regigigas: Mastery");
            LanguageAPI.Add(prefix + "MONSOONUNLOCKABLE_ACHIEVEMENT_DESC", "As Regigigas, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MONSOONUNLOCKABLE_UNLOCKABLE_NAME", "Regigigas: Mastery");
            #endregion
        }
    }
}