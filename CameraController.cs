using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CharacterController controller; //Component that drives the player
    public Transform cameraDir;

    public float speed = 6f;
    public float smoothTurn = 0.1f;
    public float rotationSpeed = 10f;
    float turnVelocity;

    public float mouseSensitivity;

    public Animator anim;
    public Rigidbody rigidBody;
    

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        //normalize so that when I press 2 keys at the same time I don't go faster
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized; 

        if (direction.magnitude >= float.Epsilon)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + cameraDir.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, smoothTurn);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            float cameraRotation = cameraDir.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0, cameraRotation, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        } 
    }
}

