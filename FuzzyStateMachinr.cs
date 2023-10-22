using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuzzyStateMachinr : MonoBehaviour
{
    public GameObject Erika;
    public GameObject Paladin;
    public GameObject Abe;
    public Slider healthBar;

    private int count = 0;

    public float maxHealth = 100.0f;
    public float currentHealth;

    public float distance;

    double dom1 = -1f;
    double dom2 = 0f;
    double dom3 = 0f;
    double dom4 = 0f;
    double dom5 = 0f;
    double dom6 = 0f;
    double dom7 = 0f;
    double dom8 = 0f;
    double dom9 = 0f;
    double dom10 = 0f;
    double dom11 = 0f;
    double dom12 = 0f;

    public string outputRule1;
    public string outputRule2;

    public Text Decision1;
    public Text Decision2;
    public Text State;
    public Text HealthPoints;
    public Text StaminaPoints;
    public Text DistanceValue;

    double DomRule1;
    double DomRule2;
    double TotalOutput;

    bool Retreating;
    bool Blocking;
    bool Searching;
    bool Hunting;
    bool Attacking;
    public bool isDead;

    public Animator anim;

    public float PaladinHp;
    public float Stamina;

    public PaladinBaseState outputState;
    public PaladinSearching SearchState;
    public PaladinHunting HuntState; // = new PaladinHunting();
    public PaladinAttack AttackState; // = new PaladinAttack();
    public PaladinIdle IdleState; // = new PaladinIdle();
    public PaladinRetreat RetreatState; // = new PaladinRetreat();
    public PaladinBlock BlockState; // = new PaladinBlock();

    private void Awake()
    {
        //Erika = GameObject.FindGameObjectWithTag("Player");
        //Paladin = GameObject.FindGameObjectWithTag("Enemy");
        Stamina = Paladin.GetComponent<StaminaController>().stamina.value;
        distance = Paladin.GetComponent<Distance>().distance;
        SearchState = Paladin.GetComponent<PaladinStateManager>().SearchState;
        AttackState = Paladin.GetComponent<PaladinStateManager>().AttackState;
        IdleState = Paladin.GetComponent<PaladinStateManager>().IdleState;
        RetreatState = Paladin.GetComponent<PaladinStateManager>().RetreatState;
        BlockState = Paladin.GetComponent<PaladinStateManager>().BlockState;
        HuntState = Paladin.GetComponent<PaladinStateManager>().HuntState;

    }

    



    //Variables needed
    //PaladinHp,PaladinStamina,ErikaHp

    // Start is called before the first frame update
    void Start()
    {
        PaladinHp = 100.0f;
        currentHealth = 100.0f;
 
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;

        Decision1.text = "Searching";
        Decision2.text = "Searching";
        //HealthPoints = GetComponent<Text>();
        //DistanceValue = GetComponent<Text>();
        //StaminaPoints = GetComponent<Text>();
    }

    private void OnCollisionEnter(Collision info)
    {

        if (info.collider.tag == "Arrow")
        {
            PaladinHp -= 17.7f;
            healthBar.value = PaladinHp;
            anim.SetBool("UnderAttack", true);
            //Debug.Log("Got hit!");
            if (PaladinHp <= 0.0f)
            {
                anim.SetBool("isRunning", false);
                anim.SetBool("isDead", true);
                
                isDead = true;
                Erika.GetComponent<CharacterStats>().count += 1;
                Destroy(Paladin, 3f);
                //this.gameObject.SetActive(false);
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        anim.SetBool("UnderAttack", false);
    }

    // Update is called once per frame
    void Update()
    {
        
        Stamina = Paladin.GetComponent<StaminaController>().stamina.value;
        //Stamina = Paladin.GetComponent<StaminaController>().currentStaminaErad;
        distance = Paladin.GetComponent<Distance>().distance;
        double[,] DoMem = Paladin.GetComponent<Arrays>().PaladinDom;
        string[,] currentState = Paladin.GetComponent<Arrays>().PaladinCurrentState;
        string[,] PaladinStates = Paladin.GetComponent<Arrays>().PaladinFuzzyRules;
        string[] hpRule = Paladin.GetComponent<Arrays>().PaladinHpRule;
        string[] staminaRule = Paladin.GetComponent<Arrays>().PaladinStaminaRule;
        string[] distanceRule = Paladin.GetComponent<Arrays>().PaladinDistanceRule;
        double[] hpDom = Paladin.GetComponent<Arrays>().PaladinHpDom;
        double[] staminaDom = Paladin.GetComponent<Arrays>().PaladinStaminaDom;
        double[] distanceDom = Paladin.GetComponent<Arrays>().PaladinDistanceDom;

        HealthPoints.text = PaladinHp.ToString();
        DistanceValue.text = distance.ToString();
        StaminaPoints.text = Stamina.ToString();

        dom1 = VeryLowHp(PaladinHp);
        if (dom1 >= 0 & dom1 <= 1)
        {
            DoMem[0, 0] = dom1;
            currentState[0, 0] = "VeryLow";
        }
        else
        {
            DoMem[0, 0] = 0f;
            currentState[0, 0] = "";
        }

        dom2 = LowHp(PaladinHp);
        if (dom2 >= 0 & dom2 <= 1)
        {
            DoMem[0, 1] = dom2;
            currentState[0, 1] = "Low";
        }
        else
        {
            DoMem[0, 1] = 0f;
            currentState[0, 1] = "";
        }

        dom3 = MediumHp(PaladinHp);
        if (dom3 >= 0 & dom3 <= 1)
        {
            DoMem[0, 2] = dom3;
            currentState[0, 2] = "Medium";
        }
        else
        {
            DoMem[0, 2] = 0f;
            currentState[0, 2] = "";
        }

        dom4 = HighHp(PaladinHp);
        if (dom4 >= 0 & dom4 <= 1)
        {
            DoMem[0, 3] = dom4;
            currentState[0, 3] = "High";
        }
        else
        {
            DoMem[0, 3] = 0f;
            currentState[0, 3] = "";
        }


        //Health Drop Rate (bleeding)

        dom5 = VeryLowStamina(Stamina);
        if (dom5 >= 0 & dom1 <= 1)
        {
            DoMem[1, 0] = dom5;
            currentState[1, 0] = "VeryLow";
        }
        else
        {
            DoMem[1, 0] = 0f;
            currentState[1, 0] = "";
        }

        dom6 = LowStamina(Stamina);
        if (dom6 >= 0 & dom6 <= 1)
        {
            DoMem[1, 1] = dom6;
            currentState[1, 1] = "Low";
        }
        else
        {
            DoMem[1, 1] = 0f;
            currentState[1, 1] = "";
        }

        dom7 = MediumStamina(Stamina);
        if (dom7 >= 0 & dom7 <= 1)
        {
            DoMem[1, 2] = dom7;
            currentState[1, 2] = "Medium";
        }
        else
        {
            DoMem[1, 2] = 0f;
            currentState[1, 2] = "";
        }

        dom8 = HighStamina(Stamina);
        if (dom8 >= 0 & dom8 <= 1)
        {
            DoMem[1, 3] = dom8;
            currentState[1, 3] = "High";
        }
        else
        {
            DoMem[1, 3] = 0f;
            currentState[1, 3] = "";
        }

        //Target Distance
        dom9 = InRange(distance);
        if(dom9 >= 0.0 & dom9 <= 1.0)
        {
            DoMem[2, 0] = dom9;
            currentState[2, 0] = "InRange";
        }
        else
        {
            DoMem[2, 0] = 0f;
            currentState[2, 0] = "";
        }

        dom10 = Close(distance);
        if (dom10 >= 0.0 & dom10 <= 1.0)
        {
            DoMem[2, 1] = dom10;
            currentState[2, 1] = "Close";
        }
        else
        {
            DoMem[2, 1] = 0f;
            currentState[2, 1] = "";
        }

        dom11 = Far(distance);
        if (dom11 >= 0.0 & dom11 <= 1.0)
        {
            DoMem[2, 2] = dom11;
            currentState[2, 2] = "Far";
        }
        else
        {
            DoMem[2, 2] = 0f;
            currentState[2, 2] = "";
        }

        dom12 = VeryFar(distance);
        if (dom12 >= 0.0 & dom12 <= 1.0)
        {
            DoMem[2, 3] = dom12;
            currentState[2, 3] = "VeryFar";
        }
        else
        {
            DoMem[2, 3] = 0f;
            currentState[2, 3] = "";
        }

        int m = 0;
        for (int j = 0; j <= 3; j++)
        {
              if (currentState[0, j] != "")
              {
                  hpRule[m] = currentState[0, j];
                  currentState[0, j] = "";
                  hpDom[m] = DoMem[0, j];
                  DoMem[0, j] = 0.0f;
                  m++;
              }
        }
        int k = 0;
        for (int j = 0; j <= 3; j++)
        {
             if (currentState[1, j] != "")
             {
                staminaRule[k] = currentState[1, j];
                currentState[1, j] = "";
                staminaDom[k] = DoMem[1, j];
                DoMem[1, j] = 0.0f;
                k++;
             }
        }
        int l = 0;
        for (int j = 0; j <= 3; j++)
        {
             if (currentState[2, j] != "")
             {
                distanceRule[l] = currentState[2, j];
                currentState[2, j] = "";
                distanceDom[l] = DoMem[2, j];
                DoMem[2, j] = 0.0f;
                l++;
            }
        }

        for (int i = 0; i < 64; i++)
        {
            if (PaladinStates[i, 0] == hpRule[0] & PaladinStates[i, 1] == staminaRule[0] & PaladinStates[i, 2] == distanceRule[0])
            {
                outputRule1 = PaladinStates[i, 3];
                Decision1.text = outputRule1;
            }
            if (PaladinStates[i, 0] == hpRule[1] & PaladinStates[i, 1] == staminaRule[1] & PaladinStates[i, 2] == distanceRule[1])
            {
                outputRule2 = PaladinStates[i, 3];
                Decision2.text = outputRule2;
            }
        }

        DomRule1 = MinValue(hpDom[0], staminaDom[0], distanceDom[0]);
        DomRule2 = MinValue(hpDom[1], staminaDom[1], distanceDom[1]);


        TotalOutput = MaxValue(DomRule1, DomRule2);
        //List<string> Output = new List<string>();

        //Output.Clear();

        if (TotalOutput == DomRule1)
        {
            if (outputRule1 == "Retreat")
            {
                outputState = RetreatState;
                State.text = outputRule1;
            }
            else if (outputRule1 == "Block")
            {
                outputState = BlockState;
                State.text = outputRule1;
            }
            else if (outputRule1 == "Searching")
            {
                outputState = SearchState;
                State.text = outputRule1;
            }
            else if (outputRule1 == "Hunting")
            {
                outputState = HuntState;
                State.text = outputRule1;
            }
            else if (outputRule1 == "Attack")
            {
                outputState = AttackState;
                State.text = outputRule1;
            }
        }
        else if (TotalOutput == DomRule2)
        {
            if (outputRule2 == "Retreat")
            {
                outputState = RetreatState;
                State.text = outputRule2;
            }
            else if (outputRule2 == "Block")
            {
                outputState = BlockState;
                State.text = outputRule2;
            }
            else if (outputRule2 == "Searching")
            {
                outputState = SearchState;
                State.text = outputRule2;
            }
            else if (outputRule2 == "Hunting")
            {
                outputState = HuntState;
                State.text = outputRule2;
            }
            else if (outputRule2 == "Attack")
            {
                outputState = AttackState;
                State.text = outputRule2;
            }
        }
    }

    

    public double MinValue(double Dom1, double Dom2, double dom3)
    {
        double minV;
        if (Dom1 <= Dom2)
        {
            minV = Dom1;
        }
        else
        {
            minV = Dom2;
        }
        if (minV >= dom3)
        {
            minV = dom3;
        }
        return minV;
    }

    public double MaxValue(double Dom1, double Dom2)
    {
        double maxV;
        if (Dom1 > Dom2)
        {
            maxV = Dom1;
        }
        else
        {
            maxV = Dom2;
        }
        return maxV;
    }

    //Health Membership Functions
    public double VeryLowHp(float PaladinHp)
    {
        if (PaladinHp > 0.0 & PaladinHp <= 30.0)
        {
            double dom = (30.0 - PaladinHp) / 30.0;

            return dom;
        }
        return -1.0f;

    }
    public double LowHp(float PaladinHp)
    {
        double dom;
        if (PaladinHp >= 20.0 & PaladinHp <= 40.0)
        {
            dom = (PaladinHp - 20.0) / 20.0;

            return dom;
        }
        else if (PaladinHp >= 40.0 & PaladinHp <= 60.0)
        {
            dom = (60.0 - PaladinHp) / 20.0;

            return dom;
        }
        return -1.0f;
    }
    public double MediumHp(float PaladinHp)
    {
        double dom;
        if (PaladinHp >= 50.0 & PaladinHp <= 65.0)
        {
            dom = (PaladinHp - 50.0) / 15.0;

            return dom;
        }
        else if (PaladinHp >= 65.0 & PaladinHp <= 80.0)
        {
            dom = (80.0 - PaladinHp) / 15.0;


            return dom;
        }
        return -1.0f;
    }
    public double HighHp(float PaladinHp)
    {
        if (PaladinHp > 70.0 & PaladinHp <= 100.0)
        {
            double dom = (PaladinHp - 70.0) / 30.0;

            return dom;
        }
        return -1.0f;

    }

    //Bleeding (Health's drop rate) Membership Functions
    public double VeryLowStamina(float stamina)
    {
        if (stamina >= 0.0 & stamina <= 30.0)
        {
            double dom = (30.0 - stamina) / 30.0;

            return dom;
        }
        return -1.0f;

    }
    public double LowStamina(float stamina)
    {
        double dom;
        if (stamina >= 20.0 & stamina <= 40.0)
        {
            dom = (stamina - 20.0) / 20.0;

            return dom;
        }
        else if (stamina >= 40.0 & stamina <= 60.0)
        {
            dom = (60.0 - stamina) / 20.0;

            return dom;
        }
        return -1.0f;
    }
    public double MediumStamina(float stamina)
    {
        double dom;
        if (stamina >= 50.0 & stamina <= 65.0)
        {
            dom = (stamina - 50.0) / 15.0;

            return dom;
        }
        else if (stamina >= 65.0 & stamina <= 80.0)
        {
            dom = (80.0 - stamina) / 15.0;

            return dom;
        }
        return -1.0f;
    }
    public double HighStamina(float stamina)
    {
        if (stamina > 70.0 & stamina <= 100.0)
        {
            double dom = (stamina - 70.0) / 30.0;

            return dom;
        }
        return -1.0f;

    }

    //Distance between targets Membership Functions
    public double InRange(float distance)
    {
        if (distance >= 0.0 & distance <= 5.0)
        {
            double dom = (5.0 - distance) / 5.0;
            return dom;
        }
        return -1.0f;
    }
    public double Close(float distance)
    {
        double dom;
        if (distance >= 4.0 & distance <= 17.0)
        {
            dom = (distance - 4.0) / 13.0;
            return dom;
        }
        else if (distance >= 17.0 & distance <= 30.0)
        {
            dom = (30.0 - distance) / 13.0;
            return dom;
        }
        return -1.0f;
    }
    public double Far(float distance)
    {
        double dom;
        if (distance >= 20.0 & distance <= 40.0)
        {
            dom = (distance - 20.0) / 20.0;
            return dom;
        }
        else if (distance >= 40.0 & distance <= 60.0)
        {
            dom = (60.0 - distance) / 20.0;
            return dom;
        }
        return -1.0f;
    }
    public double VeryFar(float distance)
    {
        double dom;
        if (distance >= 50.0 & distance <= 100.0)
        {
            dom = (distance - 50.0) / 50.0;
            return dom;
        }
        return -1.0;
    }
}
