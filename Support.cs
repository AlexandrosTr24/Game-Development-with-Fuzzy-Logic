using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Support : MonoBehaviour
{

    public GameObject Erika;
    public GameObject shield;
    public GameObject heal;
    public GameObject Abe;

    public Animator anim;

    //public Material hpMaterialAbe;
    public Slider healthBarAbe;

    public Renderer renderShield;
    public Renderer renderHeal;
    public float shieldCd = 16.5f;
    public float healCd = 20f;
    public float shieldDuration = 0f;
    public float healDuration = 0f;
    public float lastHeal;
    public float lastShield;
    public int hp_new;
    public int hpUpdate;
    public bool isCasting = false;
    public float AbeHp { get; set; }
    public float maxHealthP = 100f;
    public bool isDead = false;

    public GameObject Paladin;
    public GameObject Raylen;
    public GameObject Ollenur;

    private void Awake()
    {
        Abe = GameObject.FindGameObjectWithTag("Support");
        Erika = GameObject.FindGameObjectWithTag("Player");
        shield = Erika.transform.GetChild(1).gameObject;
        heal = Erika.transform.GetChild(0).gameObject;
        AbeHp = maxHealthP;
    }

    public void Start()
    {
        renderShield = shield.GetComponent<RenderActivate>().rend;
        renderShield.enabled = false;
        renderHeal = heal.GetComponent<RenderActivate>().rend;
        renderHeal.enabled = false;

    }

    public void OnCollisionEnter(Collision collisioninfo)
    {
        //Debug.Log("Collision detected");
        if (collisioninfo.collider.tag == "Weapon")
        {

            if (Paladin.GetComponent<PaladinStateManager>().damaging == true)
            {
                Debug.Log("Hit detected");
                AbeHp -= 18;
                healthBarAbe.value = AbeHp / 100;
                //hpMaterialAbe.SetFloat("_Health", AbeHp / 10);

            }
            else
            {

            }
            if (Raylen.GetComponent<PaladinStateManager>().damaging == true)
            {
                Debug.Log("Hit detected");
                AbeHp -= 25;
                healthBarAbe.value = AbeHp / 100;
                //hpMaterialAbe.SetFloat("_Health", AbeHp / 10);

            }

            if (Ollenur.GetComponent<PaladinStateManager>().damaging == true)
            {
                Debug.Log("Hit detected");
                AbeHp -= 20;
                healthBarAbe.value = AbeHp / 100;
                //hpMaterialAbe.SetFloat("_Health", AbeHp / 10);


            }

        }
        if (AbeHp <= 0)
        {
            Die();
        }
    }

    // Update is called once per frame
    void Update()


    {
        bool Heal = Abe.GetComponent<MembershipF>().Heal;
        bool Attack = Abe.GetComponent<MembershipF>().Attack;
        bool Shield = Abe.GetComponent<MembershipF>().Shield;
        float hp = Erika.GetComponent<CharacterStats>().currenthp;

        if ((renderHeal.enabled == true) & (Time.time - lastHeal >= 1.0))
        {
            anim.SetBool("isCasting", false);
            isCasting = false;
            renderHeal.enabled = false; 
        }
        if ((renderShield.enabled == true) & (Time.time - lastShield >= 8.25))
        {
            anim.SetBool("isCasting", false);
            isCasting = false;
            renderShield.enabled = false;
        }
        if (Heal == true)
        {
            
            hpUpdate = HealCharacter(35);
            

        }
        if(Shield == true)
        {
            ShieldCharacter();
            Shield = false;
            
        }
        
        
        

    }

    public virtual void Die()
    {
        anim.SetBool("isDead", true);
        isDead = true;
        Destroy(Abe, 3f);
    }

    public int HealCharacter(int recover)
    {
        
        if (Time.time - lastHeal < healCd)
        {
            return 0;
        }
        anim.SetBool("isCasting", true);
        isCasting = true;
        lastHeal = Time.time;
        renderHeal.enabled = true;
        
        hp_new = recover;
        return hp_new;
 
    } 
    public void ShieldCharacter()
    {
        
        if (Time.time - lastShield < shieldCd)
        {
            return;
        }
        anim.SetBool("isCasting", true);
        isCasting = true;
        lastShield = Time.time;
        renderShield.enabled = true;
        Debug.Log("Shielding");

        Raylen.GetComponent<PaladinStateManager>().target = transform;
    }


}
