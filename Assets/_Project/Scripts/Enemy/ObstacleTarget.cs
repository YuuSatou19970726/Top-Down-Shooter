using UnityEngine;

namespace TopDownShooter
{
    [RequireComponent(typeof(Rigidbody))]
    public class ObstacleTarget : MonoBehaviour
    {
        private void Start()
        {
            gameObject.layer = LayerMask.NameToLayer(LayerTags.ENEMY);
        }
    }
}