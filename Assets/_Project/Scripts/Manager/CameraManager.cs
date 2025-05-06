using Unity.Cinemachine;
using UnityEngine;
namespace TopDownShooter
{
    public class CameraManager : MonoBehaviour
    {
        private static CameraManager instance;
        public static CameraManager Instance => instance;

        private CinemachineFollow cinemachineFollow;

        [Header("Camera distance")]
        [SerializeField] private bool canChangeCameraDistance;
        [SerializeField] private float distanceChangeRate;
        private float targetCameraDistance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);

            this.cinemachineFollow = GetComponentInChildren<CinemachineFollow>();
        }

        private void Update()
        {
            this.UpdateCameraDistance();
        }

        private void UpdateCameraDistance()
        {
            if (!this.canChangeCameraDistance) return;

            float currentDistance = this.cinemachineFollow.FollowOffset.y;

            if (Mathf.Abs(this.targetCameraDistance - currentDistance) < .01f) return;

            this.cinemachineFollow.FollowOffset.y =
                            Mathf.Lerp(this.cinemachineFollow.FollowOffset.y, this.targetCameraDistance, this.distanceChangeRate * Time.deltaTime);
        }

        public void ChangeCameraDistance(float distance) => this.targetCameraDistance = distance;
    }
}