using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownShooter
{
    public class PlayerWeaponController : CustomMonoBehaviour
    {
        private const float REFERENCE_BULLET_SPEED = 20f;
        // This is the default speed from which our mass fomular is derived.

        protected Player player;

        [SerializeField] private WeaponData defaultWeaponData;
        [SerializeField] private Weapon currentWeapon;
        private bool weaponReady;
        private bool isShooting;

        [Header("Bullet details")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float bulletSpeed;

        [SerializeField] private Transform weaponHolder;

        [Header("Inventory")]
        [SerializeField] private int maxSlots = 2;
        [SerializeField] private List<Weapon> weaponSlots;
        [SerializeField] private GameObject weaponPickupPrefab;

        public Transform GunPoint() => this.player.weaponVisuals.CurrentWeaponModel().gunPoint;
        public Weapon CurrentWeapon() => this.currentWeapon;
        public bool HasOnlyOneWeapon() => this.weaponSlots.Count <= 1;

        protected override void Start()
        {
            AssignInputEvents();

            Invoke(nameof(this.EquipStartingWeapon), .1f);
        }

        protected override void Update()
        {
            if (this.isShooting)
                this.Shoot();
        }

        protected override void LoadComponents()
        {
            this.LoadComponent();
        }

        private void LoadComponent()
        {
            if (this.player != null) return;
            this.player = GetComponent<Player>();
        }

        private void Shoot()
        {
            if (!this.WeaponReady()) return;
            if (!this.currentWeapon.CanShoot()) return;
            if (this.currentWeapon.shootType == ShootType.Single)
                this.isShooting = false;

            this.player.weaponVisuals.PlayerFireAnimation();

            if (this.currentWeapon.BurstActivated())
            {
                StartCoroutine(this.BurstFire());
                return;
            }

            this.FireSingleBullet();
        }

        private void FireSingleBullet()
        {
            this.currentWeapon.bulletsInMagazine--;

            GameObject newBullet = ObjectPool.Instance.GetObject(bulletPrefab);
            if (newBullet == null) return;

            // Instantiate(this.bulletPrefab, this.gunPoint.position, Quaternion.LookRotation(this.gunPoint.forward));
            newBullet.transform.position = this.GunPoint().position;
            newBullet.transform.rotation = Quaternion.LookRotation(this.GunPoint().forward);

            Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();
            Bullet bulletScript = newBullet.GetComponent<Bullet>();
            bulletScript.BulletSetup(this.currentWeapon.gunDistance);
            Vector3 bulletDirection = this.currentWeapon.ApplySpread(this.BulletDirection());

            rbNewBullet.mass = REFERENCE_BULLET_SPEED / this.bulletSpeed;
            rbNewBullet.linearVelocity = bulletDirection * this.bulletSpeed;
        }

        private void ReloadWeapon()
        {
            this.SetWeaponReady(false);
            this.player.weaponVisuals.PlayReloadAnimation();
        }

        public Vector3 BulletDirection()
        {
            Transform aim = this.player.aim.Aim();

            Vector3 direction = (aim.position - this.GunPoint().position).normalized;

            if (this.player.aim.CanAimPrecisly() == false && this.player.aim.Target() == null)
                direction.y = 0;

            return direction;
        }

        private void EquipStartingWeapon()
        {
            weaponSlots[0] = new Weapon(this.defaultWeaponData);
            this.EquipWeapon(0);
        }

        private IEnumerator BurstFire()
        {
            this.SetWeaponReady(false);

            for (int i = 1; i <= this.currentWeapon.bulletsPerShot; i++)
            {
                this.FireSingleBullet();

                yield return new WaitForSeconds(this.currentWeapon.burstFireDelay);

                if (i >= this.currentWeapon.bulletsPerShot)
                    SetWeaponReady(true);
            }
        }

        public Weapon WeaponInSlots(WeaponType weaponType)
        {
            foreach (Weapon weapon in this.weaponSlots)
            {
                if (weapon.weaponType == weaponType)
                    return weapon;
            }

            return null;
        }

        #region Slots Management - Pickup\Equip\Drop\Ready Weapon
        public void SetWeaponReady(bool ready) => this.weaponReady = ready;
        public bool WeaponReady() => this.weaponReady;

        private void EquipWeapon(int index)
        {
            if (index >= this.weaponSlots.Count) return;

            this.SetWeaponReady(false);
            this.currentWeapon = this.weaponSlots[index];

            this.player.weaponVisuals.PlayWeaponEquipAnimation();

            CameraManager.Instance.ChangeCameraDistance(this.currentWeapon.cameraDistance);
        }

        private void DropWeapon()
        {
            if (this.HasOnlyOneWeapon()) return;
            CreateWeaponOnTheGround();

            this.weaponSlots.Remove(this.currentWeapon);

            this.EquipWeapon(0);
        }

        private void CreateWeaponOnTheGround()
        {
            GameObject droppedWeapon = ObjectPool.Instance.GetObject(this.weaponPickupPrefab);
            droppedWeapon.GetComponent<PickupWeapon>()?.SetupPickupWeapon(this.currentWeapon, transform);
        }

        public void PickupWeapon(Weapon newWeapon)
        {
            if (this.WeaponInSlots(newWeapon.weaponType) != null)
            {
                this.WeaponInSlots(newWeapon.weaponType).totalReserveAmmo += newWeapon.bulletsInMagazine;
                return;
            }

            if (this.weaponSlots.Count >= this.maxSlots && newWeapon.weaponType != currentWeapon.weaponType)
            {
                int weaponIndex = weaponSlots.IndexOf(currentWeapon);

                this.player.weaponVisuals.SwitchOffWeaponModels();
                weaponSlots[weaponIndex] = newWeapon;

                this.CreateWeaponOnTheGround();
                this.EquipWeapon(weaponIndex);
                return;
            }

            this.weaponSlots.Add(newWeapon);

            this.player.weaponVisuals.SwitchOnBackupWeaponModel();
        }
        #endregion

        #region Input Events
        private void AssignInputEvents()
        {
            PlayerController controls = this.player.controls;

            controls.Character.Fire.performed += context => this.isShooting = true;
            controls.Character.Fire.canceled += context => this.isShooting = false;
            controls.Character.EquipSlot1.performed += context => this.EquipWeapon(0);
            controls.Character.EquipSlot2.performed += context => this.EquipWeapon(1);
            controls.Character.EquipSlot3.performed += context => this.EquipWeapon(2);
            controls.Character.EquipSlot4.performed += context => this.EquipWeapon(3);
            controls.Character.EquipSlot5.performed += context => this.EquipWeapon(4);
            controls.Character.DropCurrentWeapon.performed += context => this.DropWeapon();
            controls.Character.Reload.performed += context =>
            {
                if (this.currentWeapon.CanReload() && this.WeaponReady())
                {
                    ReloadWeapon();
                }
            };
            controls.Character.ToogleWeaponMode.performed += context => this.currentWeapon.ToggleBurst();
        }
        #endregion

        // void OnDrawGizmos()
        // {
        //     Gizmos.DrawLine(this.weaponHolder.position, this.weaponHolder.position + this.weaponHolder.forward * 25);
        //     Gizmos.color = Color.yellow;

        //     if (this.player != null)
        //         Gizmos.DrawLine(this.gunPoint.position, this.gunPoint.position + this.BulletDirection() * 25);
        // }
    }
}