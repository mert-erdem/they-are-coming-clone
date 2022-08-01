using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private Transform sideMovementRoot, borderLeft, borderRight;
    [Header("Specs")]
    [SerializeField] [Range(1, 10)] private int speed = 10;
    [SerializeField] [Range(0.01f, 0.2f)] private float speedHorizontal = 0.1f;
    [SerializeField] private float borderNarrowingDelta = 0.1f;

    private Vector3 mouseRootPos;
    private float inputHorizontal;
    

    void Start()
    {
        
    }

    void Update()
    {
        GetInput();
        Move();
    }

    private void GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseRootPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            var dragVec = Input.mousePosition - mouseRootPos;
            inputHorizontal = dragVec.normalized.x;
            mouseRootPos = Input.mousePosition;
        }
        else
        {
            inputHorizontal = 0;
        }
    }

    private void Move()
    {
        transform.Translate(speed * Time.deltaTime * -transform.forward);

        var sideRootPos = sideMovementRoot.localPosition;
        sideRootPos += speedHorizontal * inputHorizontal * Vector3.right;
        sideRootPos.x = Mathf.Clamp(sideRootPos.x, borderLeft.position.x, borderRight.position.x);
        sideMovementRoot.localPosition = sideRootPos;
    }

    /// <summary>
    /// Update the max x-axis travel distance for each new survivor (to expand -1, to narrow 1).
    /// </summary>
    public void UpdateBorders(int direction)
    {
        var borderLeftPos = borderLeft.position;
        var borderRightPos = borderRight.position;

        borderLeftPos.x += borderNarrowingDelta * direction;
        borderRightPos.x -= borderNarrowingDelta * direction;

        if (borderLeftPos.x > 0f || borderRightPos.x < 0f) return;

        borderLeft.position = borderLeftPos;
        borderRight.position = borderRightPos;
    }

    private void HandleWithGate(string gateName)
    {
        char process = gateName[0];
        string number = gateName.Substring(1, gateName.Length - 1);
        bool success = int.TryParse(number, out int count);
        
        if (success)
        {
            switch (process)
            {
                case '+':
                    HiveManager.Instance.AddSurvivors(count);
                    break;
                case 'x':
                    count = HiveManager.Instance.Survivors.Count * (count - 1);
                    HiveManager.Instance.AddSurvivors(count);
                    break;
            }
        }
        else
        {
            print("Couldn't parse the gate");
        }
    }

    private void OnTriggerEnter(Collider other)// side root (for gates)
    {
        if(other.CompareTag("Gate"))
        {
            HandleWithGate(other.name);
        }
        else if(other.CompareTag("Weapon"))
        {
            WeaponManager.Instance.UpdateWeapons(other.name);
            Destroy(other.gameObject);
        }
    }
}
