using R2API;

namespace RegigigasMod.Modules {
    internal static class DamageTypes {

        public static DamageAPI.ModdedDamageType drainPunch;

        public static void RegisterDamageTypes() {

            drainPunch = DamageAPI.ReserveDamageType();
        }
    }
}