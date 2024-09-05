using RoR2;
using UnityEngine;

namespace RegigigasMod.Modules.Components
{
    public class RegigigasFlashController : MonoBehaviour
    {
        public float maxEmission = 5f;
        public float minEmission = 0f;
        private float currentEmission;
        private float lastEmission = 0;
        public float emissionSmoothSpeed = 25f;
        private ChildLocator childLocator;
        private Renderer bodyRend;
        private MaterialPropertyBlock bodyBlock;

        private enum FlashState
        {
            None,
            Up,
            Down
        };

        private FlashState currentState;

        private void Awake()
        {
            this.childLocator = this.GetComponent<ModelLocator>().modelTransform.GetComponent<ChildLocator>();// baseRendererInfos[0].defaultMaterial;
            this.currentState = FlashState.None;
        }

        private void GetRenderer() {

            this.bodyRend = this.childLocator.FindChildComponent<Renderer>("Model");
            this.bodyBlock = new MaterialPropertyBlock();
            this.bodyRend.GetPropertyBlock(this.bodyBlock);
        }

        private void FixedUpdate()
        {
            float smoothAmount = Time.fixedDeltaTime * this.emissionSmoothSpeed;

            switch (this.currentState)
            {
                case FlashState.None:
                    this.currentEmission = this.minEmission;
                    break;
                case FlashState.Down:
                    this.currentEmission -= smoothAmount;
                    if (this.currentEmission <= this.minEmission)
                    {
                        this.currentEmission = this.minEmission;
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
            if (this.lastEmission != this.currentEmission)
            {
                if (this.bodyRend)
                {
                    this.bodyBlock.SetFloat("_EmPower", this.currentEmission);
                    this.bodyRend.SetPropertyBlock(this.bodyBlock);
                }
                else
                {
                    this.GetRenderer();
                }
            }
            this.lastEmission = this.currentEmission;
        }

        public void Flash(bool playSound = true)
        {
            if (playSound) Util.PlaySound("RegigigasFlash", this.gameObject);
            this.currentState = FlashState.Up;
        }
    }
}