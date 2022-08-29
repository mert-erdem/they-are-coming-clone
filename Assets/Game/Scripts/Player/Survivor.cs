using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survivor : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private Animator animator;
    [SerializeField] private SkinnedMeshRenderer meshRenderer;

    [SerializeField] private List<Weapon> weapons;
    [SerializeField] private Material matInHive, matOutHive;

    private bool inHive = false, inHiveAtStart = false;
    private State stateIdle, stateRun, stateCurrent;
    private Vector3 miniGamePos;

    private void Awake()
    {
        stateIdle = new State(() => { }, () => { }, ChangeAnim);
        stateRun = new State(Avoid, ()=> { }, ChangeAnim);
        SetState(stateIdle);
    }

    private void Start()
    {
        if (CompareTag("HiveSurvivor"))// manually added survivor/s at editor time
        {
            inHive = true;
            inHiveAtStart = true;
            meshRenderer.material = matInHive;
            GameManager.ActionGameStart += PrepareInHiveState;
        }
            

        // TODO first survivors must be at runState when gamestart action fired
    }

    void Update()
    {
        stateCurrent.onUpdate();
    }

    private void SetState(State state)
    {
        if (stateCurrent != null)
            stateCurrent.onStateExit();

        stateCurrent = state;
        stateCurrent.onStateEnter();
    }

    private void ChangeAnim()
    {
        animator.SetTrigger("Change");
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
            if (weapon.name == weaponName)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
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

    public void EnterTheHive()
    {
        inHive = true;
        SetState(stateRun);
        meshRenderer.material = matInHive;
        HiveManager.Instance.Join(this);
    }

    public void GoToMiniGamePos(Vector3 pos)
    {
        transform.position = pos;
        SetState(stateIdle);
    }

    private void Die()
    {
        if(inHive)
        {
            HiveManager.Instance.Leave(this);
        }
        else
        {
            SurvivorPool.Instance.PullObjectBackImmediate(this);
        }

        if(inHiveAtStart)
        {
            GameManager.ActionGameStart -= PrepareInHiveState;
        }
    }

    private void PrepareInHiveState()
    {
        SetState(stateRun);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("HiveSurvivor"))
        {
            if (!inHive)
            {
                EnterTheHive();
            }         
        }
        else if(collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Obstacle"))
        {
            Die();// whether in hive or not
        }
    }
}
