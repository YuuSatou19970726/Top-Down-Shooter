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
        [SerializeField] private Transform leftHand;

        [Header("Rig")]
        [SerializeField] private float rigIncreaseStep;
        private bool rigShouldBeIncreased;

        private Rig rig;

        protected override void Start()
        {
            this.SwitchOffGuns();
            this.SwitchOnGun(this.pistol);
        }

        protected override void Update()
        {
            this.CheckWeaponSwitch();
            this.CheckWeaponReload();
            this.CheckEndReload();
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
            this.leftHand.localPosition = targetTransform.localPosition;
            this.leftHand.localRotation = targetTransform.localRotation;
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
            }

            if (InputManager.Instance.isAlpha2)
            {
                SwitchOnGun(this.revolver);
                this.SwitchAnimationLayer(1);
            }

            if (InputManager.Instance.isAlpha3)
            {
                SwitchOnGun(this.autoFire);
                this.SwitchAnimationLayer(1);
            }

            if (InputManager.Instance.isAlpha4)
            {
                SwitchOnGun(this.shotgun);
                this.SwitchAnimationLayer(2);
            }

            if (InputManager.Instance.isAlpha5)
            {
                SwitchOnGun(this.rifle);
                this.SwitchAnimationLayer(3);
            }
        }

        private void CheckWeaponReload()
        {
            if (!InputManager.Instance.isKeyR) return;

            this.anim.SetTrigger(AnimationTags.TRIGGER_RELOAD);
            this.rig.weight = .15f;
        }

        public void ReturnRigWeigthToOne() => this.rigShouldBeIncreased = true;

        private void CheckEndReload()
        {
            if (!this.rigShouldBeIncreased) return;

            this.rig.weight += this.rigIncreaseStep * Time.deltaTime;

            if (this.rig.weight >= 1)
                this.rigShouldBeIncreased = false;
        }
    }
}