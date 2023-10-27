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

            if (Modules.Config.loreFriendly)
            {
                desc = "The Stone Juggernaut is a hulking beast that requires time and support to reach its full potential. If allowed to awaken, it dominates the battlefield with ease.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine; ;
            }

            desc = desc + "< ! > Use Drain Punch to survive until Slow Start is mitigated. Having allies protect you later into a run is essential!" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Ancient Power's health penalty can boost the damage of Revenge." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Crush Grip is terrible. Don't use it, please." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Reactivate Revenge to cancel it early. Knowing how much you can take is key to surviving with this." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so it left, leaving irreparable damage in its wake.";
            string outroFailure = "..and so it vanished, returning to its eternal slumber.";

            string lore = "There is an enduring legend that states this Pokémon towed continents with ropes.";

            string charName = "Regigigas";
            if (Modules.Config.loreFriendly) charName = "Stone Juggernaut";
            LanguageAPI.Add(prefix + "NAME", charName);
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "Weary Colossus");
            LanguageAPI.Add(prefix + "LORE", lore);
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            if (Modules.Config.loreFriendly) LanguageAPI.Add(prefix + "MONSOON_SKIN_NAME", "Gold");
            else LanguageAPI.Add(prefix + "MONSOON_SKIN_NAME", "Shiny");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Slow Start");
            if (Modules.Config.loreFriendly)
            {
                LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", $"<style=cIsHealth>Stats are halved</style> upon spawning. Defeating <style=cIsUtility>10 enemies</style> will restore the Stone Juggernaut to <style=cIsDamage>full power</style>.");
            }
            else
            {
                LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", $"<style=cIsHealth>Stats are halved</style> upon spawning. Defeating <style=cIsUtility>10 enemies</style> will restore Regigigas to <style=cIsDamage>full power</style>.");
            }
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
            LanguageAPI.Add(prefix + "SECONDARY_EARTHQUAKE_DESCRIPTION", $"Stomp with all your might, summoning 5 pillars of <style=cIsDamage>flame</style> that deal <style=cIsDamage>{100f * SkillStates.Regigigas.Stomp.damageCoefficient}% damage</style>. <style=cIsDamage>Inflicts burn.</style>");

            LanguageAPI.Add(prefix + "SECONDARY_ANCIENTPOWER_NAME", "Ancient Power");
            LanguageAPI.Add(prefix + "SECONDARY_ANCIENTPOWER_DESCRIPTION", $"Charge up a barrage of rocks for <style=cIsDamage>{100f * SkillStates.Regigigas.FireAncientPower.damageCoefficient}% damage</style> each. Costs <style=cIsHealth>10% max health</style> for each rock if out of stock.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_REVENGE_NAME", "Revenge");
            LanguageAPI.Add(prefix + "UTILITY_REVENGE_DESCRIPTION", "Channel for 8 seconds, gaining <style=cIsUtility>500 armor</style>. Then unleash <style=cIsDamage>3x all damage received during this time</style>.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_IMPACT_NAME", "Giga Impact");
            LanguageAPI.Add(prefix + "SPECIAL_IMPACT_DESCRIPTION", $"Leap high into the sky, dealing <style=cIsDamage>{100f * SkillStates.Regigigas.Bounce.damageCoefficient}% damage</style> around you in an explosive landing.");
            #endregion

            #region Achievements
            if (Modules.Config.loreFriendly)
            {
                LanguageAPI.Add(prefix + "MONSOONUNLOCKABLE_ACHIEVEMENT_NAME", "Stone Juggernaut: Mastery");
                LanguageAPI.Add(prefix + "MONSOONUNLOCKABLE_ACHIEVEMENT_DESC", "As Stone Juggernaut, beat the game or obliterate on Monsoon.");
                LanguageAPI.Add(prefix + "MONSOONUNLOCKABLE_UNLOCKABLE_NAME", "Stone Juggernaut: Mastery");
            }
            else
            {
                LanguageAPI.Add(prefix + "MONSOONUNLOCKABLE_ACHIEVEMENT_NAME", "Regigigas: Mastery");
                LanguageAPI.Add(prefix + "MONSOONUNLOCKABLE_ACHIEVEMENT_DESC", "As Regigigas, beat the game or obliterate on Monsoon.");
                LanguageAPI.Add(prefix + "MONSOONUNLOCKABLE_UNLOCKABLE_NAME", "Regigigas: Mastery");
            }
            #endregion

            #region Misc
            LanguageAPI.Add(prefix + "SLOW_START", "SLOW START...");
            LanguageAPI.Add(prefix + "SLOW_START_RELEASED", "FULL POWER!");
            #endregion
        }
    }
}