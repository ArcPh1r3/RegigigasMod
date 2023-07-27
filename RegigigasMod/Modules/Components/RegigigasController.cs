using RoR2;
using UnityEngine;

namespace RegigigasMod.Modules.Components
{
    public class RegigigasController : MonoBehaviour
    {
        private CharacterBody characterBody;
        //private CharacterModel model;
        //private ChildLocator childLocator;

        private void Awake()
        {
            this.characterBody = this.gameObject.GetComponent<CharacterBody>();
            // never ended up doing anything with these so comment out until it's needed one day
            //this.childLocator = this.gameObject.GetComponentInChildren<ChildLocator>();
            //this.model = this.GetComponentInChildren<CharacterModel>();
        }

        private void Start()
        {
            if (this.characterBody)
            {
                // we don't talk about the old implementation of this
                this.characterBody.inventory.onInventoryChanged += CheckInventory;
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
            }
        }
    }
}
