using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survivor : MonoBehaviour
{
    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private List<Weapon> weapons;

    void Start()
    {
        
    }

    void LateUpdate()
    {
        Avoid();
    }

    private void Avoid()
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

                    if(distance <= HiveManager.Instance.ClosestDistance)// the other survivor is so close to this
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

    public void ChangeWeapon(string weaponName)
    {
        foreach (var weapon in weapons)
        {
            if(weapon.name == weaponName)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Use this method instead of ChangeWeapon if this survivor is a new one.
    /// </summary>
    public void PickUpWeapon(string weaponName)
    {
        var weapon = weapons.Find(x => x.name == weaponName);
        weapon.gameObject.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Survivor") && collision.transform.TryGetComponent(out Survivor newSurvivor))
        {
            HiveManager.Instance.AddSurvivor(newSurvivor);            
        }
    }
}
