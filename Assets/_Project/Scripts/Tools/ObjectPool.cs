using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TopDownShooter
{
    public class ObjectPool : MonoBehaviour
    {
        private static ObjectPool instance;
        public static ObjectPool Instance => instance;

        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private int poolSize = 10;

        private Queue<GameObject> bulletPool = new Queue<GameObject>();

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            this.CreateInitialPool();
        }

        private void CreateInitialPool()
        {
            for (int i = 0; i < this.poolSize; i++)
            {
                CreateNewBullet();
            }
        }

        private void CreateNewBullet()
        {
            GameObject newBullet = Instantiate(this.bulletPrefab, transform);
            newBullet.SetActive(false);
            this.bulletPool.Enqueue(newBullet);
        }

        public GameObject GetBullet()
        {
            if (this.bulletPool.Count == 0)
                this.CreateNewBullet();

            GameObject bulletToGet = this.bulletPool.Dequeue();
            bulletToGet.SetActive(true);
            bulletToGet.transform.parent = null;

            return bulletToGet;
        }

        public void ReturnBullet(GameObject bullet)
        {
            bullet.SetActive(false);
            this.bulletPool.Enqueue(bullet);
            bullet.transform.parent = transform;
        }
    }
}