using UnityEngine;
using RoR2;

namespace RegigigasMod.Modules.Components
{
    public class RegigigasCSS : MonoBehaviour
    {
        private void Awake()
        {
            Animator animator = this.GetComponent<Animator>();
            if (animator) animator.PlayInFixedTime("Spawn", animator.GetLayerIndex("Body"));

            Util.PlaySound("RegigigasSpawn", this.gameObject);
            this.Invoke("Cry", 5.2f);
        }

        private void Cry()
        {
            string soundString = "RegigigasCry";
            if (Modules.Config.loreFriendly) soundString = "sfx_regigigas_altcry";
            Util.PlaySound(soundString, this.gameObject);
        }
    }
}