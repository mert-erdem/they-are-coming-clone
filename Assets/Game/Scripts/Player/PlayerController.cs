using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform sideMovementRoot;
    [SerializeField] private Transform borderLeft, borderRight;
    [Header("Specs")]
    [SerializeField] private float speed = 10f;
    [SerializeField] [Range(0.1f, 1f)] private float speedHorizontal = 0.1f;
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
}
