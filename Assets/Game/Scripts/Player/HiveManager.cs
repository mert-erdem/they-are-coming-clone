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


    public void Join(Survivor newSurvivor)
    {
        newSurvivor.tag = "HiveSurvivor";

        survivors.Add(newSurvivor);

        var newSurvivorPos = hiveRoot.position;
        newSurvivorPos.y = newSurvivor.transform.position.y;
        newSurvivor.transform.position = newSurvivorPos;

        newSurvivor.transform.SetParent(hiveRoot);

        WeaponManager.Instance.GiveWeapon(newSurvivor);

        if(survivors.Count % 5 == 0)
        {
            PlayerController.Instance.UpdateBorders(1);
        }

        // update ui
    }

    public void Leave(Survivor survivor)
    {
        // remove survivor
        survivors.Remove(survivor);
        SurvivorPool.Instance.PullObjectBackImmediate(survivor);
        // update borders
        PlayerController.Instance.UpdateBorders(1);
        // update ui
    }

    public void AddSurvivors(int count)
    {
        // with object pooling
        var survivors = SurvivorPool.Instance.GetObject(count);
        survivors.ForEach(x => x.EnterTheHive());
    }
}
