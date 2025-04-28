using TopDownShooter;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace TopDownShooter
{
    public class WeaponVisualController : CustomMonoBehaviour
    {
        private Animator anim;

        [SerializeField] private Transform[] gunTransforms;

        [SerializeField] private Transform pistol;
        [SerializeField] private Transform revolver;
        [SerializeField] private Transform autoFire;
        [SerializeField] private Transform shotgun;
        [SerializeField] private Transform rifle;

        private Transform currentGun;

        [Header("Left hand IK")]
        [SerializeField] private TwoBoneIKConstraint leftHandIK;
        [SerializeField] private Transform leftHandIK_Target;
        [SerializeField] private float leftHandIK_IncreaseStep;
        private bool shouldIncreaseLeftHandIKWeight;

        [Header("Rig")]
        [SerializeField] private float rigIncreaseStep;
        private bool rigShouldBeIncreased;

        private Rig rig;

        private bool busyGrabbingWeapon;

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
            if (!InputManager.Instance.isKeyR || this.busyGrabbingWeapon) return;

            this.anim.SetTrigger(AnimationTags.TRIGGER_RELOAD);
            this.PauseRig();
        }

        private void PauseRig()
        {
            this.rig.weight = .15f;
        }

        public void ReturnRigWeigthToOne() => this.rigShouldBeIncreased = true;
        public void ReturnWeigthToLeftHandIK() => this.shouldIncreaseLeftHandIKWeight = true;

        private void UpdateRigWigth()
        {
            if (!this.rigShouldBeIncreased) return;

            this.rig.weight += this.rigIncreaseStep * Time.deltaTime;

            if (this.rig.weight >= 1)
                this.rigShouldBeIncreased = false;
        }

        private void PlayWeaponGrabAnimation(GrabType grabType)
        {
            this.leftHandIK.weight = 0;
            this.PauseRig();
            this.anim.SetFloat(AnimationTags.FLOAT_WEAPON_GRAB_TYPE, (float)grabType);
            this.anim.SetTrigger(AnimationTags.TRIGGER_WEAPON_GRAB);

            this.SetBusyGrabbingWeaponTo(true);
        }

        public void SetBusyGrabbingWeaponTo(bool busy)
        {
            this.busyGrabbingWeapon = busy;
            this.anim.SetBool(AnimationTags.BOOL_BUSY_GRABBING_WEAPON, this.busyGrabbingWeapon);
        }

        private void UpdateLeftHandIKWeight()
        {
            if (!this.shouldIncreaseLeftHandIKWeight) return;

            this.leftHandIK.weight += leftHandIK_IncreaseStep * Time.deltaTime;

            if (this.leftHandIK.weight >= 1)
                this.shouldIncreaseLeftHandIKWeight = false;
        }
    }
}