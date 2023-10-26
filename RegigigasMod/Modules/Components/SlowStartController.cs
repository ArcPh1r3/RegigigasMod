using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

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

                    EffectManager.SpawnEffect(Modules.Assets.slowStartEffect, new EffectData
                    {
                        origin = this.transform.position + new Vector3(0f, 5f, 0f),
                        rotation = Quaternion.identity
                    }, true);
                }
                else
                {
                    int count = 10;

                    bool cancel = false;

                    if (Run.instance) count = Mathf.Clamp(count - Run.instance.stageClearCount, 0, 10);
                    if (count <= 0) cancel = true;

                    // cancel on certain stages too
                    Scene currentScene = SceneManager.GetActiveScene();

                    switch (currentScene.name)
                    {
                        case "moon":
                            cancel = true;
                            break;
                        case "moon2":
                            cancel = true;
                            break;
                        case "voidraid":
                            cancel = true;
                            break;
                        case "bazaar":
                            cancel = true;
                            break;
                        case "arena":
                            cancel = true;
                            break;
                        case "goldshores":
                            cancel = true;
                            break;
                        case "limbo":
                            cancel = true;
                            break;
                        case "mysteryspace":
                            cancel = true;
                            break;
                        case "voidstage":
                            cancel = true;
                            break;
                    }

                    if (cancel)
                    {
                        this.inSlowStart = false;
                        this.ActivateSlowStart();
                        Destroy(this);
                        return;
                    }

                    for (int i = 0; i < count; i++)
                    {
                        if (this.body.GetBuffCount(Modules.Buffs.slowStartBuff) < 10) this.body.AddBuff(Modules.Buffs.slowStartBuff);
                    }

                    EffectManager.SpawnEffect(Modules.Assets.slowStartEffect, new EffectData
                    {
                        origin = this.transform.position + new Vector3(0f, 5f, 0f),
                        rotation = Quaternion.identity
                    }, true);
                }
            }
        }

        private void ActivateSlowStart()
        {
            Animator anim = this.GetComponent<ModelLocator>().modelTransform.GetComponent<Animator>();
            anim.SetLayerWeight(anim.GetLayerIndex("Body, Smooth"), 1f);

            this.body.GetComponent<RegigigasFlashController>().Flash();
            Util.PlaySound("sfx_regigigas_release", this.gameObject);

            if (NetworkServer.active)
            {
                this.body.AddBuff(Modules.Buffs.fullPowerBuff);

                EffectManager.SpawnEffect(Modules.Assets.slowStartReleasedEffect, new EffectData
                {
                    origin = this.transform.position + new Vector3(0f, 5f, 0f),
                    rotation = Quaternion.identity
                }, true);
            }
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