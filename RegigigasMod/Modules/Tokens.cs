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

            string lore = "There is an enduring legend that states this Pokémon towed continents with ropes.";

            LanguageAPI.Add(prefix + "NAME", "Regigigas");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "Weary Colossus");
            LanguageAPI.Add(prefix + "LORE", lore);
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MONSOON_SKIN_NAME", "Shiny");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_GRAB_NAME", "Crush Grip");
            LanguageAPI.Add(prefix + "PRIMARY_GRAB_DESCRIPTION", $"Grab a nearby enemy and crush them for <style=cIsHealth>50% of their max health</style>, then throw them away. BUGGED FOR NON HOSTS");

            LanguageAPI.Add(prefix + "PRIMARY_PUNCH_NAME", "Brick Break");
            LanguageAPI.Add(prefix + "PRIMARY_PUNCH_DESCRIPTION", $"Punch for <style=cIsDamage>280% damage</style>.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_EARTHQUAKE_NAME", "Earth Power");
            LanguageAPI.Add(prefix + "SECONDARY_EARTHQUAKE_DESCRIPTION", $"Stomp really really hard for <style=cIsDamage>{100f * 9}% damage</style>.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_REVENGE_NAME", "Revenge");
            LanguageAPI.Add(prefix + "UTILITY_REVENGE_DESCRIPTION", "Channel for a duration, gaining <style=cIsUtility>500 armor</style>. Then unleash a shockwave that returns all damage you receieved during this time.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_IMPACT_NAME", "Giga Impact");
            LanguageAPI.Add(prefix + "SPECIAL_IMPACT_DESCRIPTION", $"Do something cool for <style=cIsDamage>{100f * 80}% damage</style>.");
            #endregion
        }
    }
}