using RoR2;
using UnityEngine;

namespace RegigigasMod.Modules.Components
{
    public class RegigigasController : MonoBehaviour
    {
        private CharacterBody characterBody;
        private CharacterModel model;
        private ChildLocator childLocator;

        private void Awake()
        {
            this.characterBody = this.gameObject.GetComponent<CharacterBody>();
            this.childLocator = this.gameObject.GetComponentInChildren<ChildLocator>();
            this.model = this.GetComponentInChildren<CharacterModel>();

            InvokeRepeating("CheckInventory", 0.5f, 0.5f);
        }

        private void CheckInventory()
        {
            if (this.characterBody && this.characterBody.master)
            {
                if (this.characterBody.master.inventory)
                {
                    if (this.characterBody.master.inventory.GetItemCount(RoR2Content.Items.LunarPrimaryReplacement) > 0)
                    {
                        this.characterBody.hideCrosshair = false;
                    }
                }
            }
        }
    }
}
