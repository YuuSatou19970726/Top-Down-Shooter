using System.Collections.Generic;
using UnityEngine;
namespace TopDownShooter
{
    public class PickupAmmo : Interactable
    {
        [SerializeField] private AmmoBoxType boxType;
        [SerializeField] private List<AmmoData> smallBoxAmmo;
        [SerializeField] private List<AmmoData> bigBoxAmmo;
        [SerializeField] private GameObject[] boxModel;

        private void Start()
        {
            SetupBoxModel();
        }

        private void SetupBoxModel()
        {
            for (int i = 0; i < this.boxModel.Length; i++)
            {
                this.boxModel[i].SetActive(false);

                if (i == (int)this.boxType)
                {
                    this.boxModel[i].SetActive(true);
                    this.UpdateMeshAndMaterial(this.boxModel[i].GetComponent<MeshRenderer>());
                }
            }
        }

        public override void Interaction()
        {
            List<AmmoData> currentAmmolist = smallBoxAmmo;

            if (this.boxType == AmmoBoxType.BigBox)
                currentAmmolist = bigBoxAmmo;

            foreach (AmmoData ammoData in currentAmmolist)
            {
                Weapon weapon = this.playerWeaponController.WeaponInSlots(ammoData.weaponType);
                this.AddBulletsToWeapon(weapon, this.GetBulletAmount(ammoData));
            }

            ObjectPool.Instance.ReturnObject(gameObject);
        }

        private void AddBulletsToWeapon(Weapon weapon, int amount)
        {
            if (weapon == null) return;
            weapon.totalReserveAmmo += amount;
        }

        private int GetBulletAmount(AmmoData ammoData)
        {
            float min = Mathf.Min(ammoData.minAmount, ammoData.maxAmount);
            float max = Mathf.Max(ammoData.minAmount, ammoData.maxAmount);
            float randomAmmoAmount = Random.Range(min, max);

            return Mathf.RoundToInt(randomAmmoAmount);
        }
    }
}