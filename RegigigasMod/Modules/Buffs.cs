using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace RegigigasMod.Modules
{
    public static class Buffs
    {
        // regigigas armor buff
        internal static BuffDef armorBuff;

        // regigigas slow start debuff
        internal static BuffDef slowStartBuff;

        internal static BuffDef fullPowerBuff;

        internal static List<BuffDef> buffDefs = new List<BuffDef>();

        internal static void RegisterBuffs()
        {
            armorBuff = AddNewBuff("RegigigasArmorBuff", Resources.Load<Sprite>("Textures/BuffIcons/texBuffGenericShield"), Color.grey, false, false);
            slowStartBuff = AddNewBuff("RegigigasSlowStartDebuff", Modules.Assets.secondaryAssetBundle.LoadAsset<Sprite>("texBuffSlowStart"), Color.yellow, true, false);
            fullPowerBuff = AddNewBuff("RegigigasFullPowerBuff", Modules.Assets.secondaryAssetBundle.LoadAsset<Sprite>("texBuffFullPower"), Color.yellow, false, false);
        }

        // simple helper method
        internal static BuffDef AddNewBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff)
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
            buffDef.name = buffName;
            buffDef.buffColor = buffColor;
            buffDef.canStack = canStack;
            buffDef.isDebuff = isDebuff;
            buffDef.eliteDef = null;
            buffDef.iconSprite = buffIcon;

            buffDefs.Add(buffDef);

            return buffDef;
        }
    }
}