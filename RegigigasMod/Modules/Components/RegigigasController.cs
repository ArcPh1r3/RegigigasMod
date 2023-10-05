using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RegigigasMod.Modules.Components
{
    public struct RockData
    {
        public GameObject rock;
        public Vector3 position;
    }

    public class RegigigasController : MonoBehaviour
    {
        public int rockCount;

        private CharacterBody characterBody;
        private CharacterModel model;
        private ChildLocator childLocator;
        private GameObject rockPivot;
        private RockData[] rocks;
        private int activeRocks;

        private void Awake()
        {
            this.characterBody = this.gameObject.GetComponent<CharacterBody>();
            // never ended up doing anything with these so comment out until it's needed one day
            this.childLocator = this.gameObject.GetComponentInChildren<ChildLocator>();
            this.model = this.GetComponentInChildren<CharacterModel>();
            // oooo mama
            //   sexo

            // so the way this works is basically
            //  > set up a constantly rotating empty object on the character model
            //  > if rockCount isn't equal to the active displayed rocks, iterate on that
            //  > displayed rocks are set a certain distance from the center of the pivot, so they rotate around the body
            this.SetUpRockPivot();
            this.SetUpRocks();
        }

        private void SetUpRockPivot()
        {
            this.rockPivot = new GameObject();
            this.rockPivot.name = "RockPivot";
            this.rockPivot.transform.parent = this.transform;
            this.rockPivot.transform.localPosition = new Vector3(0f, 0.075f, 0f);
            this.rockPivot.transform.localRotation = Quaternion.identity;
        }

        private void SetUpRocks()
        {
            List<RockData> newRocks = new List<RockData>();

            GameObject modelPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Grandparent/GrandparentMiniBoulderGhost.prefab").WaitForCompletion();

            // limit is 30 because performance? idk why the fuck would you ever have more than 30 rocks
            for (int i = 0; i < 30; i++)
            {
                GameObject newRock = new GameObject();
                newRock.name = "RockOrSomethingIdk";
                newRock.transform.parent = this.rockPivot.transform;
                newRock.transform.localRotation = Quaternion.identity;

                // add the model here
                GameObject model = GameObject.Instantiate(modelPrefab);
                model.transform.parent = newRock.transform;
                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;

                // this is awful. but it only runs 30 times on spawn
                Destroy(model.GetComponent<RoR2.Projectile.ProjectileGhostController>());

                newRocks.Add(new RockData
                {
                    rock = newRock,
                    position = Vector3.zero
                });

                newRock.SetActive(false);
            }

            this.rocks = newRocks.ToArray();
        }

        private void Start()
        {
            if (this.characterBody)
            {
                // we don't talk about the old implementation of this
                this.characterBody.inventory.onInventoryChanged += CheckInventory;
            }
        }

        private void FixedUpdate()
        {
            this.HandleRocks();
        }

        private void HandleRocks()
        {
            // spinny wahoo
            this.rockPivot.transform.Rotate(new Vector3(0f, 128f * Time.deltaTime, 0f));

            if (this.rockCount > this.activeRocks)
            {
                this.AddRock();
            }

            if (this.rockCount < this.activeRocks)
            {
                this.KillRock();
            }
        }

        private void AddRock()
        {
            for (int i = 0; i < this.rocks.Length; i++)
            {
                // find an inactive rock to activate
                if (!this.rocks[i].rock.activeSelf)
                {
                    this.activeRocks++;

                    this.rocks[i].position = this.rockPivot.transform.position + new Vector3(0f, Random.Range(-2f, 2f), Random.Range(6f, 7f));
                    this.rocks[i].rock.transform.position = this.rocks[i].position;
                    this.rocks[i].rock.transform.rotation = Quaternion.Euler(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
                    this.rocks[i].rock.SetActive(true);
                    return;
                }
            }
        }

        private void KillRock()
        {
            for (int i = 0; i < this.rocks.Length; i++)
            {
                // and conversely, find an active rock to disable
                if (this.rocks[i].rock.activeSelf)
                {
                    this.activeRocks--;
                    this.rocks[i].rock.SetActive(false);
                    return;
                }
            }
        }

        private void CheckInventory()
        {
            if (this.characterBody && this.characterBody.inventory)
            {
                if (this.characterBody.inventory.GetItemCount(RoR2Content.Items.LunarPrimaryReplacement) > 0)
                {
                    this.characterBody.hideCrosshair = false;
                }
                else this.characterBody.hideCrosshair = true;
            }
        }
    }
}