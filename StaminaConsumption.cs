using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaConsumption : MonoBehaviour
{
 
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //StaminaController.instance.useStamina(15.0f);
        }
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    StaminaController.instance.useStamina(25.0f);
        //}
    }
}
