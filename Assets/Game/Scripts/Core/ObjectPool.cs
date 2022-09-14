using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T, T1> : MonoBehaviour where T : Component where T1 : Component
{
    private static ObjectPool<T, T1> instance;
    public static ObjectPool<T, T1> Instance => instance ??= FindObjectOfType<ObjectPool<T, T1>>();

    [SerializeField] private T poolObject;
    [SerializeField] private int poolSize;
    [SerializeField] private float objectLifetime = 1f;
    private List<T> pool;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        //DontDestroyOnLoad(this.transform.parent);
    }

    private void Start()
    {
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
            if(!pool[i].gameObject.activeInHierarchy && objects.Count < count)
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

        theObject.transform.SetParent(this.transform.parent);
        theObject.gameObject.SetActive(false);
    }

    public void PullObjectBackImmediate(T theObject)
    {
        if (!pool.Contains(theObject))
        {
            Destroy(theObject.gameObject);

            return;
        }

        theObject.transform.SetParent(this.transform.parent);
        theObject.gameObject.SetActive(false);
    }
}
