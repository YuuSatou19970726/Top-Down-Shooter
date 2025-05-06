using UnityEngine;

namespace TopDownShooter
{
    public class ItemPickup : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;

        void OnTriggerEnter(Collider other)
        {
            other.GetComponent<PlayerWeaponController>()?.PickupWeapon(weapon);
            Destroy(gameObject);
        }
    }
}