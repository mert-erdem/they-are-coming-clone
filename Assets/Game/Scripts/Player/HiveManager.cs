using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveManager : Singleton<HiveManager>
{
    [SerializeField] private Transform hiveRoot;
    [SerializeField] private List<Survivor> survivors;
    public List<Survivor> Survivors => survivors;
    [Header("Specs")]
    [SerializeField] [Range(0.5f, 10f)]
    [Tooltip("Minimum interaction distance for a neighbourhood")]
    private float neighbourDistance = 2f;
    public float NeighbourDistance => neighbourDistance;
    [SerializeField] [Range(0.5f, 1f)]
    [Tooltip("The maximum distance that a survivor can be closer to other")]
    private float closestDistance = 0.55f;
    public float ClosestDistance => closestDistance;


    public void AddSurvivor(Survivor newSurvivor)
    {
        newSurvivor.tag = "Untagged";
        survivors.Add(newSurvivor);
        newSurvivor.transform.position = hiveRoot.position;
        newSurvivor.transform.SetParent(hiveRoot);
        WeaponManager.Instance.GiveWeapon(newSurvivor);

        if(survivors.Count % 5 == 0)
        {
            PlayerController.Instance.UpdateBorders(1);
        }

        // update ui
    }

    public void RemoveSurvivor(int count)
    {
        // remove survivor/s
        // update borders
        // update ui
    }
}
