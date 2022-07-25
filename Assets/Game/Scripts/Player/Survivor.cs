using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survivor : MonoBehaviour
{
    [SerializeField] private new Rigidbody rigidbody;


    void Start()
    {
        
    }

    void LateUpdate()
    {
        Avoid();
    }

    public void Avoid()
    {
        var vecCenter = Vector3.zero;
        var vecAvoid = Vector3.zero;
        float distance;
        int groupSize = 0;

        foreach (var otherSurvivor in HiveManager.Instance.Survivors)
        {
            if (otherSurvivor != this)
            {
                var otherSurvivorPos = otherSurvivor.transform.localPosition;
                distance = Vector3.Distance(otherSurvivorPos, transform.localPosition);

                if (distance <= HiveManager.Instance.NeighbourDistance)// inside of the neighbourhood
                {
                    vecCenter += otherSurvivorPos;
                    groupSize++;

                    if(distance <= 0.55f)// the other survivor is so close to this
                    {
                        vecAvoid += transform.localPosition - otherSurvivorPos;
                    }
                }
            }
        }

        if(groupSize > 0)
        {
            var currentPos = transform.localPosition;
            currentPos += vecAvoid / groupSize;
            transform.localPosition = currentPos;
        }     
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Survivor") && collision.transform.TryGetComponent(out Survivor newSurvivor))
        {
            HiveManager.Instance.AddSurvivor(newSurvivor);            
        }
    }
}
