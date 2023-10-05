using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using RegigigasMod.Modules.Components;
using System.Linq;

namespace RegigigasMod.SkillStates.Regigigas
{
    public class GrabAttempt : BaseRegiSkillState
    {
        public static float baseDuration = 2f;
        public static string grabTransformString = "HandR";
        public static float grabRadius = 10f;

        private float grabStartTime;
        private Transform grabTransform;
        private HurtBox grabTarget;
        private float duration;
        private RegigigasGrabController grabController;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = GrabAttempt.baseDuration / this.attackSpeedStat;
            this.grabStartTime = 0.7f * this.duration;
            this.grabTransform = base.FindModelChild(GrabAttempt.grabTransformString);

            base.PlayAnimation("FullBody, Override", "GrabAttempt", "Grab.playbackRate", this.duration);

            base.flashController.Flash();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.grabStartTime)
            {
                this.AttemptGrab(GrabAttempt.grabRadius);
            }

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                if (this.grabController)
                {
                    this.outer.SetNextState(new GrabSuccess()
                    {
                        target = this.grabTarget,
                        grabController = this.grabController
                    });

                    return;
                }
                else 
                {
                    this.outer.SetNextState(new GrabFail());
                    return;
                }
            }
        }

        private void AttemptGrab(float grabRadius)
        {
            if (this.grabController) return;

            Ray aimRay = base.GetAimRay();

            BullseyeSearch search = new BullseyeSearch
            {
                teamMaskFilter = TeamMask.GetEnemyTeams(base.GetTeam()),
                filterByLoS = false,
                searchOrigin = this.grabTransform.position,
                searchDirection = Random.onUnitSphere,
                sortMode = BullseyeSearch.SortMode.Distance,
                maxDistanceFilter = grabRadius,
                maxAngleFilter = 360f
            };

            search.RefreshCandidates();
            search.FilterOutGameObject(base.gameObject);

            HurtBox target = search.GetResults().FirstOrDefault<HurtBox>();
            if (target)
            {
                if (target.healthComponent && target.healthComponent.body)
                {
                    if (this.BodyMeetsGrabConditions(target.healthComponent.body))
                    {
                        this.grabController = target.healthComponent.body.gameObject.AddComponent<RegigigasGrabController>();
                        this.grabController.pivotTransform = this.grabTransform;
                        this.grabController.grabberHealthComponent = base.healthComponent;
                        this.grabTarget = target;

                        Util.PlaySound("sfx_regigigas_grab", this.gameObject);

                        return;
                    }
                }
            }
        }

        private bool BodyMeetsGrabConditions(CharacterBody targetBody)
        {
            bool meetsConditions = true;

            if (targetBody.hullClassification == HullClassification.BeetleQueen) meetsConditions = false;

            return meetsConditions;
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}