using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    Animator anim;
    float velocityZ = 0.0f;
    float velocityX = 0.0f;
    public float acceleration = 20.0f;
    public float runAcc = 50.0f;
    public float deceleration = 20.0f;
    public bool isJumping;
    public bool isCrouching;
    public bool isAttacking;

    public CharacterController CharacterController;
    public Vector3 pushDirection = Vector3.zero;
    public float pushForce = 10f;


    public float enemyDmg;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) && velocityZ < 1.0f )
        {
            velocityZ += Time.deltaTime * acceleration;
            anim.SetBool("isWalking", true);
        }
        else if (!Input.GetKey(KeyCode.W))
        {
            anim.SetBool("isWalking", false);
        }
        if (Input.GetKey(KeyCode.S) && velocityZ < -1.0f)
        {
            velocityZ -= Time.deltaTime * acceleration;
            anim.SetBool("isWalking", true);
        }
        else if (!Input.GetKey(KeyCode.S))
        {
            //anim.SetBool("isWalking", false);
        }
        if (Input.GetKey(KeyCode.W) && velocityZ <= 2.0f && Input.GetKey(KeyCode.LeftShift))
        {
            velocityZ += Time.deltaTime * (runAcc + 10.0f);
            CharacterController.Move(pushDirection * pushForce * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A) && velocityX < 1.0f && Input.GetKey(KeyCode.LeftShift))
        {
            velocityX -= Time.deltaTime * runAcc;
        }
        if (Input.GetKey(KeyCode.D) && velocityX < 1.0f && Input.GetKey(KeyCode.LeftShift))
        {
            velocityX += Time.deltaTime * runAcc;
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            anim.SetBool("arrow", true);
            //var arrowClone = GameObject.FindGameObjectWithTag("Arrow");
            //arrowClone.GetComponent<Rigidbody>().AddForce(arrowClone.transform.forward, ForceMode.Impulse);
            isAttacking = true; 

        }
        else if (!Input.GetKey(KeyCode.C) && isAttacking)
        {
            anim.SetBool("arrow", false);
        }
        //else
        //{
        //    anim.SetBool("arrow", false);
        //}
        if (Input.GetKey(KeyCode.C))
        {
            anim.SetBool("crouch", true);
            isCrouching = true;
        }
        else if (!Input.GetKey(KeyCode.C) && isCrouching)
        {
            anim.SetBool("crouch", false);
        }
        
        if (Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("jump", true);
            isJumping = true;
        }
        else if (!Input.GetKey(KeyCode.Space) && isJumping )
        {
            anim.SetBool("jump", false);
        }
        
        

        if (!Input.GetKey(KeyCode.W) && velocityZ > 0.0f && !Input.GetKey(KeyCode.S))
        {
            velocityZ -= Time.deltaTime * deceleration;
        }
        //if (!Input.GetKey(KeyCode.W) && velocityZ < 0.0f)
        //{
        //    velocityZ = 0.0f;
        //}

        anim.SetFloat("velocityZ", velocityZ);
        anim.SetFloat("velocityX", velocityX);
    }
}
