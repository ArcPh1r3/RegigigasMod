using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RegigigasMod.SkillStates.Regigigas
{
    class WakeUp : BaseRegiSkillState
    {
        public static float duration = 8f;
        public static float delayBeforeAimAnimatorWeight = 6.5f;

        private float cryTime;
        private bool hasCried;
        private Animator modelAnimator;

        public override void OnEnter()
        {
            base.OnEnter();
            //base.modelLocator.normalizeToFloor = true;
            this.modelAnimator = base.GetModelAnimator();
            this.cryTime = WakeUp.duration * 0.65f;
            this.hasCried = false;

            if (this.modelAnimator) this.modelAnimator.SetFloat(AnimationParameters.aimWeight, 0f);

            base.PlayAnimation("Body", "Spawn");
            Util.PlaySound("RegigigasSpawn", base.gameObject);

            base.flashController.Flash();

            if (NetworkServer.active) base.characterBody.AddTimedBuff(Modules.Buffs.armorBuff, WakeUp.duration);
        }

        public override void Update()
        {
            base.Update();

            if (this.modelAnimator) this.modelAnimator.SetFloat(AnimationParameters.aimWeight, Mathf.Clamp01((base.age - WakeUp.delayBeforeAimAnimatorWeight) / WakeUp.duration));
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.cryTime && !this.hasCried)
            {
                this.hasCried = true;
                Util.PlaySound("RegigigasCry", base.gameObject);
            }
            
            if (base.fixedAge >= WakeUp.duration)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            if (this.modelAnimator) this.modelAnimator.SetFloat(AnimationParameters.aimWeight, 1f);

            base.OnExit();
        }
    }
}
/*
TheTimesweeper-FastArtificerBolts-2.2.1
TheTimesweeper-BetterHudLite-0.1.4
TheTimesweeper-SurvivorSortOrder-0.1.2
TheTimesweeper-AcridHitboxBuff-1.1.1
TheTimesweeper-DimmaBandit-0.3.2
TheTimesweeper-SillyItems-1.2.2

Flyingcomputer-Exchange_Changes-1.0.8
ThinkInvis-Yeet-3.0.1
XoXFaby-BetterUI-2.5.12
IHarbHD-DebugToolkit-3.7.1
niwith-DropinMultiplayer-2.0.0
KingEnderBrine-ProperSave-2.8.7

>EnforcerGang-Enforcer-3.3.9
>EnforcerGang-SniperClassic-1.1.3
>EnforcerGang-HAND_OVERCLOCKED-0.2.7
>EnforcerGang-MinerUnearthed-1.8.3
>Gnome-ChefMod-2.1.1
>Paladin_Alliance-PaladinMod-1.5.10
>TheTimesweeper-HenryMod-2.1.1
>TheTimesweeper-Tesla_Trooper-1.1.1
>Bog-Pathfinder-0.2.5

>KosmosisDire-TeammateRevival-4.1.3
>NotTsunami-ShowDeathCause-3.0.1
>Anreol-VoidQoL-1.1.4
>Risky_Lives-RiskyMod-0.10.12

>ThinkInvis-TinkersSatchel-3.4.1 - over half the useless shit removed
>ThinkInvis-ClassicItems-7.1.0 - over half the useless shit removed
>KomradeSpectre-Aetherium-0.6.8
>amogus_lovers-StandaloneAncientScepter-1.1.1
>William758-ZetAspects-2.7.16
>EnforcerGang-Direseeker-1.3.3
>TheTimesweeper-Regigigas-1.3.0

>Wolfo-ArtifactOfDissimilarity-2.0.2
>Volvary-SkillsPlusPlus-0.4.5
>RyanPallesen-FW_Artifacts-2.1.1
>Moffein-Risky_Artifacts-1.6.0
>Groove_Salad-UntitledDifficultyMod-1.0.1
>HIFU-Inferno-1.4.1
>ThinkInvis-Dronemeld-1.2.0*/