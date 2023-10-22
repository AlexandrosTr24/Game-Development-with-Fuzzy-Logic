using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarrokStaminaController : MonoBehaviour
{
    public Slider stamina;
    public GameObject Warrok;
    

    public float maxStamina = 100.0f;
    public float currentStaminaWarrok;
    public Coroutine regen;
    public float value = 15f;

    public float lastAttack;
    public float attackCd;

    public WaitForSeconds regenTick = new WaitForSeconds(0.3f);
    public static WarrokStaminaController instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentStaminaWarrok = maxStamina;
        stamina.maxValue = maxStamina;
        stamina.value = maxStamina;

    }

    private void Update()
    {
        if (Warrok.GetComponent<WarrokStateManager>().damaging == true)
        {
            //useStaminaWarrok();

        }

    }

    private void useStaminaWarrok()
    {
        //if (Time.time - lastAttack < attackCd)
        //{
        //    return;
        //}
        if (currentStaminaWarrok - 10 >= 0)
        {
            currentStaminaWarrok -= 13.6f;
            stamina.value = currentStaminaWarrok;
            lastAttack = Time.time;
            if (regen != null)
            {
                StopCoroutine(regen);
            }
            regen = StartCoroutine(RegenStamina(currentStaminaWarrok));

        }
        else
        {
            //Debug.Log("Not enough stamina");
        }
        //return 0;
    }



    private IEnumerator RegenStamina(float currentStamina)
    {
        yield return new WaitForSeconds(2);
        while (currentStamina < maxStamina)
        {
            currentStamina += maxStamina / 50;
            stamina.value = currentStamina;
            yield return regenTick;
        }
    }

}

