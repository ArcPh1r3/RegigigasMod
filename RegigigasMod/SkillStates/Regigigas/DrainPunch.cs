using R2API.Networking;
using R2API.Networking.Interfaces;
using RegigigasMod.SkillStates.BaseStates;
using RoR2;
using RoR2.Networking;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace RegigigasMod.SkillStates.Regigigas
{
    public class DrainPunch : PunchCombo
    {
        internal new static float damageCoefficientOverride = 2.8f;

        public override void OnEnter()
        {
            base.OnEnter();

            this.attack.damage = DrainPunch.damageCoefficientOverride * this.damageStat;

            if (this.swingIndex == 1) EffectManager.SimpleMuzzleFlash(Modules.Assets.drainPunchChargeEffect, base.gameObject, "HandR", false);
            else EffectManager.SimpleMuzzleFlash(Modules.Assets.drainPunchChargeEffect, base.gameObject, "HandL", false);
        }

        protected override void SetNextState()
        {
            int index = this.swingIndex + 1;
            if (index == 3) index = 1;

            this.outer.SetNextState(new DrainPunch
            {
                swingIndex = index
            });
        }

        protected override void OnHitEnemyAuthority() {
            base.OnHitEnemyAuthority();

            GameObject j = Modules.Assets.punchImpactEffect;
            HurtBox[] h = hitResults.ToArray();
            for (int i = 0; i < hitResults.Count; i++)
            {
                handleLifeSteal(healthComponent, damageCoefficientOverride * this.damageStat * 0.5f);

                EffectData effectData = new EffectData();
                effectData.scale = 4f;

                // extra oomph
                if (h[i] && h[i].healthComponent)
                {
                    effectData.origin = h[i].transform.position;
                    effectData.rotation = h[i].transform.rotation;

                    EffectManager.SpawnEffect(j, effectData, true);
                }
            }
        }

        private void handleLifeSteal(HealthComponent healthComponent, float healAmount) {

            if (NetworkServer.active) {

                healthComponent.Heal(healAmount, default(ProcChainMask));

            } else {

                new Modules.NetMessages.SyncLifeSteal(characterBody.networkIdentity.netId, healAmount).Send(NetworkDestination.Clients);
            }
        }
    }


}