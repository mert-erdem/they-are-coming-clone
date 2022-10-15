using System.Collections;
using System.Collections.Generic;
using Game.Core;
using UnityEngine;

[SelectionBase]
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
            inHive = inHiveAtStart = true;
            meshRenderer.material = matInHive;
            GameManager.ActionGameStart += PrepareInHiveState;
        }
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

    

    private void Avoid()
    {
        //var vecCenter = Vector3.zero;
        var vecAvoid = Vector3.zero;
        float distance;
        int groupSize = 0;
        var hiveManager = HiveManager.Instance;

        foreach (var otherSurvivor in hiveManager.Survivors)
        {
            if (otherSurvivor != this)
            {
                var otherSurvivorPos = otherSurvivor.transform.localPosition;
                distance = Vector3.Distance(otherSurvivorPos, transform.localPosition);

                if (distance <= hiveManager.NeighbourDistance)// inside of the neighbourhood
                {
                    //vecCenter += otherSurvivorPos;
                    groupSize++;

                    if(distance <= hiveManager.ClosestDistance)// the other survivor is so close to this
                    {
                        vecAvoid += transform.localPosition - otherSurvivorPos;

                        if(vecAvoid == Vector3.zero)
                        {
                            float randomValueX = Random.Range(0.55f, 0.75f) * (Random.Range(0, 1) * 2 - 1);
                            float randomValueZ = Random.Range(0.55f, 0.75f) * (Random.Range(0, 1) * 2 - 1);
                            vecAvoid += new Vector3(randomValueX, 0, randomValueZ);
                        }
                    }
                }
            }
        }

        if(groupSize > 0)
        {
            //vecCenter /= groupSize;
            var currentPos = transform.localPosition;
            currentPos += vecAvoid / groupSize;
            //currentPos = (vecCenter - currentPos) * 1f;
            transform.localPosition = currentPos;
        }

        Align();
    }

    private void Align()
    {
        var clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, -3.5f, 3.5f);
        clampedPos.y = 0f;
        transform.position = clampedPos;
        //PerformCohesion();
    }

    /*
    public void PerformCohesion()
    {
        var currentPos = transform.localPosition;
        if(Vector3.Distance(currentPos, vecCenter) > 3f)
           currentPos += (vecCenter - currentPos) * 0.1f;
        transform.localPosition = currentPos;
    }
    */

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

    public void ResetWeapon()// before turning back to the pool
    {
        string defaultWeapon = weapons[0].name;

        foreach (var weapon in weapons)
        {
            if (weapon.name == defaultWeapon)
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

    public void StopFiring()
    {
        weapons.ForEach(x => x.gameObject.SetActive(false));
        SetState(stateIdle);
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

    private void ChangeAnim()
    {
        if (animator != null)
            animator.SetTrigger("Change");
    }

    private void Die()
    {
        if(inHive)
        {
            if (inHiveAtStart)
            {
                GameManager.ActionGameStart -= PrepareInHiveState;
            }

            HiveManager.Instance.Leave(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void PrepareInHiveState()
    {
        SetState(stateRun);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HiveSurvivor"))
        {
            if (!inHive)
            {
                EnterTheHive();
            }
        }

        if (other.CompareTag("Enemy") || other.CompareTag("Obstacle"))
        {
            Die();
        }
    }
}
