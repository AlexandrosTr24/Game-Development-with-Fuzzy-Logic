using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject Erika;
    public GameObject Erad;
    public GameObject Raylen;
    public GameObject Ollenur;
    int abilityUse = 1;

    public float TargetDistance;
    public float allowedDist;
    public GameObject Abe;
    public float FollowSpeed;
    public RaycastHit Shot;
    public Animator anim;
    public Transform[] moveSpots;
    private int randomSpot;
    private float waitTime;
    public float startWaitTime;
    public Transform spotTarget;
    private Transform initialTransform;

    void Start()
    {
        initialTransform = transform;
        transform.position = Vector3.MoveTowards(transform.position, Erika.transform.position, FollowSpeed);
        //spotTarget = moveSpots[randomSpot];
    }



    // Update is called once per frame
    void Update()
    {
        
        if (Abe.GetComponent<Support>().isDead == true)
        {
            FollowSpeed = 0;
            return;
        }
        transform.LookAt(Erika.transform);
        if (Vector3.Distance(Erika.transform.position, Abe.transform.position) >= TargetDistance)
        {
            if (Vector3.Distance(Erika.transform.position, Abe.transform.position) >= allowedDist)
            {
                anim.SetBool("isWalking", false);
                anim.SetBool("isCasting", false);
                anim.SetBool("isWalking", true);
                FollowSpeed = 0.1f;
                transform.position = Vector3.MoveTowards(transform.position, Erika.transform.position, FollowSpeed);
                //if (Abe.GetComponent<Support>().isCasting == false)
                //{
                    
                    
                //}
            }
            else
            {
                anim.SetBool("isWalking", false);
                FollowSpeed = 0f;
            }
        }
        transform.eulerAngles = new Vector3(0f, initialTransform.eulerAngles.y, 0f);
    }
}
