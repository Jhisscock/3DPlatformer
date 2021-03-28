using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{

    public LayerMask groundMask, wallMask, slopeMask, grappleMask;
    [SerializeField]
    private bool G, W, AG, OS;
    public static bool isGrounded, isOnWall, isAirGrounded, isOnSlope;
    public Transform groundCheck, wallCheck, airCheck, slopeCheck;
    public float groundDistance = 0.4f, wallDistance = 0.1f, airGroundDistance = 0.4f, slopeDistance = 0.4f;

    // Update is called once per frame
    void Update()
    {
        G = isGrounded;
        W = isOnWall;
        AG = isAirGrounded;
        OS = isOnSlope;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isOnWall = Physics.CheckSphere(wallCheck.position, wallDistance, wallMask);
        if(!isGrounded){
            isAirGrounded = Physics.CheckSphere(airCheck.position, airGroundDistance, groundMask);
        }
        isOnSlope = Physics.CheckSphere(slopeCheck.position, slopeDistance, slopeMask);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(airCheck.position, airGroundDistance);  
    }
}
