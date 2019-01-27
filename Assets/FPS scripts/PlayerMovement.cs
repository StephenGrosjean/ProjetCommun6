using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float lookSpeed, moveSpeed, jumpForce, mouseSensitivity;
    [SerializeField] private GameObject cam;
    [SerializeField] private Animator head;
    private Rigidbody rigid;

    

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update() {
      
        if(Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) {
            head.SetBool("isRunning", true);
        }
        else {
            head.SetBool("isRunning", false);
        }

        rigid.MovePosition(transform.position + (transform.forward * Input.GetAxis("Vertical") * moveSpeed) + (transform.right * Input.GetAxis("Horizontal") * moveSpeed));


        if (Input.GetKeyDown("space")) {
            rigid.AddForce(transform.up * jumpForce);
        }

        float z = transform.eulerAngles.z;
        float x = transform.eulerAngles.x;
        float y = cam.transform.eulerAngles.y;
        Vector3 desiredRot = new Vector3(x, y, z);

        transform.rotation = Quaternion.Euler(desiredRot);
    }



}
