using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour

{
    private bool canJump = true;
    private bool canPowerJump = false;
    public float jumpforce = 2;
    public float gravity = 30.0f;
    public float moveSpeed = 5f;
    Vector3 direction=Vector3.zero;
    Vector3 moveDirection = Vector3.zero;
    public float rotationSpeed = 360f;
    CharacterController characterController;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        

    }
    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (moveDirection.y <= 0)
            
                if(canJump)
                {
                    canJump = false;
                animator.SetInteger("Jump", 1);
                if (canPowerJump)
                {
                    canPowerJump = false;
                    moveDirection.y = jumpforce*2;
                }
                else
                {
                    moveDirection.y = jumpforce;
                }   
            }
        }
        float x2 = Input.GetAxis("Horizontal");
        float z2 = Input.GetAxis("Vertical");

        direction = new Vector3(x2, 0, z2);

        if (direction.sqrMagnitude > 0.01f)
        {
            Vector3 forward = Vector3.Slerp(transform.forward, direction,
                rotationSpeed * Time.deltaTime / Vector3.Angle(transform.forward, direction));
            transform.LookAt(transform.position + forward);
            characterController.Move(direction * moveSpeed * Time.deltaTime);
        }
        animator.SetFloat("Speed", characterController.velocity.magnitude);
        if (characterController.isGrounded)
        {
            if(moveDirection.y<=0) 
                canJump = true;
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            moveDirection = new Vector3(x, moveDirection.y, z);
            
          
            characterController.Move(moveDirection * Time.deltaTime);
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        characterController.Move(moveDirection * Time.deltaTime);
        
    }
    private void FixedUpdate()
    {
        animator.SetInteger("Jump", 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Respawn"))
        {
            canPowerJump = true;
            Destroy(other.gameObject);
        }
    }
}
