using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace TopDownShooter
{
    public class PlayerWeaponVisuals : CustomMonoBehaviour
    {
        private Player player;
        private Animator anim;

        [SerializeField] private WeaponModel[] weaponModels;
        [SerializeField] private BackupWeaponModel[] backupWeaponModels;

        [Header("Left hand IK")]
        [SerializeField] private float leftHandIKWeightIncreaseRate = 3f;
        [SerializeField] private TwoBoneIKConstraint leftHandIK;
        [SerializeField] private Transform leftHandIK_Target;
        private bool shouldIncrease_LeftHandIKWeight;

        [Header("Rig")]
        [SerializeField] private float rigWeightIncreaseRate = 2.75f;
        private bool shouldIncrease_RigWeight;
        private Rig rig;

        public void PlayerFireAnimation() => this.anim.SetTrigger(AnimationTags.TRIGGER_FIRE);

        protected override void Start()
        {
            this.SwitchOffWeaponModels();
        }

        protected override void Update()
        {
            this.UpdateRigWigth();
            this.UpdateLeftHandIKWeight();
        }

        protected override void LoadComponents()
        {
            this.LoadComponent();
        }

        protected void LoadComponent()
        {
            if (this.player != null && this.anim != null && this.rig != null && this.weaponModels.Length > 0) return;
            this.player = GetComponent<Player>();
            this.anim = GetComponentInChildren<Animator>();
            this.rig = GetComponentInChildren<Rig>();
            this.weaponModels = GetComponentsInChildren<WeaponModel>(true);
            this.backupWeaponModels = GetComponentsInChildren<BackupWeaponModel>(true);
        }

        public void SwitchOnWeaponModel()
        {
            int animationIndex = (int)this.CurrentWeaponModel().holdType;

            this.SwitchOffWeaponModels();
            this.SwitchOffBackupWeaponModels();

            if (!this.player.weapon.HasOnlyOneWeapon())
                this.SwitchOnBackupWeaponModel();

            this.SwitchAnimationLayer(animationIndex);

            this.CurrentWeaponModel().gameObject.SetActive(true);

            this.AttachLeftHand();
        }

        public void SwitchOffWeaponModels()
        {
            for (int i = 0; i < this.weaponModels.Length; i++)
                this.weaponModels[i].gameObject.SetActive(false);
        }

        private void SwitchAnimationLayer(int layerIndex)
        {
            for (int i = 1; i < anim.layerCount; i++)
            {
                this.anim.SetLayerWeight(i, 0);
            }

            this.anim.SetLayerWeight(layerIndex, 1);
        }

        public void PlayReloadAnimation()
        {
            float reloadSpeed = this.player.weapon.CurrentWeapon().reloadSpeed;

            this.anim.SetFloat(AnimationTags.FLOAT_RELOAD_SPEED, reloadSpeed);
            this.anim.SetTrigger(AnimationTags.TRIGGER_RELOAD);
            this.ReduceRigWeight();
        }

        public void PlayWeaponEquipAnimation()
        {
            EquipType equipType = this.CurrentWeaponModel().equipAnimationType;

            float equipmentAnimation = this.player.weapon.CurrentWeapon().equipmentSpeed;

            this.leftHandIK.weight = 0;
            this.ReduceRigWeight();
            this.anim.SetFloat(AnimationTags.FLOAT_EQUIP_TYPE, (float)equipType);
            this.anim.SetFloat(AnimationTags.FLOAT_EQUIP_SPEED, equipmentAnimation);
            this.anim.SetTrigger(AnimationTags.TRIGGER_EQUIP_WEAPON);
        }

        public WeaponModel CurrentWeaponModel()
        {
            WeaponModel weaponModel = null;

            WeaponType weaponType = this.player.weapon.CurrentWeapon().weaponType;

            for (int i = 0; i < this.weaponModels.Length; i++)
            {
                if (this.weaponModels[i].weaponType == weaponType)
                    weaponModel = this.weaponModels[i];
            }

            return weaponModel;
        }

        private void SwitchOffBackupWeaponModels()
        {
            for (int i = 0; i < this.backupWeaponModels.Length; i++)
                this.backupWeaponModels[i].Activate(false);
        }

        public void SwitchOnBackupWeaponModel()
        {
            this.SwitchOffBackupWeaponModels();

            BackupWeaponModel lowHangWeapon = null;
            BackupWeaponModel backHangWeapon = null;
            BackupWeaponModel sideHangWeapon = null;

            foreach (BackupWeaponModel backupWeaponModel in this.backupWeaponModels)
            {
                if (backupWeaponModel.weaponType == this.player.weapon.CurrentWeapon().weaponType)
                    continue;

                if (this.player.weapon.WeaponInSlots(backupWeaponModel.weaponType) != null)
                {
                    if (backupWeaponModel.HangTypeIs(HangType.LowBackHang))
                        lowHangWeapon = backupWeaponModel;

                    if (backupWeaponModel.HangTypeIs(HangType.BackHang))
                        backHangWeapon = backupWeaponModel;

                    if (backupWeaponModel.HangTypeIs(HangType.SideHang))
                        sideHangWeapon = backupWeaponModel;
                }
            }

            lowHangWeapon?.Activate(true);
            backHangWeapon?.Activate(true);
            sideHangWeapon?.Activate(true);
        }

        #region Animation Rigging Methods
        public void MaximizeRigWeight() => this.shouldIncrease_RigWeight = true;
        public void MaximizeLeftHandWeight() => this.shouldIncrease_LeftHandIKWeight = true;

        private void UpdateLeftHandIKWeight()
        {
            if (!this.shouldIncrease_LeftHandIKWeight) return;

            this.leftHandIK.weight += leftHandIKWeightIncreaseRate * Time.deltaTime;

            if (this.leftHandIK.weight >= 1)
                this.shouldIncrease_LeftHandIKWeight = false;
        }

        private void UpdateRigWigth()
        {
            if (!this.shouldIncrease_RigWeight) return;

            this.rig.weight += this.rigWeightIncreaseRate * Time.deltaTime;

            if (this.rig.weight >= 1)
                this.shouldIncrease_RigWeight = false;
        }

        private void ReduceRigWeight()
        {
            this.rig.weight = .15f;
        }

        private void AttachLeftHand()
        {
            Transform targetTransform = this.CurrentWeaponModel().holdPoint;
            this.leftHandIK_Target.localPosition = targetTransform.localPosition;
            this.leftHandIK_Target.localRotation = targetTransform.localRotation;
        }
        #endregion
    }
}