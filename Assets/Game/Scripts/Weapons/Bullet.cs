using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * transform.forward);
    }
}
