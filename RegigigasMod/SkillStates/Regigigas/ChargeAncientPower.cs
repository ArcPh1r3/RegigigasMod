using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using static RoR2.CameraTargetParams;

namespace RegigigasMod.SkillStates.Regigigas
{
    public class ChargeAncientPower : BaseRegiSkillState
    {
        public static float baseStartDuration = 0.5f;
        public static float baseRockTimer = 0.3f;
        public static float healthCostFraction = 0.1f;

        private int rockCount;
        private float rockTimer;
        private float rockStopwatch;
        private float startDuration;
        private uint playID;

        private CameraParamsOverrideHandle camParamsOverrideHandle;

        public override void OnEnter()
        {
            base.OnEnter();
            this.startDuration = ChargeAncientPower.baseStartDuration;// / this.attackSpeedStat;
            this.rockTimer = ChargeAncientPower.baseRockTimer;// / this.attackSpeedStat;
            this.rockCount = 0;
            this.rockStopwatch = this.startDuration + this.rockTimer;
            this.camParamsOverrideHandle = Modules.CameraParams.OverrideCameraParams(base.cameraTargetParams, RegigigasCameraParams.AIM, 0.5f);

            base.PlayAnimation("Gesture, Override", "ChargeAncientPower", "AncientPower.playbackRate", this.startDuration);

            base.characterBody.hideCrosshair = false;


            this.playID = Util.PlaySound("sfx_regigigas_rocks_loop", this.gameObject);
        }

        public override void OnExit()
        {
            base.OnExit();

            AkSoundEngine.StopPlayingID(this.playID);

            this.cameraTargetParams.RemoveParamsOverride(this.camParamsOverrideHandle, 0.5f);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.StartAimMode(0.5f);
            this.rockStopwatch -= Time.fixedDeltaTime;

            this.regigigasController.rockCount = this.rockCount;

            if (this.rockStopwatch <= 0f)
            {
                this.ChargeRock();
            }

            if (base.isAuthority && base.fixedAge >= this.startDuration && !base.inputBank.skill2.down)
            {
                if (this.rockCount > 0)
                {
                    FireAncientPower nextState = new FireAncientPower
                    {
                        rockCount = this.rockCount
                    };
                    this.outer.SetNextState(nextState);
                    return;
                }
                else
                {
                    base.PlayAnimation("Gesture, Override", "BufferEmpty");
                    this.outer.SetNextStateToMain();
                    return;
                }
            }
        }

        private bool SpendStockOrHealth()
        {
            bool success = false;

            if (base.skillLocator.secondary.stock > 0)
            {
                base.skillLocator.secondary.DeductStock(1);
                success = true;
            }
            else
            {
                if (base.healthComponent)
                {
                    if (base.healthComponent.combinedHealth >= (ChargeAncientPower.healthCostFraction * base.healthComponent.fullCombinedHealth))
                    {
                        if (NetworkServer.active)
                        {
                            DamageInfo damageInfo = new DamageInfo();
                            damageInfo.damage = base.healthComponent.fullCombinedHealth * ChargeAncientPower.healthCostFraction;
                            damageInfo.position = base.characterBody.corePosition;
                            damageInfo.force = Vector3.zero;
                            damageInfo.damageColorIndex = DamageColorIndex.Default;
                            damageInfo.crit = false;
                            damageInfo.attacker = null;
                            damageInfo.inflictor = null;
                            damageInfo.damageType = (DamageType.NonLethal | DamageType.BypassArmor);
                            damageInfo.procCoefficient = 0f;
                            damageInfo.procChainMask = default(ProcChainMask);
                            base.healthComponent.TakeDamage(damageInfo);
                        }
                        success = true;
                    }
                }
            }

            return success;
        }

        private void ChargeRock()
        {
            if (this.SpendStockOrHealth())
            {
                this.rockCount++;
                this.flashController.Flash(false);
                Util.PlaySound("sfx_regigigas_rock_spawn", this.gameObject);
            }

            this.rockStopwatch = this.rockTimer;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}