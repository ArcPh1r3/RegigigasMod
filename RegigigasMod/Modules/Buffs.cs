using R2API;
using RoR2;
using UnityEngine;

namespace RegigigasMod.Modules
{
    public static class Buffs
    {
        // regigigas armor buff
        internal static BuffIndex armorBuff;

        internal static void RegisterBuffs()
        {
            armorBuff = AddNewBuff("RegigigasArmorBuff", "Textures/BuffIcons/texBuffGenericShield", Color.grey, false, false);
        }

        // simple helper method
        internal static BuffIndex AddNewBuff(string buffName, string iconPath, Color buffColor, bool canStack, bool isDebuff)
        {
            CustomBuff tempBuff = new CustomBuff(new BuffDef
            {
                name = buffName,
                iconPath = iconPath,
                buffColor = buffColor,
                canStack = canStack,
                isDebuff = isDebuff,
                eliteIndex = EliteIndex.None
            });

            return BuffAPI.Add(tempBuff);
        }
    }
}