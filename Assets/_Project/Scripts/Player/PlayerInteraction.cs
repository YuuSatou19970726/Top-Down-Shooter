using System.Collections.Generic;
using UnityEngine;
namespace TopDownShooter
{
    public class PlayerInteraction : MonoBehaviour
    {
        private List<Interactable> interactables = new List<Interactable>();

        private Interactable closestInteractable;

        public List<Interactable> GetInteractables() => this.interactables;

        private void Start()
        {
            Player player = GetComponent<Player>();
            player.controls.Character.Interaction.performed += context => this.InteractWithClosest();
        }

        private void InteractWithClosest()
        {
            this.closestInteractable?.Interaction();
            this.interactables.Remove(closestInteractable);
            this.UpdateClosestInteractable();
        }

        public void UpdateClosestInteractable()
        {
            this.closestInteractable?.HighlightActive(false);

            this.closestInteractable = null;

            float closestDistance = float.MaxValue;

            foreach (Interactable interactable in this.interactables)
            {
                float distance = Vector3.Distance(transform.position, interactable.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    this.closestInteractable = interactable;
                }
            }

            this.closestInteractable?.HighlightActive(true);
        }
    }
}