using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RegigigasMod.Modules
{
    internal static class Config
    {
        public static ConfigFile myConfig;

        internal static ConfigEntry<float> _shinySpawnRate;
        internal static ConfigEntry<bool> _nerfedEarthPower;
        internal static ConfigEntry<bool> _earthPowerAfterburn;
        internal static ConfigEntry<bool> _nerfedMelee;
        internal static ConfigEntry<bool> _loreFriendly;
        internal static ConfigEntry<bool> _loreFriendly2;
        internal static ConfigEntry<bool> _cssActualSize;

        public static List<StageSpawnInfo> StageList = new List<StageSpawnInfo>();

        internal static float shinySpawnRate;
        internal static bool nerfedEarthPower;
        internal static bool earthPowerAfterburn;
        internal static bool nerfedMelee;
        internal static bool loreFriendly;
        internal static bool loreFriendly2;
        internal static bool cssActualSize;

        internal static void ReadConfig()
        {
            Config.InitROO(Assets.mainAssetBundle.LoadAsset<Sprite>("texRegigigasPlayerIcon"), "Regi Pokémon, also known as the Legendary Titans, refers to the Legendary Pokémon Regirock, Regice, Registeel, Regigigas, [REDACTED] and [REDACTED].");

            _shinySpawnRate = 
                Config.BindAndOptions("Regigigas", 
                                                            "Shiny Chance", 
                                                            0.01220703125f, 
                                                            "Chance for spawned Regigigas to be shiny and drop Irradiant Pearls", true);

            shinySpawnRate = _shinySpawnRate.Value;

            _nerfedEarthPower =
                Config.BindAndOptions("Regigigas",
                                                           "Nerfed Earth Power",
                                                           true,
                                                           "Nerfs the projectile count on Earth Power. Set to false to restore the old unfair values.", true);

            nerfedEarthPower = _nerfedEarthPower.Value;

            _earthPowerAfterburn =
                Config.BindAndOptions("Regigigas",
                                                           "Earth Power Afterburn",
                                                           false,
                                                           "Earth Power applies afterburn. Set to true for the original experience.", true);

            earthPowerAfterburn = _earthPowerAfterburn.Value;

            _nerfedMelee =
                Config.BindAndOptions("Regigigas",
                                                           "Nerfed Melee",
                                                           false,
                                                           "Nerfs the melee attack of the boss version, by removing the grab and letting it punch instead. Set to false to use the unfair grab.", true);

            nerfedMelee = _nerfedMelee.Value;

            _loreFriendly =
                Config.BindAndOptions("Regigigas",
                                               "Lore Friendly",
                                               false,
                                               "Gives Regigigas an alternate name and model to make it fit in a little better.", true);

            loreFriendly = _loreFriendly.Value;

            _loreFriendly2 =
   Config.BindAndOptions("Regigigas (Playable)",
                                   "Lore Friendly",
                                   false,
                                   "Gives Regigigas (Player) an alternate name and model to make it fit in a little better.", true);

            loreFriendly2 = _loreFriendly2.Value;

            _cssActualSize =
                    Config.BindAndOptions("Regigigas",
                                   "CSS Actual Size",
                                   false,
                                   "Makes Regigigas use his proper size in the Character Select Screen.", true);

            cssActualSize = _cssActualSize.Value;


            string stages = RegigigasPlugin.instance.Config.Bind<string>(
                "Regigigas", 
                "Stage List",
                "shipgraveyard, frozenwall, goldshores, golemplains - loop, golemplains2 - loop, arena, goolake, foggyswamp, snowyforest, itfrozenwall, itgolemplains, itgoolake, rootjungle",
                "What stages the boss will show up on. Add a '- loop' after the stagename to make it only spawn after looping. List of stage names can be found at https://github.com/risk-of-thunder/R2Wiki/wiki/Mod-Creation_Developer-Reference_Scene-Names").Value;

            //parse stage
            stages = new string(stages.ToCharArray().Where(c => !System.Char.IsWhiteSpace(c)).ToArray());
            string[] splitStages = stages.Split(',');
            foreach (string str in splitStages) {
                string[] current = str.Split('-');
                
                string name = current[0];
                int minStages = 0;
                if (current.Length > 1) {
                    minStages = 5;
                }

                StageList.Add(new StageSpawnInfo(name, minStages));
            }
        }

        internal static ConfigEntry<bool> EnemyEnableConfig(string characterName)
        {
            return RegigigasPlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), true, new ConfigDescription("Set to false to disable this enemy"));
        }

        internal static ConfigEntry<bool> RiskyArtifactsOriginConfig(string characterName)
        {
            return RegigigasPlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Risky Artifacts: Add to Origination"), true, new ConfigDescription("Add this character to the Orignation boss spawn pool?"));
        }

        public static void InitROO(Sprite modSprite, string modDescription)
        {
            if (RegigigasPlugin.rooInstalled) _InitROO(modSprite, modDescription);
        }

        public static void _InitROO(Sprite modSprite, string modDescription)
        {
            ModSettingsManager.SetModIcon(modSprite);
            ModSettingsManager.SetModDescription(modDescription);
        }

        public static ConfigEntry<T> BindAndOptions<T>(string section, string name, T defaultValue, string description = "", bool restartRequired = false)
        {
            if (string.IsNullOrEmpty(description))
            {
                description = name;
            }

            if (restartRequired)
            {
                description += " (restart required)";
            }

            ConfigEntry<T> configEntry = myConfig.Bind(section, name, defaultValue, description);

            if (RegigigasPlugin.rooInstalled)
            {
                TryRegisterOption(configEntry, restartRequired);
            }

            return configEntry;
        }

        public static ConfigEntry<float> BindAndOptionsSlider(string section, string name, float defaultValue, string description = "", float min = 0, float max = 20, bool restartRequired = false)
        {
            if (string.IsNullOrEmpty(description))
            {
                description = name;
            }

            description += " (Default: " + defaultValue + ")";

            if (restartRequired)
            {
                description += " (restart required)";
            }

            ConfigEntry<float> configEntry = myConfig.Bind(section, name, defaultValue, description);

            if (RegigigasPlugin.rooInstalled)
            {
                TryRegisterOptionSlider(configEntry, min, max, restartRequired);
            }

            return configEntry;
        }

        public static ConfigEntry<int> BindAndOptionsSlider(string section, string name, int defaultValue, string description = "", int min = 0, int max = 20, bool restartRequired = false)
        {
            if (string.IsNullOrEmpty(description))
            {
                description = name;
            }

            description += " (Default: " + defaultValue + ")";

            if (restartRequired)
            {
                description += " (restart required)";
            }

            ConfigEntry<int> configEntry = myConfig.Bind(section, name, defaultValue, description);

            if (RegigigasPlugin.rooInstalled)
            {
                TryRegisterOptionSlider(configEntry, min, max, restartRequired);
            }

            return configEntry;
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void TryRegisterOption<T>(ConfigEntry<T> entry, bool restartRequired)
        {
            if (entry is ConfigEntry<float>)
            {
                ModSettingsManager.AddOption(new SliderOption(entry as ConfigEntry<float>, new SliderConfig() { min = 0, max = 20, formatString = "{0:0.00}", restartRequired = restartRequired }));
            }
            if (entry is ConfigEntry<int>)
            {
                ModSettingsManager.AddOption(new IntSliderOption(entry as ConfigEntry<int>, restartRequired));
            }
            if (entry is ConfigEntry<bool>)
            {
                ModSettingsManager.AddOption(new CheckBoxOption(entry as ConfigEntry<bool>, restartRequired));
            }
            if (entry is ConfigEntry<KeyboardShortcut>)
            {
                ModSettingsManager.AddOption(new KeyBindOption(entry as ConfigEntry<KeyboardShortcut>, restartRequired));
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void TryRegisterOptionSlider(ConfigEntry<int> entry, int min, int max, bool restartRequired)
        {
            ModSettingsManager.AddOption(new IntSliderOption(entry as ConfigEntry<int>, new IntSliderConfig() { min = min, max = max, formatString = "{0:0.00}", restartRequired = restartRequired }));
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void TryRegisterOptionSlider(ConfigEntry<float> entry, float min, float max, bool restartRequired)
        {
            ModSettingsManager.AddOption(new SliderOption(entry as ConfigEntry<float>, new SliderConfig() { min = min, max = max, formatString = "{0:0.00}", restartRequired = restartRequired }));
        }

        internal static ConfigEntry<bool> CharacterEnableConfig(string characterName)
        {
            return Config.BindAndOptions("Regigigas (Playable)",
                         "Enabled",
                         true,
                         "Set to false to disable this character", true);
        }

        internal static ConfigEntry<bool> ForceUnlockConfig(string characterName)
        {
            return Config.BindAndOptions("Regigigas (Playable)",
                         "Force Unlock",
                         false,
                         "Makes this character unlocked by default", true);
        }

        public static bool GetKeyPressed(ConfigEntry<KeyboardShortcut> entry)
        {
            foreach (var item in entry.Value.Modifiers)
            {
                if (!Input.GetKey(item))
                {
                    return false;
                }
            }
            return Input.GetKeyDown(entry.Value.MainKey);
        }
    }


    public class StageSpawnInfo {
        private string stageName;
        private int minStages;

        public StageSpawnInfo(string stageName, int minStages) {
            this.stageName = stageName;
            this.minStages = minStages;
        }

        public string GetStageName() { return stageName; }
        public int GetMinStages() { return minStages; }
    }
}