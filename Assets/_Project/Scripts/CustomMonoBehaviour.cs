using UnityEngine;

namespace TopDownShooter
{
    public class CustomMonoBehaviour : MonoBehaviour
    {
        protected virtual void Awake()
        {
            this.LoadComponents();
        }

        protected virtual void OnEnable()
        {

        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {

        }

        protected virtual void FixedUpdate()
        {

        }

        protected virtual void OnDisable()
        {

        }

        protected virtual void Reset()
        {
            this.LoadComponents();
        }

        protected virtual void LoadComponents()
        {

        }
    }
}