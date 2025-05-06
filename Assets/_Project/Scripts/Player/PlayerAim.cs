using UnityEngine;
namespace TopDownShooter
{
    public class PlayerAim : CustomMonoBehaviour
    {
        protected Player player;
        private PlayerController controls;

        [Header("Aim Visual - Laser")]
        [SerializeField] private LineRenderer aimLaser;

        [Header("Aim control")]
        [SerializeField] private Transform aim;
        [SerializeField] private bool isAimingPrecisely;
        [SerializeField] private bool isLockingToTarget;

        [Header("Camera Control")]
        [SerializeField] private Transform cameraTarget;
        [Range(.5f, 1.5f)]
        [SerializeField] private float minCameraDistance = 1.5f;
        [Range(1f, 4f)]
        [SerializeField] private float maxCameraDistance = 4f;
        [Range(3f, 5f)]
        [SerializeField] private float cameraSensetivity = 5f;

        [Space]
        [SerializeField] private LayerMask aimPlayerMask;

        private Vector2 mouseInput;
        private RaycastHit lastKnownMouseHit = default;

        protected override void Start()
        {
            this.AssignInputEvents();
        }

        protected override void Update()
        {
            if (InputManager.Instance.isKeyP) this.isAimingPrecisely = !this.isAimingPrecisely;
            if (InputManager.Instance.isKeyL) this.isLockingToTarget = !this.isLockingToTarget;

            this.UpdateAimVisuals();
            this.UpdateAimPosition();
            this.UpdateCameraPosition();
        }

        private void UpdateAimPosition()
        {
            Transform target = this.Target();
            if (target != null && this.isLockingToTarget)
            {
                if (target.GetComponent<Renderer>() != null)
                    this.aim.position = target.GetComponent<Renderer>().bounds.center;
                else
                    this.aim.position = target.position;
                return;
            }

            this.aim.position = this.GetMouseHitInfo().point;

            if (!this.isAimingPrecisely)
                this.aim.position = new Vector3(this.aim.position.x, transform.position.y + 1, this.aim.position.z);
        }

        private void UpdateAimVisuals()
        {
            this.aimLaser.enabled = this.player.weapon.WeaponReady();

            if (this.aimLaser.enabled == false) return;

            WeaponModel weaponModel = this.player.weaponVisuals.CurrentWeaponModel();
            weaponModel.transform.LookAt(this.aim);
            weaponModel.gunPoint.LookAt(this.aim);

            Transform gunPoint = this.player.weapon.GunPoint();
            Vector3 laserDirection = this.player.weapon.BulletDirection();
            float laserTipLenght = .5f;
            float gunDistance = this.player.weapon.CurrentWeapon().gunDistance;

            this.aimLaser.SetPosition(0, gunPoint.position);

            Vector3 endPoint = gunPoint.position + laserDirection * gunDistance;

            if (Physics.Raycast(gunPoint.position, laserDirection, out RaycastHit hit, gunDistance))
            {
                endPoint = hit.point;
                laserTipLenght = 0;
            }

            this.aimLaser.SetPosition(0, gunPoint.position);
            this.aimLaser.SetPosition(1, endPoint);
            this.aimLaser.SetPosition(2, endPoint + laserDirection * laserTipLenght);
        }

        protected override void LoadComponents()
        {
            this.LoadComponent();
        }

        private void LoadComponent()
        {
            if (this.player != null) return;
            this.player = GetComponent<Player>();
        }

        public RaycastHit GetMouseHitInfo()
        {
            RaycastHit? hit = InputManager.Instance.GetAimPlayer(this.aimPlayerMask, this.mouseInput);

            if (hit == null) return this.lastKnownMouseHit;

            this.lastKnownMouseHit = (RaycastHit)hit;
            return this.lastKnownMouseHit;
        }

        private void AssignInputEvents()
        {
            this.controls = this.player.controls;

            this.controls.Character.Aim.performed += context => this.mouseInput = context.ReadValue<Vector2>();
            this.controls.Character.Aim.canceled += context => this.mouseInput = Vector2.zero;
        }

        public Transform Aim() => this.aim;

        public bool CanAimPrecisly() => this.isAimingPrecisely;

        public Transform Target()
        {
            Transform target = null;

            if (this.GetMouseHitInfo().transform != null)
                if (this.GetMouseHitInfo().transform.GetComponent<ObstacleTarget>() != null)
                    target = GetMouseHitInfo().transform;


            return target;
        }

        #region Camera Region
        private void UpdateCameraPosition()
        {
            this.cameraTarget.position =
                Vector3.Lerp(cameraTarget.position, this.DesiredCameraPosition(), this.cameraSensetivity * Time.deltaTime);
        }

        private Vector3 DesiredCameraPosition()
        {
            float actualMaxCameraDistance;
            bool movingDownwards = this.player.movement.moveInput.y < -.5f;

            actualMaxCameraDistance = movingDownwards ? minCameraDistance : maxCameraDistance;

            Vector3 desiredCameraPosition = this.GetMouseHitInfo().point;
            Vector3 aimDirection = (desiredCameraPosition - transform.position).normalized;

            float distanceToDesiredPosition = Vector3.Distance(transform.position, desiredCameraPosition);
            float clampedDistance = Mathf.Clamp(distanceToDesiredPosition, this.minCameraDistance, actualMaxCameraDistance);

            desiredCameraPosition = transform.position + aimDirection * clampedDistance;
            desiredCameraPosition.y = transform.position.y + 1;

            return desiredCameraPosition;
        }
        #endregion
    }
}