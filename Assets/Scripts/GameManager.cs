using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform[] wayPoints;
    public Transform[] WayPoints {
        get { return wayPoints; }
    }
}
