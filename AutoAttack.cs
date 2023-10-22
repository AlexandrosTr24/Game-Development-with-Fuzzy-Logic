using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttack : MonoBehaviour
{
    [SerializeField]
    public Transform arcAttack;
    [SerializeField]
    public GameObject autoAttack;

    public Animator anim;
    public TrailRenderer trailEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

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

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(autoAttack, 0.1f);
    }

    private void arrowInstance()
    {
        

        var clone = Instantiate(autoAttack, arcAttack.position, arcAttack.rotation) as GameObject;
        clone.GetComponent<Rigidbody>().AddForce(clone.transform.forward * 50, ForceMode.Impulse);

        var trail = Instantiate(trailEffect, arcAttack.position, arcAttack.rotation);
        trail.GetComponent<Rigidbody>().AddForce(clone.transform.forward * 50, ForceMode.Impulse);

        

    }
}
//����� �� ������� 
//stats abe
//paladin sensors
//abe movement
//�� �������� � �������������� ���������
//�������� �� state ��� warrok
//1.1+1.2 ������� 
//�������
//����� ������
//���������� ��������
//3� �������� ��������������
//3.2 -> cinemachine
//������������
//���� ��� ���� ��� ��� 
//������� ��� ��������
//����������� ����� ��� ������� + ��� ����� 
//demo ���� ��� screenshot
//��� 3 ����� �� ������� ������� ��� 
//scripts �� ���������