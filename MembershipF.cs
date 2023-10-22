using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;


public class MembershipF : MonoBehaviour
{
    public GameObject Erika;
    public GameObject Abe;

    public bool Heal;
    public bool Attack;
    public bool Shield;

    public Text HealthPoints;
    public Text BleedPoints;
    public Text Decision1;
    public Text Decision2;
    public Text State;

    double dom1 = -1f;
    double dom2 = 0f;
    double dom3 = 0f;
    double dom4 = 0f;
    double dom5 = 0f;
    double dom6 = 0f;
    double dom7 = 0f;
    double dom8 = 0f;
    string outputRule1;
    string outputRule2;
    double DomRule1;
    double DomRule2;
    double TotalOutput;

    public void Awake()
    {
        Erika = GameObject.FindGameObjectWithTag("Player");
        Abe = GameObject.FindGameObjectWithTag("Support");
        float health = Erika.GetComponent<CharacterStats>().currenthp;
    }

    
    


    

    private void Start()
    {
        float health = Erika.GetComponent<CharacterStats>().currenthp;
        float bleed = Erika.GetComponent<CharacterStats>().bleeding;
        double[,] DoMem = Abe.GetComponent<Arrays>().DoMem;

        Decision2.text = "Shield";
        
    }
    

    void Update()
    {
        float health = Erika.GetComponent<CharacterStats>().currenthp;
        float bleed = Erika.GetComponent<CharacterStats>().bleeding;

        HealthPoints.text = health.ToString();
        BleedPoints.text = bleed.ToString();

        double[,] DoMem = Abe.GetComponent<Arrays>().DoMem; //keeps all the degrees of membership based on the current crisp values
        string[,] currentState = Abe.GetComponent<Arrays>().currentState; //keeps all the possible states based on the current crisp values
        string[,] States = Abe.GetComponent<Arrays>().States; //keeps the fuzzy rules
        string[] hpRule = Abe.GetComponent<Arrays>().hpRule; //keeps the fuzzy set based on the current hp
        string[] bleedRule = Abe.GetComponent<Arrays>().bleedRule; //keeps the fuzzy set based on the current bleed
        double[] hpDom = Abe.GetComponent<Arrays>().hpDom; //keeps the dom of the current hp for each fuzzy set
        double[] bleedDom = Abe.GetComponent<Arrays>().bleedDom; //keeps the dom of the current bleed for each fuzzy set

        Heal = false;
        Shield = false;

        //DoM calculation 
        //Health (hp)
        if(health < 0)
        {
            health = 1;
        }
        else if (health > 100)
        {
            health = 100;
        }

        dom1 = VeryLow(health);
        if (dom1 >= 0 & dom1 <= 1)
        {
            DoMem[0,0] = dom1;
            currentState[0,0] = "VeryLow";
        }
        else
        {
            DoMem[0, 0] = 0f;
            currentState[0, 0] = "";
        }

        dom2 = Low(health);
        if (dom2 >= 0 & dom2 <= 1)
        {
            DoMem[0,1] = dom2;
            currentState[0, 1] = "Low";
        }
        else
        {
            DoMem[0, 1] = 0f;
            currentState[0, 1] = "";
        }

        dom3 = Medium(health);
        if (dom3 >= 0 & dom3 <= 1)
        {
            DoMem[0,2] = dom3;
            currentState[0, 2] = "Medium";
        }
        else
        {
            DoMem[0, 2] = 0f;
            currentState[0, 2] = "";
        }

        dom4 = High(health);
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

        dom5 = VeryLowDropRate(bleed);
        if (dom5 >= 0 & dom1 <= 1)
        {
            DoMem[1,0] = dom5;
            currentState[1, 0] = "VeryLow";
        }
        else
        {
            DoMem[1, 0] = 0f;
            currentState[1, 0] = "";
        }

        dom6 = LowDropRate(bleed);
        if (dom6 >= 0 & dom6 <= 1)
        {
            DoMem[1,1] = dom6;
            currentState[1, 1] = "Low";
        }
        else
        {
            DoMem[1, 1] = 0f;
            currentState[1, 1] = "";
        }

        dom7 = MediumDropRate(bleed);
        if (dom7 >= 0 & dom7 <= 1)
        {
            DoMem[1,2] = dom7;
            currentState[1, 2] = "Medium";
        }
        else
        {
            DoMem[1, 2] = 0f;
            currentState[1, 2] = "";
        }

        dom8 = HighDropRate(bleed);
        if (dom8 >= 0 & dom8 <= 1)
        {
            DoMem[1,3] = dom8;
            currentState[1, 3] = "High";
        }
        else
        {
            DoMem[1, 3] = 0f;
            currentState[1, 3] = "";
        }

        //Keep the states to check the correspinding rule
        //for (int i = 0; i <= 1; i++)
        //{
            int k = 0;
            for (int j = 0; j <= 3; j++)
            {
                if (currentState[0, j] != "")
                {

                    hpRule[k] = currentState[0, j];
                    currentState[0, j] = "";
                    hpDom[k] = DoMem[0, j];
                    DoMem[0, j] = 0.0f;
                    k++;
                }
            }
        //}


        //for (int i = 0; i <= 1; i++)
        //{
            int m = 0;
            for (int j = 0; j <= 3; j++)
            {
                if (currentState[1, j] != "")
                {
                    bleedRule[m] = currentState[1, j];
                    currentState[1, j] = "";
                    bleedDom[m] = DoMem[1, j];
                    DoMem[1, j] = 0.0f;
                    m++;
                }
            }
        //}
         


        //Find the two possible outcomes
        for(int i = 0; i < 16; i++)
        {
           if (States[i, 0] == hpRule[0] & States[i,1] == bleedRule[0]) {
                outputRule1 = States[i,2];
                Decision1.text = outputRule1;
            }
           if (States[i, 0] == hpRule[1] & States[i, 1] == bleedRule[1])
           {
                outputRule2 = States[i, 2];
                Decision2.text = outputRule1;
            }
        }

        DomRule1 = MinValue(hpDom[0], bleedDom[0]);
        DomRule2 = MinValue(hpDom[1], bleedDom[1]);

        TotalOutput = MaxValue(DomRule1, DomRule2);
        
        
        if (TotalOutput == DomRule1)
        {
            if (outputRule1 == "Heal")
            {
                Heal = true;
                State.text = outputRule1;
            }
            else if (outputRule1 == "Shield")
            {
                Shield = true;
                State.text = outputRule1;
            }
        }
        else if (TotalOutput == DomRule2) 
        {
            if (outputRule2 == "Heal")
            {
                Heal = true;
                State.text = outputRule2;
            }
            else if (outputRule2 == "Shield")
            {
                Shield = true;
                State.text = outputRule2;
            }
        }
    }




