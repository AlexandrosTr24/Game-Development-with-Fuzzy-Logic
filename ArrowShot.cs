using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShot : MonoBehaviour
{
    [SerializeField]
    private Transform arrow;
    [SerializeField]
    private GameObject aimShot;

    public TrailRenderer trailEffect;
    public Animator anim;

    // Update is called once per frame
    void Update()
    {
        
    }

    void CollisionDetection(Collision info)
    {
        if (info.collider.tag == "Enemy")
        { 
            Debug.Log("Got hit!");
        }
    }

    private void arrowInstance()
    {
        var clone = Instantiate(aimShot, arrow.position, arrow.rotation) as GameObject;
        clone.GetComponent<Rigidbody>().AddForce(clone.transform.forward * 50, ForceMode.Impulse);

        var trail = Instantiate(trailEffect, arrow.position, arrow.rotation);
        trail.GetComponent<Rigidbody>().AddForce(clone.transform.forward * 50, ForceMode.Impulse);  
    }

    

    

}
