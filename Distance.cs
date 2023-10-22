using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distance : MonoBehaviour
{
    public GameObject Erika;
    public GameObject Erad;
    public float distance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(Erad.GetComponent<PaladinStateManager>().target.transform.position, Erad.transform.position);
        //Erad.GetComponent<PaladinStateManager>().target
    }
}
