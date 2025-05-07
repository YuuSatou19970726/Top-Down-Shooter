using UnityEngine;
namespace TopDownShooter
{
    [CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapon System/Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        public string weaponName;

        [Header("Magazine details")]
        public int bulletsInMagazine;
        public int magazineCapacity;
        public int totalReserveAmmo;

        [Header("Burst")]
        public bool burstAvalible;
        public bool burstActive;
        public int burstBulletsPerShot;
        public float burstFireRate;
        public float burstFireDelay = .1f;

        [Header("Spread")]
        public float baseSpread = 1f;
        public float maximumSpread = 3f;
        public float spreadIncreaseRate = .15f;
        public float spreadCooldown = 1f;

        [Header("Generics")]
        public WeaponType weaponType;
        [Range(1, 3)]
        public float reloadSpeed = 1f;
        [Range(1, 3)]
        public float equipmentSpeed = 1f;
        [Range(2, 12)]
        public float gunDistance = 4f;
        [Range(3, 8)]
        public float cameraDistance = 6f;

        [Header("Regular")]
        public ShootType shootType;
        public int bulletsPerShot;
        public float defaultFireRate;
        public float fireRate = 1f; // bullets per second
    }
}