using UnityEngine;
using RoR2;

namespace RegigigasMod.Modules.Components
{
    public class RegiSkinPicker : MonoBehaviour
    {
        private ModelSkinController skin;
        private CharacterModel characterModel;
        private string sceneName;

        private void Awake()
        {
            this.skin = this.GetComponent<ModelSkinController>();
            this.characterModel = this.GetComponent<CharacterModel>();
            this.sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }

        private void Update()
        {
            this.Init();
        }

        private void Init()
        {
            // i forget how the game does this so i'm just writing my own logic. sorry
            // this doesn't need to be done in update. but i'm tired of fighting with this
            // someone please optimize this cause i'm done here
            if (this.skin)
            {
                if (Modules.Config.loreFriendly)
                {
                    if (this.characterModel && !this.characterModel.body.isPlayerControlled)
                    {
                        for (int i = 0; i < this.skin.skins.Length; i++)
                        {
                            if (this.sceneName == "goldshores") // gonna need update for sots stages?
                            {
                                if (this.skin.skins[i] && this.skin.skins[i].nameToken == "JESTANANIMATOR_SKIN_GOLDREGIGIGAS_NAME")
                                {
                                    this.characterModel.baseRendererInfos[0].renderer.GetComponent<SkinnedMeshRenderer>().sharedMesh = this.skin.skins[i].meshReplacements[0].mesh;
                                    this.characterModel.baseRendererInfos[0].defaultMaterial = this.skin.skins[i].rendererInfos[0].defaultMaterial;
                                    Destroy(this.skin);
                                    Destroy(this);
                                }
                            }
                            else if (this.sceneName == "moon" || this.sceneName == "moon2")
                            {
                                if (this.skin.skins[i] && this.skin.skins[i].nameToken == "JESTANANIMATOR_SKIN_LUNARREGIGIGAS_NAME")
                                {
                                    RegigigasFlashController flash = this.characterModel.body.GetComponent<RegigigasFlashController>();
                                    if (flash)
                                    {
                                        flash.minEmission = 2f;
                                        flash.maxEmission = 6f;
                                    }

                                    this.characterModel.baseRendererInfos[0].renderer.GetComponent<SkinnedMeshRenderer>().sharedMesh = this.skin.skins[i].meshReplacements[0].mesh;
                                    this.characterModel.baseRendererInfos[0].defaultMaterial = this.skin.skins[i].rendererInfos[0].defaultMaterial;
                                    Destroy(this.skin);
                                    Destroy(this);

                                    // this sucks lol.
                                    // they look like an entirely new enemy but they don't get a log or anything, it's just bad
                                    this.characterModel.body.baseNameToken = "Lunar Chimera";
                                    this.characterModel.body.subtitleNameToken = "";
                                    this.characterModel.body.skillLocator.primary.SetSkillOverride(this.gameObject, Modules.Enemies.Regigigas.lunarPunchSkillDef, GenericSkill.SkillOverridePriority.Replacement);
                                    this.characterModel.body.skillLocator.secondary.SetSkillOverride(this.gameObject, Modules.Enemies.Regigigas.lunarStompSkillDef, GenericSkill.SkillOverridePriority.Replacement);
                                    this.characterModel.body.skillLocator.special.SetSkillOverride(this.gameObject, Modules.Enemies.Regigigas.lunarBounceSkillDef, GenericSkill.SkillOverridePriority.Replacement);
                                }
                            }
                            else
                            {
                                if (this.skin.skins[i] && this.skin.skins[i].nameToken == "JESTANANIMATOR_SKIN_STONEREGIGIGAS_NAME")
                                {
                                    this.characterModel.baseRendererInfos[0].renderer.GetComponent<SkinnedMeshRenderer>().sharedMesh = this.skin.skins[i].meshReplacements[0].mesh;
                                    this.characterModel.baseRendererInfos[0].defaultMaterial = this.skin.skins[i].rendererInfos[0].defaultMaterial;
                                    Destroy(this.skin);
                                    Destroy(this);
                                }
                            }
                        }
                    }
                    else Destroy(this);
                }
                else Destroy(this);
            }
        }
    }
}