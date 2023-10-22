using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class PaladinStateManager : MonoBehaviour
{
    public Animator anim;
    public GameObject Erika;
    public GameObject Erad;
    public GameObject Abe; 
    public GameObject MoveSpots;
    float currenthp;
    public float staminaNew;
    public bool dead;
    public bool damaging;
    public Stats armor;

    public bool inBattle;

    public float DetectionRange = 38f;
    public float MeleeRange = 2.0f;
    public Transform target;
    public Transform spotTarget;
    
    public Transform[] Focus;
    private int randomFocus;
    private Transform tempTarget;
    //NavMeshAgent agent;

    public float speed;
    public float retreatSpeed;
    private float waitTime;
    public float startWaitTime;
    public Transform[] moveSpots;
    private int randomSpot;
    public Transform retreatSpot;

    public CharacterController CharacterController;
    public Vector3 pushDirection = Vector3.zero;
    private float pushForce = 7.5f;
    public float distanceAbe;
    public float distanceErika;

    private Quaternion initialRotation;
    //List<string> Output;



    public void Awake()
    {
        //Erika = GameObject.FindGameObjectWithTag("Player");
        //Erad = GameObject.FindGameObjectWithTag("Enemy");
        currenthp = Erika.GetComponent<CharacterStats>().currenthp;
        staminaNew = Erad.GetComponent<StaminaController>().currentStaminaErad;
        anim = Erad.GetComponent<Animator>();
        //Output = Erad.GetComponent<FuzzyStateMachinr>().Output;
    }

   

    public PaladinBaseState currState;
    public PaladinSearching SearchState   = new PaladinSearching();
    public PaladinHunting HuntState      = new PaladinHunting();
    public PaladinAttack AttackState   = new PaladinAttack();
    public PaladinIdle IdleState       = new PaladinIdle();
    public PaladinRetreat RetreatState   = new PaladinRetreat();
    public PaladinBlock BlockState       = new PaladinBlock();

    // Start is called before the first frame update
    void Start()
    {
        initialRotation = transform.rotation;
        currState = IdleState;
        currState.EnterState(this);
        waitTime = startWaitTime;
        randomSpot = Random.Range(0, moveSpots.Length);//να μην παιρνει το ιδιο
        spotTarget = moveSpots[randomSpot];
        transform.LookAt(spotTarget);
        randomFocus = Random.Range(0, Focus.Length);
        //target = Focus[randomFocus];
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Erad.GetComponent<StaminaController>().stamina.value >= 100)
        {
            Erad.GetComponent<StaminaController>().recharge = false;
        }
        distanceAbe =  Vector3.Distance(Erad.transform.position, Abe.transform.position);
        distanceErika = Vector3.Distance(Erad.transform.position, Erika.transform.position);
        if (Abe.GetComponent<Support>().isDead == true)
        {
            target = Erika.transform;
        }
        if (target == Abe.transform & (distanceAbe > distanceErika))
        {
            target = Erika.transform;
        }
        else if (target == Erika.transform & (distanceAbe < distanceErika))
        {
            target = Abe.transform;
        }

        

        if (currState == IdleState)
        {
            Erad.GetComponent<StaminaController>().recharge = false;
            anim.SetBool("powerup", true);
            damaging = false;
        }
        if (currState == SearchState )
        {
            Erad.GetComponent<StaminaController>().recharge = false;
            anim.SetBool("block", false);
            anim.SetBool("isRunning", false);
            anim.SetBool("attack1", false);
            damaging = false;
            transform.position = Vector3.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);
            
            if (Vector3.Distance(transform.position, moveSpots[randomSpot].position) < 0.5f)
            {
                
                if (waitTime <= 0)
                {
                    anim.SetBool("moving", true);
                    
                    randomSpot = Random.Range(0, moveSpots.Length);
                    spotTarget = moveSpots[randomSpot];
                    
                    
                    waitTime = startWaitTime;
                    transform.LookAt(spotTarget);
                    
                    
                }
                else
                {
                    anim.SetBool("moving", false);
                    waitTime -= Time.deltaTime;
                    
                    
                    
                }
                
            }
        }
        if (currState == HuntState)
        {
            Erad.GetComponent<StaminaController>().recharge = false;
            inBattle = true;
            anim.SetBool("block", false);
            //anim.SetBool("isRunning", true);
            //anim.SetBool("moving", true);
            damaging = false;
            float distance = Vector3.Distance(target.position, transform.position);
            if (distance <= DetectionRange)
            {
                anim.SetBool("isRunning", true);
                anim.SetBool("moving", false);
                //anim.SetBool("moving", false);
                //CharacterController.Move(pushDirection * pushForce * Time.deltaTime);
                //agent.MoveTowards(target.position, speed * Time.deltaTime);
                transform.LookAt(target);
                transform.position = Vector3.MoveTowards(transform.position, target.position, pushForce * Time.deltaTime);
            }
        }
        else
        {
            //anim.SetBool("isRunning", false);
        }
        if (currState == AttackState)
        {
            Erad.GetComponent<StaminaController>().recharge = false;
            inBattle = true;
            anim.SetBool("block", false);
            anim.SetBool("moving", false);
            anim.SetBool("isRunning", false);
            float distance = Vector3.Distance(target.position, transform.position);
            
            if(distance > 2.3f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, (speed + 2) * Time.deltaTime);
                anim.SetBool("moving", true);
                transform.LookAt(target);
            }
            else if (distance <= 2.3f) //& distance > 1.5f)
            {
                anim.SetBool("attack1", true);
                anim.SetBool("moving", false);
                damaging = true;
            }
            
            
            
            
        }
        else
        {
            //anim.SetBool("moving", true);
            //anim.SetBool("isRunning", false);
            //anim.SetBool("attack1", false);
            damaging = false;
        }
        
        if (currState == BlockState)
        {
            anim.SetBool("attack1", false);
            anim.SetBool("moving", false);
            anim.SetBool("isRunning", false);
            anim.SetBool("block", true);
            transform.LookAt(Erika.transform);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            damaging = false;
        }
        else
        {
            //anim.SetBool("block", false);
        }
        if (currState == RetreatState)
        {
            Erad.GetComponent<StaminaController>().recharge = false;
            anim.SetBool("block", false);
            anim.SetBool("attack1", false);
            anim.SetBool("moving", false);

            damaging = false;

            if (Erad.GetComponent<FuzzyStateMachinr>().isDead)
            {
                retreatSpeed = 0;
                return;
            }
            else if (Vector3.Distance(transform.position, retreatSpot.position) < 5f)
            {
                //Erika.GetComponent<CharacterStats>().count += 1;
                retreatSpeed = 0;
                anim.SetBool("isRunning", false);
                return;
            }
            
            
            
            anim.SetBool("isRunning", true);
            transform.LookAt(retreatSpot);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            transform.position = Vector3.MoveTowards(transform.position, retreatSpot.position, pushForce * Time.deltaTime);
            

        }

        if (Erad.GetComponent<StaminaController>().recharge == true)
        {
            currState = BlockState;
        }
        else if(Erad.GetComponent<StaminaController>().recharge == false)
        {
            currState = GetComponent<FuzzyStateMachinr>().outputState;
        }
        
        currState.UpdateState(this);
        

    }

    public void SwitchState(PaladinBaseState state)
    {
        currState = state;
        state.EnterState(this);
    }

    //public void TakeDamage(int damage)
    //{


    //    damage -= armor.GetValue();
    //    damage = Mathf.Clamp(damage, 0, int.MaxValue);


    //    if (currenthp - damage <= 0)
    //    {
    //        Debug.Log("Zero Hp " + currenthp);
    //        currenthp = 0;
    //    }
    //    else if (currenthp - damage > 0)
    //    {
    //        Debug.Log(transform.name + "takes" + damage + "damage.");
    //        //
    //        currenthp -= damage;

    //        Debug.Log(currenthp);
    //    }

    //    if (currenthp == 0)
    //    {
    //        Die();
    //    }
    //}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, DetectionRange);
    }

    //public virtual void Die()
    //{
    //    Debug.Log(transform.name + "died.");
    //    dead = true;
    //}
}
