using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class FuzzyController : MonoBehaviour
{
    public GameObject Ganfaul;
    public GameObject Warrok;
    public Slider healthBar;

    public float maxHealth;
    public float currentHealth;

    public float distance;

    public Text Decision1;
    public Text Decision2;
    public Text HealthPoints;
    public Text StaminaPoints;
    public Text DistanceValue;

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

    string outputRule1;
    string outputRule2;

    double DomRule1;
    double DomRule2;
    double TotalOutput;

    public bool isDead = false;

    public Animator anim;

    public float WarrokHp;
    public float Stamina;


    //Αλλαγή των state στα αντίστοιχα του Warrok
    public WarrokBaseState outputState;
    public AttackState AttackState; 
    public IdleState IdleState; 
    public RageState RageState;
    public HealState HealState;
    public DeathState DeathState;
    public LungeState LungeState;


    private void Awake()
    {
        //Erika = GameObject.FindGameObjectWithTag("Player");
        //Paladin = GameObject.FindGameObjectWithTag("Enemy");
        Stamina = Warrok.GetComponent<WarrokStaminaController>().stamina.value;
        //distance = Warrok.GetComponent<DistanceW>().distance;
        distance = Warrok.GetComponent<DistanceW>().distance;
        IdleState = Warrok.GetComponent<WarrokStateManager>().Idle;
        AttackState = Warrok.GetComponent<WarrokStateManager>().Attack;
        RageState = Warrok.GetComponent<WarrokStateManager>().Rage;
        HealState = Warrok.GetComponent<WarrokStateManager>().Heal;
        DeathState = Warrok.GetComponent<WarrokStateManager>().Death;
        LungeState = Warrok.GetComponent<WarrokStateManager>().Lunge;



    }

    private void OnCollisionEnter(Collision info)
    {

        if (info.collider.tag == "Arrow")
        {
            healthBar.value = Warrok.GetComponent<WarrokStats>().currentHealth;
            Warrok.GetComponent<WarrokStats>().currentHealth -= 23.6f;
            


            if (Warrok.GetComponent<WarrokStats>().currentHealth <= 0.0f)
            {
                anim.SetBool("isDead", true);
                isDead = true;
                Destroy(Warrok, 6f);

            }
        }
    }


    //Αλλαγή των ονομάτων των μεταβλητών
    //Variables needed
    //PaladinHp,PaladinStamina,ErikaHp

    // Start is called before the first frame update
    void Start()
    {
        WarrokHp = Warrok.GetComponent<WarrokStats>().maxHealth;
        Warrok.GetComponent<WarrokStats>().currentHealth = WarrokHp;

        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;

        Decision2.text = "Attack";
    }

    
    //private void OnCollisionExit(Collision collision)
    //{
    //    anim.SetBool("UnderAttack", false);
    //}

    // Update is called once per frame
    void Update()
    {

        Stamina = Warrok.GetComponent<WarrokStaminaController>().currentStaminaWarrok;
        distance = Warrok.GetComponent<DistanceW>().distance;
        double[,] DoMem = Warrok.GetComponent<Arrays>().WarrokDom;
        string[,] currentState = Warrok.GetComponent<Arrays>().WarrokCurrentState;
        string[,] WarrokStates = Warrok.GetComponent<Arrays>().WarrokFuzzyRules;
        string[] hpRule = Warrok.GetComponent<Arrays>().WarrokHpRule;
        string[] staminaRule = Warrok.GetComponent<Arrays>().WarrokStaminaRule;
        string[] distanceRule = Warrok.GetComponent<Arrays>().WarrokDistanceRule;
        double[] hpDom = Warrok.GetComponent<Arrays>().WarrokHpDom;
        double[] staminaDom = Warrok.GetComponent<Arrays>().WarrokStaminaDom;
        double[] distanceDom = Warrok.GetComponent<Arrays>().WarrokDistanceDom;

        if (Warrok.GetComponent<WarrokStats>().currentHealth < 0)
        {
            Warrok.GetComponent<WarrokStats>().currentHealth = 0;
        }

        HealthPoints.text = Warrok.GetComponent<WarrokStats>().currentHealth.ToString();
        DistanceValue.text = distance.ToString();
        StaminaPoints.text = Stamina.ToString();


        dom1 = VeryLowHp(Warrok.GetComponent<WarrokStats>().currentHealth);
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

        dom2 = LowHp(Warrok.GetComponent<WarrokStats>().currentHealth);
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

        dom3 = MediumHp(Warrok.GetComponent<WarrokStats>().currentHealth);
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

        dom4 = HighHp(Warrok.GetComponent<WarrokStats>().currentHealth);
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
        if (dom9 >= 0.0 & dom9 <= 1.0)
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
            if (WarrokStates[i, 0] == hpRule[0] & WarrokStates[i, 1] == staminaRule[0] & WarrokStates[i, 2] == distanceRule[0])
            {
                outputRule1 = WarrokStates[i, 3];
            }
            if (WarrokStates[i, 0] == hpRule[1] & WarrokStates[i, 1] == staminaRule[1] & WarrokStates[i, 2] == distanceRule[1])
            {
                outputRule2 = WarrokStates[i, 3];
            }
        }

        DomRule1 = MinValue(hpDom[0], staminaDom[0], distanceDom[0]);
        DomRule2 = MinValue(hpDom[1], staminaDom[1], distanceDom[1]);


        TotalOutput = MaxValue(DomRule1, DomRule2);
        //List<string> Output = new List<string>();

        //Output.Clear();

        if (TotalOutput == DomRule1)
        {
            if (outputRule1 == "Heal")
            {
                if(Warrok.GetComponent<WarrokStateManager>().healing == 1)
                {
                    outputState = HealState;
                    Decision1.text = outputRule1;
                }
                else if (Warrok.GetComponent<WarrokStateManager>().healing == 0)
                {
                    outputState = AttackState;
                    outputRule1 = "Attack";
                    Decision1.text = outputRule1.ToString();
                }
                
            }
            else if (outputRule1 == "Attack")
            {
                outputState = AttackState;
                Decision1.text = outputRule1;
            }
            else if (outputRule1 == "Rage")
            {
                outputState = RageState;
                Decision1.text = outputRule1;
            }
            else if (outputRule1 == "Death")
            {
                outputState = DeathState;
                Decision1.text = outputRule1;
            }
            else if (outputRule1 == "Lunge")
            {
                outputState = LungeState;
                Decision1.text = outputRule1;
            }
        }
        else if (TotalOutput == DomRule2)
        {
            if (outputRule2 == "Heal")
            {
                if (Warrok.GetComponent<WarrokStateManager>().healing == 1)
                {
                    outputState = HealState;
                    Decision1.text = outputRule1;
                }
                else if (Warrok.GetComponent<WarrokStateManager>().healing == 0)
                {
                    outputState = AttackState;
                    outputRule1 = "Attack";
                    Decision1.text = outputRule1.ToString();
                }
            }
            else if (outputRule2 == "Attack")
            {
                outputState = AttackState;
                Decision2.text = outputRule2;
            }
            else if (outputRule2 == "Rage")
            {
                outputState = RageState;
                Decision2.text = outputRule2;
            }
            else if (outputRule2 == "Death")
            {
                outputState = DeathState;
                Decision2.text = outputRule2;
            }
            else if (outputRule2 == "Lunge")
            {
                outputState = LungeState;
                Decision2.text = outputRule2;
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
    public double VeryLowHp(float WarrokHp)
    {
        if (WarrokHp > 0.0 & WarrokHp <= 30.0)
        {
            double dom = (30.0 - WarrokHp) / 30.0;

            return dom;
        }
        return -1.0f;

    }
    public double LowHp(float WarrokHp)
    {
        double dom;
        if (WarrokHp >= 20.0 & WarrokHp <= 40.0)
        {
            dom = (WarrokHp - 20.0) / 20.0;

            return dom;
        }
        else if (WarrokHp >= 40.0 & WarrokHp <= 60.0)
        {
            dom = (60.0 - WarrokHp) / 20.0;

            return dom;
        }
        return -1.0f;
    }
    public double MediumHp(float WarrokHp)
    {
        double dom;
        if (WarrokHp >= 50.0 & WarrokHp <= 65.0)
        {
            dom = (WarrokHp - 50.0) / 15.0;

            return dom;
        }
        else if (WarrokHp >= 65.0 & WarrokHp <= 80.0)
        {
            dom = (80.0 - WarrokHp) / 15.0;


            return dom;
        }
        return -1.0f;
    }
    public double HighHp(float WarrokHp)
    {
        if (WarrokHp > 70.0 & WarrokHp <= 100.0)
        {
            double dom = (WarrokHp - 70.0) / 30.0;

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
