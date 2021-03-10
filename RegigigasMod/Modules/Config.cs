using BepInEx.Configuration;
using UnityEngine;

namespace RegigigasMod.Modules
{
    public static class Config
    {
        public static void ReadConfig()
        {

        }

        internal static ConfigEntry<bool> EnemyEnableConfig(string characterName)
        {
            return RegigigasPlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), true, new ConfigDescription("Set to false to disable this enemy"));
        }

        internal static ConfigEntry<bool> CharacterEnableConfig(string characterName)
        {
            return RegigigasPlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), false, new ConfigDescription("Set to false to disable this character"));
        }
    }
}