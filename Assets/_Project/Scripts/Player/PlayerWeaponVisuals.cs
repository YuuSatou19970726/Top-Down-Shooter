using TopDownShooter;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace TopDownShooter
{
    public class PlayerWeaponVisuals : CustomMonoBehaviour
    {
        private Animator anim;

        #region Gun transforms region
        [SerializeField] private Transform[] gunTransforms;

        [SerializeField] private Transform pistol;
        [SerializeField] private Transform revolver;
        [SerializeField] private Transform autoFire;
        [SerializeField] private Transform shotgun;
        [SerializeField] private Transform rifle;

        private Transform currentGun;
        #endregion

        [Header("Left hand IK")]
        [SerializeField] private float leftHandIKWeightIncreaseRate = 3f;
        [SerializeField] private TwoBoneIKConstraint leftHandIK;
        [SerializeField] private Transform leftHandIK_Target;
        private bool shouldIncrease_LeftHandIKWeight;

        [Header("Rig")]
        [SerializeField] private float rigWeightIncreaseRate = 2.75f;
        private bool shouldIncrease_RigWeight;
        private Rig rig;

        private bool isGrabbingWeapon;

        protected override void Start()
        {
            this.SwitchOffGuns();
            this.SwitchOnGun(this.pistol);
        }

        protected override void Update()
        {
            this.CheckWeaponSwitch();
            this.CheckWeaponReload();
            this.UpdateRigWigth();
            this.UpdateLeftHandIKWeight();
        }

        protected override void LoadComponents()
        {
            this.LoadComponent();
        }

        protected void LoadComponent()
        {
            this.anim = GetComponentInChildren<Animator>();
            this.rig = GetComponentInChildren<Rig>();
        }

        private void SwitchOnGun(Transform gunTransform)
        {
            this.SwitchOffGuns();
            gunTransform.gameObject.SetActive(true);
            this.currentGun = gunTransform;

            this.AttachLeftHand();
        }

        private void SwitchOffGuns()
        {
            for (int i = 0; i < this.gunTransforms.Length; i++)
                this.gunTransforms[i].gameObject.SetActive(false);
        }

        private void AttachLeftHand()
        {
            Transform targetTransform = this.currentGun.GetComponentInChildren<LeftHandTargetTransform>().transform;
            this.leftHandIK_Target.localPosition = targetTransform.localPosition;
            this.leftHandIK_Target.localRotation = targetTransform.localRotation;
        }

        private void SwitchAnimationLayer(int layerIndex)
        {
            for (int i = 1; i < anim.layerCount; i++)
            {
                this.anim.SetLayerWeight(i, 0);
            }

            this.anim.SetLayerWeight(layerIndex, 1);
        }

        private void CheckWeaponSwitch()
        {
            if (InputManager.Instance.isAlpha1)
            {
                this.SwitchOnGun(this.pistol);
                this.SwitchAnimationLayer(1);
                this.PlayWeaponGrabAnimation(GrabType.SideGrab);
            }

            if (InputManager.Instance.isAlpha2)
            {
                SwitchOnGun(this.revolver);
                this.SwitchAnimationLayer(1);
                this.PlayWeaponGrabAnimation(GrabType.SideGrab);
            }

            if (InputManager.Instance.isAlpha3)
            {
                SwitchOnGun(this.autoFire);
                this.SwitchAnimationLayer(1);
                this.PlayWeaponGrabAnimation(GrabType.BackGrab);
            }

            if (InputManager.Instance.isAlpha4)
            {
                SwitchOnGun(this.shotgun);
                this.SwitchAnimationLayer(2);
                this.PlayWeaponGrabAnimation(GrabType.BackGrab);
            }

            if (InputManager.Instance.isAlpha5)
            {
                SwitchOnGun(this.rifle);
                this.SwitchAnimationLayer(3);
                this.PlayWeaponGrabAnimation(GrabType.BackGrab);
            }
        }

        private void CheckWeaponReload()
        {
            if (!InputManager.Instance.isKeyR || this.isGrabbingWeapon) return;

            this.anim.SetTrigger(AnimationTags.TRIGGER_RELOAD);
            this.ReduceRigWeight();
        }

        private void ReduceRigWeight()
        {
            this.rig.weight = .15f;
        }

        public void MaximizeRigWeight() => this.shouldIncrease_RigWeight = true;
        public void MaximizeLeftHandWeight() => this.shouldIncrease_LeftHandIKWeight = true;

        private void UpdateRigWigth()
        {
            if (!this.shouldIncrease_RigWeight) return;

            this.rig.weight += this.rigWeightIncreaseRate * Time.deltaTime;

            if (this.rig.weight >= 1)
                this.shouldIncrease_RigWeight = false;
        }

        private void PlayWeaponGrabAnimation(GrabType grabType)
        {
            this.leftHandIK.weight = 0;
            this.ReduceRigWeight();
            this.anim.SetFloat(AnimationTags.FLOAT_WEAPON_GRAB_TYPE, (float)grabType);
            this.anim.SetTrigger(AnimationTags.TRIGGER_WEAPON_GRAB);

            this.SetBusyGrabbingWeaponTo(true);
        }

        public void SetBusyGrabbingWeaponTo(bool busy)
        {
            this.isGrabbingWeapon = busy;
            this.anim.SetBool(AnimationTags.BOOL_BUSY_GRABBING_WEAPON, this.isGrabbingWeapon);
        }

        private void UpdateLeftHandIKWeight()
        {
            if (!this.shouldIncrease_LeftHandIKWeight) return;

            this.leftHandIK.weight += leftHandIKWeightIncreaseRate * Time.deltaTime;

            if (this.leftHandIK.weight >= 1)
                this.shouldIncrease_LeftHandIKWeight = false;
        }
    }
}