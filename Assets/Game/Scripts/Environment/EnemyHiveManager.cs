using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyHiveManager : Singleton<EnemyHiveManager>
{
    private List<Enemy> enemies = new List<Enemy>();
    [SerializeField] private Transform currentTarget;// the first target was assigned via editor

    public void Join(Enemy enemy)
    {
        enemies.Add(enemy);
        enemy.SetTarget(currentTarget);
    }

    public void Leave(Enemy enemy)
    {
        enemies.Remove(enemy);

        if(enemies.Count == 0)
        {
            GameManager.ActionLevelPass?.Invoke();
        }
    }

    public void UpdateTheTarget(Transform target)
    {
        currentTarget = target;
        enemies.ForEach(x => x.SetTarget(currentTarget));
    }
}
