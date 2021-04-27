using BepInEx.Configuration;

namespace RegigigasMod.Modules
{
    internal static class Config
    {
        internal static ConfigEntry<float> _shinySpawnRate;

        internal static float shinySpawnRate;

        internal static void ReadConfig()
        {
            _shinySpawnRate = RegigigasPlugin.instance.Config.Bind<float>(new ConfigDefinition("Regigigas", "Shiny Chance"), 0.01220703125f, new ConfigDescription("Chance for spawned Regigigas to be shiny and drop Irradiant Pearls"));

            shinySpawnRate = _shinySpawnRate.Value;
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