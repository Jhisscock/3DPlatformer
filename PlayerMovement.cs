using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f, gravity = -9.81f, jumpHeight = 3f;
    private Vector3 velocity;
    [SerializeField]
    private Vector3 playerVelocity = Vector3.zero;
    [SerializeField]
    private Vector3 hitNormal;
    private float airSpeed = 1f;
    public float airAccel = 0.001f;
    [SerializeField]
    private float slopeSpeed = 1f, slopeFriction = 0.75f;
    public float slopeAccel = 0.004f;
    [SerializeField]
    private Vector3 impact = Vector3.zero, grappleImpact = Vector3.zero;
    public float force, grappleForce;
    private bool canJump = true, hitGrapple = false;
    public bool finish = false;
    private bool slopeExit = false;
    private Vector3 prevAirVelocity;
    public LayerMask wallMask, slopeMask, grappleMask;
    private GameObject lastHitObject;

    private void Start() {
        finish = false;
    }

    void Update()
    {
        if(Collision.isGrounded && velocity.y < 0){
            ResetValuesOnGround();
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxis("Horizontal");
        float zRaw = Input.GetAxis("Vertical");

        playerVelocity = transform.right * x + transform.forward * z;

        CalculateNormals(new Vector3(xRaw, 0, zRaw));

        if(Collision.isAirGrounded || Collision.isGrounded){
            canJump = true;
        }

        if(Input.GetButtonDown("Jump") && (Collision.isGrounded || Collision.isAirGrounded || Collision.isOnSlope || Collision.isOnWall) && canJump){
            if(Collision.isOnSlope || Collision.isOnWall){
                AddImpact();
                canJump = false;
                hitNormal = Vector3.zero;
            }
            Jump();
        }

        if(Input.GetKey(KeyCode.Mouse0)){
            GrappleCheck();
        }

        if(!Collision.isGrounded && !Collision.isOnSlope && !Collision.isOnWall && !hitGrapple){
            AirAccel();
            if(playerVelocity.magnitude < 0.5f){
                airSpeed = 1f;
            }  
        }

        if(Collision.isOnSlope && !Collision.isGrounded){
            SlopeAccel();
            slopeExit = true;
        }else{
            slopeExit = false;
            slopeSpeed = 1f;
        }
        
        velocity.y += gravity * Time.deltaTime;

        impact = Vector3.Lerp(impact, Vector3.zero, 3 * Time.deltaTime);
        grappleImpact = Vector3.Lerp(grappleImpact, Vector3.zero, 5 * Time.deltaTime);

        if(impact.magnitude > 0.2) controller.Move(impact * Time.deltaTime);

        if(grappleImpact.magnitude > 0.2)controller.Move(grappleImpact * Time.deltaTime);

        controller.Move(playerVelocity * speed * Time.deltaTime);

        controller.Move(velocity * Time.deltaTime);

    }
    private void Jump(){
        controller.stepOffset = 0;
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private void SlopeAccel(){
        //Allows player to move on slope where the camera is facing
        float playerMag = playerVelocity.magnitude;
        if(playerMag == 0) playerMag = 1; 
        Vector3 result = prevAirVelocity.magnitude * playerVelocity / playerMag;
        playerVelocity += result * slopeFriction;
        prevAirVelocity = result;

        //Calculate player velocity x and z based of the normals of the slope multipled by the slope speed
        slopeSpeed += slopeAccel * Time.deltaTime;
        playerVelocity.x += hitNormal.x * slopeSpeed;
        playerVelocity.z += hitNormal.z * slopeSpeed;
    }

    private void AirAccel(){
        airSpeed += airAccel * Time.deltaTime;
        playerVelocity *= airSpeed;
        prevAirVelocity = playerVelocity;
    }

    private void ResetValuesOnGround(){
        velocity.y = -1f;
        controller.stepOffset = 0.3f;
        Collision.isAirGrounded = false;
        airSpeed = 1f;
        slopeSpeed = 1f;
        impact = Vector3.zero;
        grappleImpact = Vector3.zero;
        prevAirVelocity = playerVelocity;
    }

    private void AddImpact(){
        hitNormal.Normalize();
        impact += hitNormal.normalized * force;
        impact.y = 0;
    }

    private void GrappleCheck(){
        Vector3 lookDirection = Camera.main.transform.forward;
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, lookDirection, out hit, Mathf.Infinity, grappleMask)){
            lookDirection.Normalize();
            grappleImpact += lookDirection.normalized * grappleForce;
            hitGrapple = true;
        }else{
            hitGrapple = false;
        }
    }

    private void CalculateNormals(Vector3 dir){
        if(Collision.isOnSlope && !Collision.isOnWall && !Collision.isGrounded){
            CalculateHitNormal(Vector3.down, slopeMask);
        }

        if(Collision.isOnWall && !Collision.isOnSlope && !Collision.isGrounded){
            CalculateHitNormal(dir, wallMask);
        }
    }

    private void CalculateHitNormal(Vector3 dir, LayerMask mask){
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.TransformDirection(dir), out hit, Mathf.Infinity, mask)){
            hitNormal = hit.normal;
            if(!GameObject.ReferenceEquals(lastHitObject, null) && !GameObject.ReferenceEquals(hit.transform.gameObject, lastHitObject)){
                canJump = true;
            }
            lastHitObject = hit.transform.gameObject;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Finish"){
            finish = true;
        }
    }
}
