using UnityEngine;

namespace TopDownShooter
{
    public class PlayerWeaponController : CustomMonoBehaviour
    {
        protected Player player;

        protected override void LoadComponents()
        {
            if (this.player != null) return;
            this.player = GetComponent<Player>();
        }

        protected override void Start()
        {
            this.player.controls.Character.Fire.performed += context => this.Shoot();
        }

        private void Shoot()
        {
            GetComponentInChildren<Animator>().SetTrigger(AnimationTags.TRIGGER_FIRE);
        }
    }
}