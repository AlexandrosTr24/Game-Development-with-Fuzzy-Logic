using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Rendering.InspectorCurveEditor;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class WarrokStateManager : MonoBehaviour
{
    public Animator anim;
    public GameObject Warrok;
    public Transform target;
    public Transform spotTarget;
    public float speed;
    public bool damaging;
    public bool needHeal = false;
    public int healing;
    public bool isRaging = false;

    WarrokBaseState currentState;
    public IdleState Idle = new IdleState();
    public AttackState Attack = new AttackState();
    public RageState Rage = new RageState();
    public HealState Heal = new HealState();
    public DeathState Death = new DeathState();
    public LungeState Lunge = new LungeState();

    public Slider healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentState = Idle;
        currentState.EnterState(this);
        healing = 1;
        transform.LookAt(spotTarget);
    }
    private void Awake()
    {
        anim = Warrok.GetComponent<Animator>();
        Rigidbody rigidBody = Warrok.GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentState);
        if (currentState == Attack)
        {
            //Debug.Log("attacking");
            anim.SetBool("isLunging", false);
            float distance = Vector3.Distance(target.position, transform.position);
            if ( distance > 3.7f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, (speed + 5) * Time.deltaTime);
                transform.LookAt(target);
                anim.SetBool("isWalking", true);
                anim.SetBool("isAttacking", false);
                if (Warrok.GetComponent<FuzzyController>().isDead)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target.position, 0 * Time.deltaTime);
                }

            }
            else
            {
                damaging = true;
                transform.LookAt(target);
                anim.SetBool("isAttacking", true);
                anim.SetBool("isWalking", false);
                
            }

        }
        if(currentState == Rage)
        {
            transform.LookAt(target);
            transform.position = Vector3.MoveTowards(transform.position, target.position, (speed + 1) * Time.deltaTime);
            anim.SetBool("isWalking", true);
            anim.SetBool("isAttacking", false);
            isRaging = true;
        }
        if(currentState == Heal & healing == 1)
        {
            
            needHeal = true;
            if(healing == 1)
            {
                Debug.Log("healing");
                HealCast(100f - Warrok.GetComponent<WarrokStats>().currentHealth);
                healthBar.value = Warrok.GetComponent<WarrokStats>().currentHealth;
                healing -= 1;
            }
            

        }
        if(currentState == Lunge)
        {
            
            float distance = Vector3.Distance(target.position, transform.position);
            if (distance > 7.8f & distance < 60.0f)
            {
                damaging = true;

                //anim.SetBool("isWalking", false);
                anim.SetBool("isLunging", true);
                Debug.Log(distance);
                transform.position = Vector3.MoveTowards(transform.position, target.position, (speed + 30) * Time.deltaTime);
                //Warrok.GetComponent<Rigidbody>().AddForce(transform.forward * (speed + 60), ForceMode.Impulse);
                transform.LookAt(target);
                
                if (Warrok.GetComponent<FuzzyController>().isDead)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target.position, 0 * Time.deltaTime);
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, (speed) * Time.deltaTime);
                transform.LookAt(target);
                anim.SetBool("isWalking", true);
                anim.SetBool("isLunging", false);
                if (Warrok.GetComponent<FuzzyController>().isDead)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target.position, 0 * Time.deltaTime);
                }
            }
            //anim.SetBool("isLunging", true);
        }
        else
        {
            //anim.SetBool("isLunging", false);
        }

        currentState = GetComponent<FuzzyController>().outputState;
        currentState.UpdateState(this);
    }
    public void HealCast(float amount)
    {

        Warrok.GetComponent<WarrokStats>().currentHealth += amount;

        //if (Warrok.GetComponent<WarrokStateManager>().healing == 1)
        //{
        //    //Warrok.GetComponent<WarrokStats>().currentHealth = 100.0f;
        //    Warrok.GetComponent<WarrokStateManager>().healing = 0;
        //}
    }

    public void switchState(WarrokBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
}
