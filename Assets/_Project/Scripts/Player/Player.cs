using UnityEngine;

namespace TopDownShooter
{
    public class Player : CustomMonoBehaviour
    {
        public PlayerController controls { get; private set; }
        public PlayerAim aim { get; private set; } // read-only

        protected override void Awake()
        {
            this.controls = new PlayerController();
            this.aim = GetComponent<PlayerAim>();
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