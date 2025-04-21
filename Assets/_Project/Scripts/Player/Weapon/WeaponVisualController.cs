using TopDownShooter;
using UnityEngine;

namespace TopDownShooter
{
    public class WeaponVisualController : MonoBehaviour
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

        private void Start()
        {
            this.SwitchOffGuns();
            this.SwitchOnGun(this.pistol);

            this.anim = GetComponentInChildren<Animator>();
        }

        private void Update()
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
    }
}