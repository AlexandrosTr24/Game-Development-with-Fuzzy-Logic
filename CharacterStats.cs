using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor.SearchService;
using System.Threading;

public class CharacterStats : MonoBehaviour
{
    public float maxHealthP = 100f;
    public float currenthp { get;  set; }

    public bool pause;


    public Stats damage;
    public Stats armor;
    public float bleeding = 0;

    public int count = 0;
    
    public Material hpMaterial;
    public Animator anim;

    public GameObject Abe;
    public GameObject Paladin;
    public GameObject Raylen;
    public GameObject Ollenur;

    public float lastAttack;
    public float attackCd = 3.0f;

    public bool dead;
    //public bool damaging;

    public List<int> bleedTicks = new List<int>();

    public float restartDelay = 5f;
    public float loadDelay = 5f;
    //Shader Healthbar;

    public void Awake()
    {
        currenthp = maxHealthP;
    }
    private void Start()
    {
        hpMaterial.SetFloat("_Health", 10f);
    }

    public void OnCollisionEnter(Collision collisioninfo)
    {
        Debug.Log("Collision detected");
        if (collisioninfo.collider.tag == "Weapon")
        {
            
            if (Paladin.GetComponent<PaladinStateManager>().damaging == true)
            {
                Debug.Log("Hit detected");
                TakeDamagePaladin(12);
                
                hpMaterial.SetFloat("_Health", currenthp / 10);
                
            }
            else
            {
                
            }
            if (Raylen.GetComponent<PaladinStateManager>().damaging == true)
            {
                Debug.Log("Hit detected");
                TakeDamageRaylen(17);
                
                hpMaterial.SetFloat("_Health", currenthp / 10);
                
            }
            
            if (Ollenur.GetComponent<PaladinStateManager>().damaging == true)
            {
                Debug.Log("Hit detected");
                TakeDamageOllenur(23);
                
                hpMaterial.SetFloat("_Health", currenthp / 10);
                

            }
            
        }
    }

    void OnCollisionExit(Collision collision)
    {
        anim.SetBool("incoming", false);
    }

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

        int points = Abe.GetComponent<Support>().hpUpdate;
        bool Heal = Abe.GetComponent<MembershipF>().Heal;
        
        if(count == 3)
        {
            NewScene();
        }
        
        if (Heal == true)
        {
            
            currenthp += points;
            
            hpMaterial.SetFloat("_Health", currenthp/10);


        }


    }

    public int TakeDamagePaladin(int damage)
    {
        if (Abe.GetComponent<Support>().renderShield.enabled == true)
        {
            //Debug.Log("No damage taken");
            return 0;
        }
        anim.SetBool("incoming", true);
        lastAttack = Time.time;
        bleeding += 11f;
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);


        if (currenthp - damage <= 0)
        {
            Debug.Log("Zero Hp ");
            Die();
            currenthp = 0;
            
        }
        else if(currenthp - damage > 0)
        {
            //Debug.Log(transform.name + "takes" + damage + "damage.");
            
            currenthp -=  damage;
            
            


            Debug.Log(currenthp);
        }

        
        BleedingEffect(3);
        return 0;
    }

    public int TakeDamageRaylen(int damage)
    {
        if ( Abe.GetComponent<Support>().renderShield.enabled == true)
        {
            //Debug.Log("No damage taken");
            return 0;
        }
        anim.SetBool("incoming", true);
        lastAttack = Time.time;
        bleeding += 7f;
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);


        if (currenthp - damage <= 0)
        {
            Debug.Log("Zero Hp " + currenthp);
            Die();
            currenthp = 0;
        }
        else if (currenthp - damage > 0)
        {
            Debug.Log(transform.name + "takes" + damage + "damage.");
            //
            currenthp -= damage;

            //Debug.Log(currenthp);
        }


        BleedingEffect(5);
        return 0;
    }

    public int TakeDamageOllenur(int damage)
    {
        if ( Abe.GetComponent<Support>().renderShield.enabled == true)
        {
            //Debug.Log("No damage taken");
            return 0;
        }
        anim.SetBool("incoming", true);
        lastAttack = Time.time;
        bleeding += 5f;
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);


        if (currenthp - damage <= 0)
        {
            Debug.Log("Zero Hp " + currenthp);
            Die();
            currenthp = 0;
        }
        else if (currenthp - damage > 0)
        {
            Debug.Log(transform.name + "takes" + damage + "damage.");
            //
            currenthp -= damage;

            //Debug.Log(currenthp);
        }


        BleedingEffect(7);
        return 0;
    }

    public void BleedingEffect(int ticks)
    {
        if (bleedTicks.Count <= 0)
        {
            bleedTicks.Add(ticks);
            StartCoroutine(Bleed());
            
        }
    }
    IEnumerator Bleed()
    {
        

        while (bleedTicks.Count > 0)
        {
            for(int i=0;  i < bleedTicks.Count; i++)
            {
                bleedTicks[i]--;
                bleeding -= 1f;
            }
            currenthp -= bleeding/20;
            bleedTicks.RemoveAll(i => i == 0);
            Debug.Log("bleeding");
            yield return new WaitForSeconds(0.5f);
        }
        

    }


    public virtual void Die()
    {
        Debug.Log(transform.name + "died.");
        anim.SetBool("isDead", true);
        Invoke("Restart", restartDelay);
    }

    //Load new scene in case of victory 
    public virtual void NewScene()
    {
        Debug.Log("Village cleared");
        StartCoroutine(LoadSceneWithDelay());
   
    }
    IEnumerator LoadSceneWithDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(loadDelay);

        // Load the new scene
        SceneManager.LoadScene(1);
    }

    //Restart the scene if defeated
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
