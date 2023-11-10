using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace RegigigasMod.SkillStates.Regigigas
{
    public class MachPunch : PunchCombo
    {
        internal new static float damageCoefficientOverride = 0.4f;

        private GameObject chargeEffectInstance;

        public override void OnEnter()
        {
            this.baseDuration = 0.6f;

            base.OnEnter();

            this.earlyExitTime = 0.3f / this.attackSpeedStat;
            this.attack.damage = MachPunch.damageCoefficientOverride * this.damageStat;
            this.attack.damageType = DamageType.Generic;
            this.attack.pushAwayForce = -100f;

            /*string muzzleString = "HandL";
            if (this.swingIndex == 1) muzzleString = "HandR";
            this.chargeEffectInstance = GameObject.Instantiate(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/ChargeMageIceBomb.prefab").WaitForCompletion());
            this.chargeEffectInstance.transform.parent = this.FindModelChild(muzzleString);
            this.chargeEffectInstance.transform.localPosition = new Vector3(0f, 0f, 0f);
            this.chargeEffectInstance.transform.localRotation = Quaternion.identity;
            this.chargeEffectInstance.transform.localScale = Vector3.one;*/

            //this.chargeEffectInstance.GetComponentInChildren<ObjectScaleCurve>().timeMax = 0.75f * (this.duration * this.attackStartTime);
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

            this.outer.SetNextState(new MachPunch
            {
                swingIndex = index
            });
        }

        protected override void PlaySwingEffect()
        {
            if (this.chargeEffectInstance) EntityState.Destroy(this.chargeEffectInstance);
        }

        protected override void OnHitEnemyAuthority()
        {
            base.OnHitEnemyAuthority();

            GameObject j = Modules.Assets.punchImpactEffect;
            HurtBox[] h = hitResults.ToArray();
            for (int i = 0; i < hitResults.Count; i++)
            {
                EffectData effectData = new EffectData();
                effectData.scale = 3f;

                // extra oomph
                if (h[i] && h[i].healthComponent)
                {
                    effectData.origin = h[i].transform.position;
                    effectData.rotation = h[i].transform.rotation;

                    EffectManager.SpawnEffect(j, effectData, true);
                }
            }
        }
    }
}