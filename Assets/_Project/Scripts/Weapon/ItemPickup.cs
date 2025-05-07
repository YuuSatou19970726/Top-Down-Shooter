using UnityEngine;

namespace TopDownShooter
{
    public class ItemPickup : MonoBehaviour
    {
        [SerializeField] private WeaponData weaponData;

        void OnTriggerEnter(Collider other)
        {
            other.GetComponent<PlayerWeaponController>()?.PickupWeapon(weaponData);
            Destroy(gameObject);
        }
    }
}