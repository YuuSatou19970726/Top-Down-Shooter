using UnityEngine;

namespace TopDownShooter
{
    public class PickupWeapon : Interactable
    {
        [SerializeField] private WeaponData weaponData;
        [SerializeField] private Weapon weapon;

        [SerializeField] private BackupWeaponModel[] models;

        private bool oldWeapon = false;

        private void Start()
        {
            if (!oldWeapon)
                this.weapon = new Weapon(this.weaponData);

            this.SetupGameObject();
        }

        public override void Interaction()
        {
            if (this.playerWeaponController == null) return;

            this.playerWeaponController.PickupWeapon(weapon);
            ObjectPool.Instance.ReturnObject(gameObject);
        }

        private void SetupWeaponModel()
        {
            foreach (BackupWeaponModel model in this.models)
            {
                model.gameObject.SetActive(false);

                if (model.weaponType == weaponData.weaponType)
                {
                    model.gameObject.SetActive(true);
                    this.UpdateMeshAndMaterial(model.GetComponent<MeshRenderer>());
                }
            }
        }

        [ContextMenu("Update Item Model")]
        public void SetupGameObject()
        {
            if (weaponData == null) return;
            gameObject.name = "Pickup_Weapon - " + weaponData.weaponType.ToString();
            this.SetupWeaponModel();
        }

        public void SetupPickupWeapon(Weapon weapon, Transform transform)
        {
            this.oldWeapon = true;

            this.weapon = weapon;
            this.weaponData = this.weapon.weaponData;

            this.transform.position = transform.position;
            this.transform.position = new Vector3(this.transform.position.x, 0.01f, this.transform.position.z);
        }
    }
}