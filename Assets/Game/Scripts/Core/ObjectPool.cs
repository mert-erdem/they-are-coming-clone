using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class ObjectPool<T, T1> : Singleton<ObjectPool<T, T1>> where T : Component where T1 : Component
    {
        [SerializeField] private T poolObject;
        [SerializeField] private int poolSize;
        [SerializeField] private float objectLifetime = 1f;
        private List<T> pool;


        private void Start()
        {
            DontDestroyOnLoad(this.transform.parent);

            pool = new List<T>();

            for (int i = 0; i < poolSize; i++)
            {
                var objectForPool = Instantiate(poolObject, transform);
                objectForPool.gameObject.SetActive(false);
                pool.Add(objectForPool);
            }
        }

        public T GetObject()
        {
            for (int i = 0; i < poolSize; i++)
            {
                if (!pool[i].gameObject.activeInHierarchy)
                {
                    pool[i].gameObject.SetActive(true);
                    return pool[i];
                }
            }

            return null;
        }

        public List<T> GetObject(int count)
        {
            List<T> objects = new List<T>();

            for (int i = 0; i < poolSize; i++)
            {
                if (!pool[i].gameObject.activeInHierarchy && objects.Count < count)
                {
                    pool[i].gameObject.SetActive(true);
                    objects.Add(pool[i]);
                }
            }

            return objects;
        }

        public void PullObjectBack(T theObject)
        {
            if (!pool.Contains(theObject))
            {
                Destroy(theObject.gameObject);

                return;
            }

            StartCoroutine(PullObjectBackRoutine(theObject));
        }
        private IEnumerator PullObjectBackRoutine(T theObject)
        {
            yield return new WaitForSeconds(objectLifetime);

            theObject.transform.rotation = Quaternion.identity;
            theObject.transform.SetParent(this.transform);
            theObject.gameObject.SetActive(false);
        }

        public void PullObjectBackImmediate(T theObject)
        {
            if (!pool.Contains(theObject))
            {
                Destroy(theObject.gameObject);

                return;
            }

            theObject.transform.rotation = Quaternion.identity;
            theObject.transform.SetParent(this.transform);
            theObject.gameObject.SetActive(false);
        }
    }
}
