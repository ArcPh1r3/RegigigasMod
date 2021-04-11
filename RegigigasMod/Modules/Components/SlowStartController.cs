using RoR2;
using UnityEngine;

namespace RegigigasMod.Modules.Components
{
    public class SlowStartController : MonoBehaviour
    {
        public Transform pivotTransform;

        private float stopwatch;
        private CharacterBody body;

        private void Awake()
        {
            this.body = this.GetComponent<CharacterBody>();
            this.stopwatch = 120f;
            if (this.GetComponent<TeamComponent>().teamIndex == TeamIndex.Player) this.stopwatch = 30f;
        }

        private void FixedUpdate()
        {
            this.stopwatch -= Time.fixedDeltaTime;
            if (this.stopwatch <= 0f)
            {
                this.body.baseDamage *= 2f;
                this.body.baseMoveSpeed *= 2f;
                this.body.baseAttackSpeed *= 2f;
                this.body.baseArmor *= 0.5f;
                this.body.RecalculateStats();

                Animator anim = this.GetComponent<ModelLocator>().modelTransform.GetComponent<Animator>();
                anim.SetLayerWeight(anim.GetLayerIndex("Body, Smooth"), 1f);

                this.body.GetComponent<RegigigasFlashController>().Flash();

                Destroy(this);
            }
        }
    }
}