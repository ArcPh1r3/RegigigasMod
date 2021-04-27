using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RegigigasMod.Modules.Components
{
    public class RegigigasShinyComponent : NetworkBehaviour
    {
        [SyncVar]
        private bool isShiny;

        private CharacterBody characterBody;
        private CharacterModel model;
        private ModelSkinController skinController;

        private void Awake()
        {
            this.characterBody = this.GetComponent<CharacterBody>();
            this.model = this.GetComponentInChildren<CharacterModel>();
            this.skinController = this.GetComponentInChildren<ModelSkinController>();
            this.isShiny = false;

            this.Invoke("ShinyRoll", 0.1f);
            this.Invoke("ApplyShiny", 0.2f);
        }

        private void ShinyRoll()
        {
            if (NetworkServer.active)
            {
                this.isShiny = Util.CheckRoll(Modules.Config.shinySpawnRate, this.characterBody.master);
            }
        }

        private void ApplyShiny()
        {
            if (this.isShiny)
            {
                if (this.skinController)
                {
                    this.characterBody.skinIndex = 1;
                    this.skinController.currentSkinIndex = 1;
                    this.skinController.ApplySkin(1);

                    this.model.baseRendererInfos[1].defaultMaterial = this.skinController.skins[1].rendererInfos[1].defaultMaterial;
                }

                this.characterBody.master.gameObject.AddComponent<RegigigasDropComponent>().itemDropDef = RoR2Content.Items.ShinyPearl;
            }
        }
    }
}