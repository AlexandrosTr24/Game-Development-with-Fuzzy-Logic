using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceW : MonoBehaviour
{
    public GameObject Warrok;
    public GameObject Gandaulf;
    public float distance;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(Gandaulf.transform.position, Warrok.transform.position);
        //Erad.GetComponent<PaladinStateManager>().target
    }
}