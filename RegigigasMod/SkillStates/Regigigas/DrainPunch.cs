using EntityStates;
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

        private GameObject chargeEffectInstance;

        public override void OnEnter()
        {
            base.OnEnter();

            this.attack.damage = DrainPunch.damageCoefficientOverride * this.damageStat;

            string muzzleString = "HandL";
            if (this.swingIndex == 1) muzzleString = "HandR";
            this.chargeEffectInstance = GameObject.Instantiate(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Grandparent/ChargeGrandParentSunHands.prefab").WaitForCompletion());
            this.chargeEffectInstance.transform.parent = this.FindModelChild(muzzleString);
            this.chargeEffectInstance.transform.localPosition = new Vector3(0f, 0f, 0f);
            this.chargeEffectInstance.transform.localRotation = Quaternion.identity;
            this.chargeEffectInstance.transform.localScale = Vector3.one;

            this.chargeEffectInstance.GetComponentInChildren<ObjectScaleCurve>().timeMax = 0.5f;
        }

        public override void OnExit()
        {
            base.OnExit();
            if (this.chargeEffectInstance) EntityState.Destroy(this.chargeEffectInstance);
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

        protected override void PlaySwingEffect()
        {
            if (this.chargeEffectInstance) EntityState.Destroy(this.chargeEffectInstance);
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