using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RegigigasMod.Modules.Components
{
    public class SlowStartController : MonoBehaviour
    {
        public Transform pivotTransform;

        private bool isEnemy;
        private bool inSlowStart;
        private float stopwatch;
        private CharacterBody body;

        private void Awake()
        {
            this.body = this.GetComponent<CharacterBody>();
            this.inSlowStart = true;
            this.stopwatch = 120f;
        }

        private void Start()
        {
            if (this.GetComponent<TeamComponent>().teamIndex != TeamIndex.Player)
            {
                this.isEnemy = true;
            }

            if (NetworkServer.active)
            {
                if (this.isEnemy)
                {
                    this.body.AddBuff(Modules.Buffs.slowStartBuff);
                }
                else
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (this.body.GetBuffCount(Modules.Buffs.slowStartBuff) < 10) this.body.AddBuff(Modules.Buffs.slowStartBuff);
                    }
                }
            }
        }

        private void ActivateSlowStart()
        {
            Animator anim = this.GetComponent<ModelLocator>().modelTransform.GetComponent<Animator>();
            anim.SetLayerWeight(anim.GetLayerIndex("Body, Smooth"), 1f);

            this.body.GetComponent<RegigigasFlashController>().Flash();
        }

        public void GrantKill()
        {
            if (this.isEnemy) return;

            //this.killsNeeded--;

            //if (this.killsNeeded <= 0)
            //{
            //    if (NetworkServer.active) this.body.RemoveBuff(Modules.Buffs.slowStartBuff);
            //}

            if (NetworkServer.active) this.body.RemoveBuff(Modules.Buffs.slowStartBuff);
        }

        private void FixedUpdate()
        {
            if (this.isEnemy)
            {
                this.stopwatch -= Time.fixedDeltaTime;
                if (this.stopwatch <= 0f)
                {
                    if (NetworkServer.active) this.body.RemoveBuff(Modules.Buffs.slowStartBuff);
                    this.ActivateSlowStart();
                    Destroy(this);
                }
            }
            else
            {
                if (this.inSlowStart)
                {
                    if (!this.body.HasBuff(Modules.Buffs.slowStartBuff))
                    {
                        this.inSlowStart = false;
                        this.ActivateSlowStart();
                        Destroy(this);
                    }
                }
            }
        }
    }
}