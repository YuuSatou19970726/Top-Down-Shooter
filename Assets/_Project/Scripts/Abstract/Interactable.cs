using UnityEngine;
namespace TopDownShooter
{
    public class Interactable : MonoBehaviour
    {
        protected PlayerWeaponController playerWeaponController;

        protected MeshRenderer meshRenderer;
        [SerializeField] private Material highlightMaterial;
        protected Material defaultMaterial;

        private void Start()
        {
            if (this.meshRenderer == null)
                this.meshRenderer = GetComponentInChildren<MeshRenderer>();

            this.defaultMaterial = this.meshRenderer.sharedMaterial;
        }

        public virtual void Interaction() { }

        public void HighlightActive(bool active)
        {
            if (active)
                this.meshRenderer.material = this.highlightMaterial;
            else
                this.meshRenderer.material = this.defaultMaterial;
        }

        protected void UpdateMeshAndMaterial(MeshRenderer newMesh)
        {
            this.meshRenderer = newMesh;
            this.defaultMaterial = newMesh.sharedMaterial;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (this.playerWeaponController == null)
                this.playerWeaponController = other.GetComponent<PlayerWeaponController>();

            PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();

            if (playerInteraction == null) return;

            playerInteraction.GetInteractables().Add(this);
            playerInteraction.UpdateClosestInteractable();
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();

            if (playerInteraction == null) return;

            playerInteraction.GetInteractables().Remove(this);
            playerInteraction.UpdateClosestInteractable();
        }
    }
}