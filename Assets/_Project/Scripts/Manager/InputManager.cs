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

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        private void Update()
        {
            this.AxisInput();
        }

        private void AxisInput()
        {
            this.verticalInput = Input.GetAxis(AxisTags.VERTICAL);
            this.horizontalInput = Input.GetAxis(AxisTags.HORIZONTAL);
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