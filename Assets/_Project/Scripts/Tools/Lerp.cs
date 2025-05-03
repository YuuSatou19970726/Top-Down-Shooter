using UnityEngine;

namespace TopDownShooter
{
    public class Lerp : MonoBehaviour
    {
        // Lerp Linear Interpolation

        public float value;
        public float maxValue;

        [Range(0, 1)]
        public float step;

        void Update()
        {
            this.value = Mathf.Lerp(this.value, maxValue, step * Time.deltaTime);
        }
    }
}