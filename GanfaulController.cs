using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using UnityEngine.SceneManagement;

public class GanfaulController : MonoBehaviour
{
    //Animation Section
    public Animator anim;
    public Material hpMaterialGandaulf;

    public Transform mainCamera;
    public float speed = 6f;
    public float smoothTurn = 0.1f;
    float TurnVelocity;

    public GameObject Warrok;

    [Header("Movement Control Section")]
    public float sensX;
    public float sensY;
    float xRotation;
    float yRotation;
    float horizontalInput;
    float verticalInput;
    float velocityZ = 0.0f;
    float velocityX = 0.0f;
    public float acceleration = 2.0f;
    public float deceleration = 2.0f;
    public float maxWalkVelocity = 0.5f;
    public float maxRunVelocity = 2.0f;

    public Transform playerOrientation;
    public float movementSpeed;
    public float runSpeed = 30f;
    Vector3 movementDirection;
    Rigidbody rigidBody;
    public float playerHeight;
    public LayerMask Ground;
    public float drag;
    bool isGrounded;
    bool isAttacking;
    public float attackCd = 2.0f;
    public float attackDur = 0.0f;
    public float lastAttack;

    public float GandaulfHp = 100.0f;
    public Slider healthBar;
    private bool isDead = false;

    //Dash Ability Variables
    public float dashForce;
    public float dashDuration = 0.2f;
    private bool isDashing = false;
    public float dashCooldown = 1f;
    private bool canDash = true;

    public float loadDelay = 5f;
    public float restartDelay = 3f;

    public bool pause;

