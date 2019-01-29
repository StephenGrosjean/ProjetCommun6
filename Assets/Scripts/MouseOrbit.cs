using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOrbit : MonoBehaviour
{
    [SerializeField] private float xRot = 0f;
    [SerializeField] private float yRot = 0f;

    [SerializeField] private float distance = 5f;
    [SerializeField] private float sensitivity = 1000f;

    [SerializeField] private Transform target;
    [SerializeField] private float scrollSensitivity, moveSensitivity;

    private float rotationSpeed = 10;
    private Quaternion lookRotation;
    private Vector3 direction;

    private void Start() {
        yRot = 180;
    }
    

    void Update() {
        if(target.position.x > 28) {
            target.position = new Vector3(28, target.position.y, target.position.z);
        }
        if (target.position.x < 0) {
            target.position = new Vector3(0, target.position.y, target.position.z);
        }

        if (target.position.z > 20) {
            target.position = new Vector3(target.position.x, target.position.y, 20);
        }
        if (target.position.z < -20) {
            target.position = new Vector3(target.position.x, target.position.y, -20);
        }


        if (Input.GetMouseButton(1)) {
            //xRot += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
           
            yRot += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;


            if (xRot > 90f) {
                xRot = 90f;
            }
            else if (xRot < -90f) {
                xRot = -90f;
            }
        }

        if(Input.mouseScrollDelta.y > 0  && distance > 2) {
            distance -= Input.mouseScrollDelta.y * Time.deltaTime * scrollSensitivity;
        }
        else if(Input.mouseScrollDelta.y < 0 && distance < 24){
            distance += -Input.mouseScrollDelta.y * Time.deltaTime * scrollSensitivity;
        }

        xRot = -3.076f * distance - 13.846f;
        transform.position = target.position + Quaternion.Euler(xRot, yRot, 0f) * (distance * -Vector3.back);
        transform.LookAt(target.position, Vector3.up);

        direction = (new Vector3(target.position.x, transform.position.y, target.position.z) - transform.position).normalized;
        lookRotation = Quaternion.LookRotation(direction);
        target.rotation = Quaternion.Slerp(target.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        if (Input.GetAxis("Horizontal") != 0) {
            target.Translate(Vector3.right * Time.deltaTime * moveSensitivity * Input.GetAxis("Horizontal"), Space.Self);
        }
        if (Input.GetAxis("Vertical") != 0) {
            target.Translate(Vector3.forward * Time.deltaTime * moveSensitivity * Input.GetAxis("Vertical"), Space.Self); 
        }
    }
}

