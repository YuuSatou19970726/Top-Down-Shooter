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
        public bool isClick;
        [HideInInspector]
        public bool isAlpha1, isAlpha2, isAlpha3, isAlpha4, isAlpha5;

        private void Awake()
        {
            if (instance == null)
                instance = this;
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
        }

        private void AssignClickEvent()
        {
            this.isClick = Input.GetKeyDown(KeyCode.Mouse0);
        }

        public RaycastHit? GetAimTank(LayerMask layerMask)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                return hit;

            return null;
        }
    }
}