    //Output Degree of Membership calculation
    public double MinValue(double Dom1, double Dom2)
    {
        double minV;
        if (Dom1 < Dom2)
        {
            minV = Dom1;
        }
        else
        {
            minV = Dom2;
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
    public double VeryLow(float health)
    {
        if (health > 0.0 & health <= 30.0)
        {
            double dom = (30.0 - health) / 30.0;
            
            return dom;
        }
        return -1.0f;
        
    }

    public double Low(float health)
    {
        double dom;
        if(health >= 20.0 & health <= 40.0)
        {
            dom = (health - 20.0) / 20.0;
           
            return dom;
        }
        else if (health >= 40.0 & health <= 60.0)
        {
            dom = (60.0 - health) / 20.0;
            
            return dom;
        }
        return -1.0f;
    }

    public double Medium(float health)
    {
        double dom;
        if (health >= 50.0 & health <= 65.0)
        {
            dom = (health - 50.0) / 15.0;
            
            return dom;
        }
        else if (health >= 65.0 & health <= 80.0)
        {
            dom = (80.0 - health) / 15.0;
            

            return dom;
        }
        return -1.0f;
    }

    public double  High(float health)
    {
        if (health > 70.0 & health <= 100.0) {
            double dom = (health - 70.0) / 30.0;
            
            return dom;
        }
        return -1.0f;
        
    }


    //Bleeding (Health's drop rate) Membership Functions
    public double VeryLowDropRate(float bleed)
    {
        if (bleed >= 0.0 & bleed <= 30.0)
        {
            double dom = (30.0 - bleed) / 30.0;
            
            return dom;
        }
        return -1.0f;

    }

    public double LowDropRate(float bleed)
    {
        double dom;
        if (bleed >= 20.0 & bleed <= 40.0)
        {
            dom = (bleed - 20.0) / 20.0;
            
            return dom;
        }
        else if (bleed >= 40.0 & bleed <= 60.0)
        {
            dom = (60.0 - bleed) / 20.0;

            return dom;
        }
        return -1.0f;
    }

    public double MediumDropRate(float bleed)
    {
        double dom;
        if (bleed >= 50.0 & bleed <= 65.0)
        {
            dom = (bleed - 50.0) / 15.0;

            return dom;
        }
        else if (bleed >= 65.0 & bleed <= 80.0)
        {
            dom = (80.0 - bleed) / 15.0;

            return dom;
        }
        return -1.0f;
    }

    public double HighDropRate(float bleed)
    {
        if (bleed > 70.0 & bleed <= 100.0)
        {
            double dom = (bleed - 70.0) / 30.0;
            
            return dom;
        }
        return -1.0f;

    }



}

