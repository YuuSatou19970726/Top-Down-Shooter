using UnityEngine;

namespace TopDownShooter
{
    [System.Serializable]
    public struct AmmoData
    {
        public WeaponType weaponType;
        [Range(0, 100)] public int minAmount;
        [Range(0, 100)] public int maxAmount;
    }
}