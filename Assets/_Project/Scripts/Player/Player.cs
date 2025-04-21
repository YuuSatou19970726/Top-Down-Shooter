using UnityEngine;

namespace TopDownShooter
{
    public class Player : CustomMonoBehaviour
    {
        public PlayerController controls;

        protected override void Awake()
        {
            this.controls = new PlayerController();
        }

        protected override void OnEnable()
        {
            this.controls.Enable();
        }

        protected override void OnDisable()
        {
            this.controls.Disable();
        }
    }
}