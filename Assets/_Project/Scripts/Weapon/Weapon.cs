using UnityEngine;

namespace TopDownShooter
{
    [System.Serializable]
    public class Weapon
    {
        [Header("Magazine details")]
        public int bulletsInMagazine;
        public int magazineCapacity;
        public int totalReserveAmmo;

        #region Burst mode variables
        private bool burstAvalible;
        private bool burstActive;
        private int burstBulletsPerShot;
        private float burstFireRate;
        public float burstFireDelay { get; private set; }
        #endregion

        #region Spread variables
        private float baseSpread = 1f;
        private float maximumSpread = 3f;
        private float currentSpread = 1f;
        private float spreadIncreaseRate = .15f;
        private float lastSpreadUpdateTime;
        private float spreadCooldown = 1f;
        #endregion

        #region Generics info
        public WeaponType weaponType;
        public float reloadSpeed { get; private set; } // How fast character reloads weapon
        public float equipmentSpeed { get; private set; } // How fast character equips weapon
        public float gunDistance { get; private set; }
        public float cameraDistance { get; private set; }
        #endregion

        #region Regular mode variables 
        public ShootType shootType;
        public int bulletsPerShot { get; private set; }
        private float fireRate = 1f; // bullets per second
        private float defaultFireRate;
        private float lastShootTime;
        #endregion

        public Weapon(WeaponData data)
        {
            this.bulletsInMagazine = data.bulletsInMagazine;
            this.magazineCapacity = data.magazineCapacity;
            this.totalReserveAmmo = data.totalReserveAmmo;

            this.burstAvalible = data.burstAvalible;
            this.burstActive = data.burstActive;
            this.burstBulletsPerShot = data.burstBulletsPerShot;
            this.burstFireRate = data.burstFireRate;
            this.burstFireDelay = data.burstFireDelay;

            this.baseSpread = data.baseSpread;
            this.maximumSpread = data.maximumSpread;
            this.spreadIncreaseRate = data.spreadIncreaseRate;
            this.spreadCooldown = data.spreadCooldown;

            this.weaponType = data.weaponType;
            this.reloadSpeed = data.reloadSpeed;
            this.equipmentSpeed = data.equipmentSpeed;
            this.gunDistance = data.gunDistance;
            this.cameraDistance = data.cameraDistance;

            this.shootType = data.shootType;
            this.fireRate = data.fireRate;
            this.defaultFireRate = this.fireRate;
            this.bulletsPerShot = data.bulletsPerShot;
        }

        #region Shoot methods
        public bool CanShoot() => this.HaveEnoughBullets() && this.ReadyToFire();

        private bool HaveEnoughBullets() => this.bulletsInMagazine > 0;

        private bool ReadyToFire()
        {
            if (Time.time > this.lastShootTime + 1 / this.fireRate)
            {
                this.lastShootTime = Time.time;
                return true;
            }
            return false;
        }
        #endregion

        #region Reload methods
        public bool CanReload()
        {
            return this.HaveReloadBullets();
        }

        private bool HaveReloadBullets()
        {
            if (this.bulletsInMagazine == this.magazineCapacity) return false;

            if (this.totalReserveAmmo > 0)
                return true;

            return false;
        }

        public void RefillBullets()
        {
            // This will add bullets in magazine to total amount of bullets
            this.totalReserveAmmo += this.bulletsInMagazine;

            int bulletsToReload = this.magazineCapacity;

            if (bulletsToReload > this.totalReserveAmmo)
                bulletsToReload = this.totalReserveAmmo;

            this.totalReserveAmmo -= bulletsToReload;
            this.bulletsInMagazine = bulletsToReload;

            if (this.totalReserveAmmo < 0)
                this.totalReserveAmmo = 0;
        }
        #endregion

        #region Spread methods
        public Vector3 ApplySpread(Vector3 originalDirection)
        {
            this.UpdateSpread();

            float randomizedValue = Random.Range(-this.currentSpread, this.currentSpread);

            Quaternion spreadRotation = Quaternion.Euler(randomizedValue, randomizedValue, randomizedValue);

            return spreadRotation * originalDirection;
        }

        private void IncreaseSpread()
        {
            this.currentSpread = Mathf.Clamp(this.currentSpread + this.spreadIncreaseRate, this.baseSpread, maximumSpread);
        }

        private void UpdateSpread()
        {
            if (Time.time > this.lastSpreadUpdateTime + this.spreadCooldown)
                this.currentSpread = this.baseSpread;
            else
                this.IncreaseSpread();

            this.lastSpreadUpdateTime = Time.time;
        }
        #endregion

        #region Burst methods
        public bool BurstActivated()
        {
            if (weaponType == WeaponType.Shotgun)
            {
                this.burstFireDelay = 0;
                return true;
            }

            return this.burstActive;
        }

        public void ToggleBurst()
        {
            if (!this.burstAvalible) return;

            this.burstActive = !this.burstActive;

            if (this.burstActive)
            {
                this.bulletsPerShot = this.burstBulletsPerShot;
                this.fireRate = this.burstFireRate;
            }
            else
            {
                this.bulletsPerShot = 1;
                this.fireRate = this.defaultFireRate;
            }
        }
        #endregion
    }
}