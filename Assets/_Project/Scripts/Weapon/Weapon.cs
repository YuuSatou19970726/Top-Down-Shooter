using UnityEngine;

namespace TopDownShooter
{
    [System.Serializable]
    public class Weapon
    {
        public WeaponType weaponType;

        [Header("Magazine details")]
        public int bulletsInMagazine;
        public int magazineCapacity;
        public int totalReserveAmmo;

        // How fast character reloads weapon
        [Range(1, 3)]
        public float reloadSpeed = 1f;

        // How fast character equips weapon
        [Range(1, 3)]
        public float equipmentSpeed = 1f;
        [Range(2, 12)]
        public float gunDistance = 4f;
        [Range(3, 8)]
        public float cameraDistance = 6f;

        [Header("Shooting spesifics")]
        public ShootType shootType;
        public int bulletsPerShot;
        public float defaultFireRate;
        public float fireRate = 1f; // bullets per second
        private float lastShootTime;

        [Header("Spread")]
        public float baseSpread = 1f;
        public float maximumSpread = 3f;
        private float currentSpread = 1f;
        public float spreadIncreaseRate = .15f;
        private float lastSpreadUpdateTime;
        private float spreadCooldown = 1f;

        [Header("Burst fire")]
        public bool burstAvalible;
        public bool burstActive;
        public int burstBulletsPerShot;
        public float burstFireRate;
        public float burstFireDelay = .1f;

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