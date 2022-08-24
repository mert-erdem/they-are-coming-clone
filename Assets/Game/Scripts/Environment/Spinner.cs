using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] private int spinningSpeed = 5;

    void Update()
    {
        transform.Rotate(0, 0, 10 * Time.deltaTime * spinningSpeed);
    }
}