    private void OnCollisionEnter(Collision info)
    {
        if(info.collider.tag == "Melee")
        {
            //Debug.Log("got hit");
            if(Warrok.GetComponent<WarrokStateManager>().isRaging == true)
            {
                GandaulfHp -= 30.0f;
                hpMaterialGandaulf.SetFloat("_Health", GandaulfHp / 10);
            }
            else
            {
                GandaulfHp -= 20.0f;
                hpMaterialGandaulf.SetFloat("_Health", GandaulfHp / 10);
            }
            
            //healthBar.value = GandaulfHp;
            if (GandaulfHp <= 0.0f)
            {
                anim.SetBool("isDead", true);
                isDead = true;
                //Debug.Log("is Dead");

            }
        }
    }
    

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        hpMaterialGandaulf.SetFloat("_Health", GandaulfHp/10);
        Cursor.lockState = CursorLockMode.Locked;
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pause == true)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        //Character & Camera Rotation
        CharacterRotation();

        //Attack Animations
        //AttackAnimations();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetBool("NormalA", true);

        }
        else if (Time.time - lastAttack > attackCd)
        {
            anim.SetBool("NormalA", false);
            lastAttack = Time.time;

        }


        //else if (!Input.GetKeyDown(KeyCode.Mouse0) && isAttacking)
        //{
        //    anim.SetBool("NormalA", false);
        //}
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            anim.SetBool("ChargeA", true);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            anim.SetBool("ChargeA", false);
        }

        if(Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            Vector3 dashDirection = GetDashDirection();
            StartCoroutine(Dash(dashDirection));
        }

        //Ground Check
        GroundDrag();

        //Limit Velocity
        SpeedLimit();

        //MovementAnimations
        MovementAnimations();

    }

    void FixedUpdate()
    {
        MovementInputControl();

    }

    private Vector3 GetDashDirection()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if(direction.magnitude < 0.1f)
        {
            direction = transform.forward;
        }

        return direction;
    }

    private IEnumerator Dash(Vector3 dashDirection)
    {
        //if (!canDash) yield break;
        //Debug.Log("Dashing");
        //canDash = false;
        //isDashing = true;
        //rigidBody.AddForce(transform.forward * dashForce , ForceMode.VelocityChange);
        //yield return new WaitForSeconds(dashDuration);
        //rigidBody.velocity = Vector3.zero;
        //isDashing = false;
        canDash = false;
        rigidBody.AddForce(dashDirection * dashForce, ForceMode.VelocityChange);
        //rigidBody.velocity = dashDirection * dashForce * 10;
        yield return new WaitForSeconds(dashDuration);
        rigidBody.velocity = Vector3.zero;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

    }

    private void MovementInputControl()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        movementDirection = playerOrientation.forward * verticalInput + playerOrientation.right * horizontalInput;
        rigidBody.AddForce(movementDirection.normalized * movementSpeed * 10f, ForceMode.Force);

        
    }
    private void CharacterRotation()
    {
        ////Mouse Direction
        //float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        //float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
        ////yRotation += mouseX;
        //yRotation += mouseX;
        //xRotation = Mathf.Clamp(xRotation, -90f, +90f);
        ////Camera Rotation
        //transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        //playerOrientation.rotation = Quaternion.Euler(0, yRotation, 0);
        float targetAngle1 = mainCamera.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0f, targetAngle1, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f * Time.deltaTime);
        playerOrientation.rotation = rotation;
    }

    private void MovementAnimations()
    {
        //Running check (if left shift is pressed)
        float currVelocity;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currVelocity = maxRunVelocity;
        }
        else
        {
            currVelocity = maxWalkVelocity;
        }
        //Even if Left shift is stopped being pressed the running animation will still play
        //when it is not pressed the walking animation should be played again 
        //Fix in the video. Min 18:00

        //Running
        if (currVelocity > 0.5f)
        {
            rigidBody.AddForce(movementDirection.normalized * movementSpeed * runSpeed, ForceMode.Force);
        }
        //Walking and Running Forward(acceleration & deceleration)
        if (Input.GetKey(KeyCode.W) && velocityZ < currVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;
            anim.SetBool("isWalking", true);
            
        }
        else if (!(Input.GetKey(KeyCode.W)) && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * deceleration;
            anim.SetBool("isWalking", false);
        }

        //Walking and Running Left
        if (Input.GetKey(KeyCode.A) && velocityX > -currVelocity)
        {
            velocityX -= Time.deltaTime * acceleration;
            anim.SetBool("isWalking", true);
        }
        else if (!(Input.GetKey(KeyCode.A)) && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * deceleration;
            anim.SetBool("isWalking", false);
        }

        //Walking and Running Right
        if (Input.GetKey(KeyCode.D) && velocityX < currVelocity)
        {
            velocityX += Time.deltaTime * acceleration;
            anim.SetBool("isWalking", true);
        }
        else if (!(Input.GetKey(KeyCode.D)) && velocityX > 0.5f)
        {
            velocityX -= Time.deltaTime * deceleration;
            anim.SetBool("isWalking", false);
        }

        //VelocityZ Reset
        //if (!(Input.GetKey(KeyCode.W) && velocityZ < 0.0f))
        //{
        //    velocityZ = 0.0f;
        //}

        //VelocityX Reset
        //if (!Input.GetKey(KeyCode.A) && !(Input.GetKey(KeyCode.D) && velocityX != 0.0f && (velocityX > -0.05f && velocityX < 0.05f)))
        //{
        //velocityX = 0.0f;
        //}

        //Forward
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift) && velocityZ > currVelocity)
        {
            velocityZ = currVelocity;
        }
        else if (Input.GetKey(KeyCode.W) && velocityZ > currVelocity)
        {
            velocityZ -= Time.deltaTime * deceleration;
            if (velocityZ > currVelocity && velocityZ < (currVelocity + 0.05f))
            {
                velocityZ = currVelocity;
            }
        }
        else if (Input.GetKey(KeyCode.W) && velocityZ < currVelocity && velocityZ > (currVelocity - 0.05f))
        {
            velocityZ = currVelocity;
        }

        //Passing the Velocity variables to the animator
        anim.SetFloat("Velocity Z", velocityZ);
        anim.SetFloat("Velocity X", velocityX);
    }

    private void AttackAnimations()
    {
        //if (Input.GetKey(KeyCode.Mouse0))
        //{
        //    anim.SetBool("NormalA", true);

        //}
        //else if (Time.time - lastAttack > attackCd)
        //{
        //    anim.SetBool("NormalA", false);
        //    lastAttack = Time.time;
            
        //}
       
       
        ////else if (!Input.GetKeyDown(KeyCode.Mouse0) && isAttacking)
        ////{
        ////    anim.SetBool("NormalA", false);
        ////}
        //if (Input.GetKeyDown(KeyCode.Mouse1))
        //{
        //    anim.SetBool("ChargeA", true);
        //}
        //else if (Input.GetKeyUp(KeyCode.Mouse1))
        //{
        //    anim.SetBool("ChargeA", false);
        //}
    }

    private void GroundDrag()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground);
        if (isGrounded)
            rigidBody.drag = drag;
        else
            rigidBody.drag = 0;
    }

    private void SpeedLimit()
    {
        Vector3 Velocity = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);
        if (Velocity.magnitude > movementSpeed)
        {
            Vector3 limitVelocity = Velocity.normalized * movementSpeed;
            rigidBody.velocity = new Vector3(limitVelocity.x, rigidBody.velocity.y, limitVelocity.z);
        }
    }

    public virtual void Die()
    {
        Debug.Log(transform.name + "died.");
        Invoke("Restart", restartDelay);
    }

    IEnumerator LoadSceneWithDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(loadDelay);

        // Load the new scene
        SceneManager.LoadScene(1);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pause = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pause = false;
    }
}
