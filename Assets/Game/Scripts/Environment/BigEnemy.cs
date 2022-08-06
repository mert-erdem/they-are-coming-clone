using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemy : Enemy
{
    [SerializeField] private Transform equipmentsRoot;

    private void Start()
    {
        ImproveTheSpecs();
    }

    /// <summary>
    /// Improves health if this enemy has armor equipments
    /// </summary>
    private void ImproveTheSpecs()
    {
        base.Health += equipmentsRoot.childCount;
    }
}
