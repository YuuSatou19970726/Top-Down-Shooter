using UnityEngine;

public class TankFollow : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;

    private void Update()
    {
        transform.position =
        new Vector3(this.targetTransform.transform.position.x, transform.position.y, this.targetTransform.transform.position.z - 5);
    }
}
