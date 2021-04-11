using RoR2;
using UnityEngine;

namespace RegigigasMod.Modules.Components
{
    public class RegigigasGrabController : MonoBehaviour
    {
        public Transform pivotTransform;

        private Rigidbody rb;
        private CharacterBody body;
        private CharacterMotor motor;
        private CharacterDirection direction;
        private ModelLocator modelLocator;
        private Transform modelTransform;
        private Quaternion originalRotation;

        private void Awake()
        {
            this.rb = this.GetComponent<Rigidbody>();
            this.body = this.GetComponent<CharacterBody>();
            this.motor = this.GetComponent<CharacterMotor>();
            this.direction = this.GetComponent<CharacterDirection>();
            this.modelLocator = this.GetComponent<ModelLocator>();

            if (this.direction) this.direction.enabled = false;

            if (this.modelLocator)
            {
                if (this.modelLocator.modelTransform)
                {
                    this.modelTransform = modelLocator.modelTransform;
                    this.originalRotation = this.modelTransform.rotation;

                    this.modelLocator.enabled = false;
                }
            }

            if (this.motor)
            {
                this.gameObject.layer = LayerIndex.fakeActor.intVal;
                this.motor.Motor.RebuildCollidableLayers();
            }
        }

        private void FixedUpdate()
        {
            if (this.motor)
            {
                this.motor.disableAirControlUntilCollision = true;
                this.motor.velocity = Vector3.zero;
                this.motor.rootMotion = Vector3.zero;

                if (this.pivotTransform) this.motor.Motor.SetPosition(this.pivotTransform.position, true);
            }

            if (this.pivotTransform)
            {
                this.transform.position = this.pivotTransform.position;
                if (this.modelTransform)
                {
                    this.modelTransform.position = this.pivotTransform.position;
                    this.modelTransform.rotation = this.pivotTransform.rotation;
                }
            }
            else
            {
                this.Release();
            }
        }

        private void OnDestroy()
        {
            if (this.motor)
            {
                this.gameObject.layer = LayerIndex.fakeActor.intVal;
                this.motor.Motor.RebuildCollidableLayers();
            }

            this.Release();
        }

        public void Release()
        {
            if (this.modelLocator) this.modelLocator.enabled = true;
            if (this.modelTransform) this.modelTransform.rotation = this.originalRotation;
            if (this.direction) this.direction.enabled = true;

            Destroy(this);
        }

        public void Throw(Vector3 throwVector)
        {
            if (this.modelLocator) this.modelLocator.enabled = true;
            if (this.modelTransform) this.modelTransform.rotation = this.originalRotation;
            if (this.direction) this.direction.enabled = true;

            if (this.motor) this.motor.velocity = throwVector;
            if (!this.motor && this.rb) this.rb.velocity = throwVector;

            this.motor = null;
            this.pivotTransform = null;
            this.modelTransform = null;

            Destroy(this);
        }
    }
}