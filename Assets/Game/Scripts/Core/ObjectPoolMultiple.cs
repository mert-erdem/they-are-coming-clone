using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ObjectPool's extension for inheritance members of T.
/// Use this if you have multiple T type objects to spawn.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="T1"></typeparam>
namespace Game.Core
{
    public class ObjectPoolMultiple<T, T1> : MonoBehaviour where T : Component where T1 : Component
    {
        private static ObjectPoolMultiple<T, T1> instance;
        public static ObjectPoolMultiple<T, T1> Instance => instance ??= FindObjectOfType<ObjectPoolMultiple<T, T1>>();

        [SerializeField] private List<T> poolObjects = new List<T>();
        [SerializeField] private List<int> poolSizes = new List<int>();
        [SerializeField] private float objectLifetime = 1f;

        private List<List<T>> pools = new List<List<T>>();
        private int ObjectCount => poolObjects.Count;


        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(this.transform.parent);
        }

        private void Start()
        {
            for (int i = 0; i < ObjectCount; i++)
            {
                var pool = new List<T>();
                pools.Add(pool);
            }

            for (int i = 0; i < ObjectCount; i++)
            {
                for (int j = 0; j < poolSizes[i]; j++)
                {
                    var objectForPool = Instantiate(poolObjects[i], transform);
                    objectForPool.transform.rotation = Quaternion.identity;
                    objectForPool.gameObject.SetActive(false);
                    pools[i].Add(objectForPool);
                }
            }
        }

        public T GetObject(Type enemyType)
        {
            var theObject = poolObjects.Find(x => x.GetType() == enemyType);
            int indexOfObject = poolObjects.IndexOf(theObject);
            var poolOfObject = pools[indexOfObject];

            for (int i = 0; i < poolSizes[indexOfObject]; i++)
            {
                if (!poolOfObject[i].gameObject.activeInHierarchy)
                {
                    poolOfObject[i].gameObject.SetActive(true);
                    return poolOfObject[i];
                }
            }

            return null;
        }

        public List<T> GetObject(Type enemyType, int count)
        {
            List<T> objects = new List<T>();

            var theObject = poolObjects.Find(x => x.GetType() == enemyType);
            int indexOfObject = poolObjects.IndexOf(theObject);
            var poolOfObject = pools[indexOfObject];

            for (int i = 0; i < poolSizes[indexOfObject]; i++)
            {
                if (!poolOfObject[i].gameObject.activeInHierarchy && objects.Count < count)
                {
                    poolOfObject[i].gameObject.SetActive(true);
                    objects.Add(poolOfObject[i]);
                }
            }

            return objects;
        }

        public void PullObjectBack(T theObject)
        {
            if (!IsObjectInPool(theObject))
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
            if (!IsObjectInPool(theObject))
            {
                Destroy(theObject.gameObject);

                return;
            }

            theObject.transform.rotation = Quaternion.identity;
            theObject.transform.SetParent(this.transform);
            theObject.gameObject.SetActive(false);
        }

        private bool IsObjectInPool(T theObject)
        {
            bool objectInPool = false;

            foreach (var pool in pools)
            {
                if (pool.Contains(theObject))
                {
                    objectInPool = true;
                }
            }

            return objectInPool;
        }
    }
}
