using RoR2;
using UnityEngine;

namespace RegigigasMod.Modules.Components
{
    public class RegigigasFlashController : MonoBehaviour
    {
        private float maxEmission = 5f;
        private float currentEmission;
        public float emissionSmoothSpeed = 25f;
        private CharacterBody body;
        private CharacterModel model;
        private Material bodyMat;

        private enum FlashState
        {
            None,
            Up,
            Down
        };

        private FlashState currentState;

        private void Awake()
        {
            this.body = this.GetComponent<CharacterBody>();
            this.model = this.GetComponentInChildren<CharacterModel>();// baseRendererInfos[0].defaultMaterial;
            this.currentState = FlashState.None;

            Invoke("GetMaterial", 0.2f);
        }

        private void GetMaterial()
        {
            this.bodyMat = this.model.GetComponent<ModelSkinController>().skins[this.body.skinIndex].rendererInfos[1].defaultMaterial;
        }

        private void FixedUpdate()
        {
            float smoothAmount = Time.fixedDeltaTime * this.emissionSmoothSpeed;

            switch (this.currentState)
            {
                case FlashState.None:
                    this.currentEmission = 0f;
                    break;
                case FlashState.Down:
                    this.currentEmission -= smoothAmount;
                    if (this.currentEmission <= 0f)
                    {
                        this.currentEmission = 0f;
                        this.currentState = FlashState.None;
                    }
                    break;
                case FlashState.Up:
                    this.currentEmission += smoothAmount;
                    if (this.currentEmission >= this.maxEmission)
                    {
                        this.currentEmission = this.maxEmission;
                        this.currentState = FlashState.Down;
                    }
                    break;
            }

            if (this.bodyMat)
            {
                this.bodyMat.SetFloat("_EmPower", this.currentEmission);
            }
        }

        public void Flash()
        {
            Util.PlaySound("RegigigasFlash", this.gameObject);
            this.currentState = FlashState.Up;
        }
    }
}