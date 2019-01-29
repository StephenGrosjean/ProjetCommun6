using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAttach : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private Transform bone;

    // Start is called before the first frame update
    void Update()
    {
        transform.position = bone.transform.position;
    }

}
