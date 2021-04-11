using RegigigasMod.SkillStates.Regigigas;
using RegigigasMod.SkillStates.BaseStates;
using System.Collections.Generic;
using MonoMod.RuntimeDetour;
using System;
using EntityStates;
using System.Reflection;
using RoR2;
using RoR2.Skills;

namespace RegigigasMod.Modules
{
    public static class States
    {
        internal static List<Type> entityStates = new List<Type>();

        private static Hook set_stateTypeHook;
        private static Hook set_typeNameHook;
        private static readonly BindingFlags allFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;
        private delegate void set_stateTypeDelegate(ref SerializableEntityStateType self, Type value);
        private delegate void set_typeNameDelegate(ref SerializableEntityStateType self, String value);

        internal static void AddSkill(Type t)
        {
            entityStates.Add(t);
        }

        public static void RegisterStates()
        {
            AddSkill(typeof(BaseRegiSkillState));

            AddSkill(typeof(SpawnState));
            AddSkill(typeof(WakeUp));
            AddSkill(typeof(MainState));
            AddSkill(typeof(JumpState));
            AddSkill(typeof(DeathState));

            AddSkill(typeof(BaseMeleeAttack));
            AddSkill(typeof(PunchCombo));
            AddSkill(typeof(DrainPunch));

            AddSkill(typeof(GrabAttempt));
            AddSkill(typeof(GrabFail));
            AddSkill(typeof(GrabSuccess));

            AddSkill(typeof(Stomp));
            AddSkill(typeof(ChargeAncientPower));
            AddSkill(typeof(FireAncientPower));

            AddSkill(typeof(Revenge));
            AddSkill(typeof(RevengeEnd));

            AddSkill(typeof(GigaImpact));

            // fixing a vanilla bug- ignore this
            Type type = typeof(SerializableEntityStateType);
            HookConfig cfg = default;
            cfg.Priority = Int32.MinValue;
            set_stateTypeHook = new Hook(type.GetMethod("set_stateType", allFlags), new set_stateTypeDelegate(SetStateTypeHook), cfg);
            set_typeNameHook = new Hook(type.GetMethod("set_typeName", allFlags), new set_typeNameDelegate(SetTypeName), cfg);
            //
        }

        // ignore this
        private static void SetStateTypeHook(ref this SerializableEntityStateType self, Type value)
        {
            self._typeName = value.AssemblyQualifiedName;
        }

        private static void SetTypeName(ref this SerializableEntityStateType self, String value)
        {
            Type t = GetTypeFromName(value);
            if (t != null)
            {
                self.SetStateTypeHook(t);
            }
        }

        private static Type GetTypeFromName(String name)
        {
            Type[] types = EntityStateCatalog.stateIndexToType;
            return Type.GetType(name);
        }
    }
}