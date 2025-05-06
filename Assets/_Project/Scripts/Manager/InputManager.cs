using UnityEngine;

namespace TopDownShooter
{
    public class InputManager : MonoBehaviour
    {
        private static InputManager instance;
        public static InputManager Instance => instance;

        [HideInInspector]
        public float verticalInput;
        [HideInInspector]
        public float horizontalInput;
        [HideInInspector]
        public bool isLeftClick;
        [HideInInspector]
        public bool isAlpha1, isAlpha2, isAlpha3, isAlpha4, isAlpha5, isKeyR, isKeyK, isKeyP, isKeyL, isKeyT;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        private void Update()
        {
            this.AssignInputEvents();
            this.AssignClickEvent();
        }

        private void AssignInputEvents()
        {
            this.verticalInput = Input.GetAxis(AxisTags.VERTICAL);
            this.horizontalInput = Input.GetAxis(AxisTags.HORIZONTAL);

            this.isAlpha1 = Input.GetKeyDown(KeyCode.Alpha1);
            this.isAlpha2 = Input.GetKeyDown(KeyCode.Alpha2);
            this.isAlpha3 = Input.GetKeyDown(KeyCode.Alpha3);
            this.isAlpha4 = Input.GetKeyDown(KeyCode.Alpha4);
            this.isAlpha5 = Input.GetKeyDown(KeyCode.Alpha5);
            this.isKeyR = Input.GetKeyDown(KeyCode.R);
            this.isKeyK = Input.GetKeyDown(KeyCode.K);
            this.isKeyP = Input.GetKeyDown(KeyCode.P);
            this.isKeyL = Input.GetKeyDown(KeyCode.L);
            this.isKeyT = Input.GetKeyDown(KeyCode.T);
        }

        private void AssignClickEvent()
        {
            this.isLeftClick = Input.GetKeyDown(KeyCode.Mouse0);
        }

        public RaycastHit? GetAimTank(LayerMask layerMask)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                return hit;

            return default;
        }

        public RaycastHit? GetAimPlayer(LayerMask layerMask, Vector2 aimInput)
        {
            Ray ray = Camera.main.ScreenPointToRay(aimInput);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layerMask))
            {
                return hit;
            }
            return default;
        }
    }
}