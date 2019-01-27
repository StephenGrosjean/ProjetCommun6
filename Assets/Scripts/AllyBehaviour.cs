﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AllyBehaviour : MonoBehaviour
{
    private enum states { combat, walking };

    [SerializeField] private float speed, attackTimer;
    [SerializeField] private int attack;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Transform currentWayPoint, nextWayPoint;
    [SerializeField] private GameObject currentEnemy;
    [SerializeField] private states currentState;

    [SerializeField] private Transform[] getWaypoints;
    [SerializeField] private List<Transform> waypoints;

    [SerializeField] private int life = 100;
    public int Life {
        get { return life; }
        set { life = value; }
    }

    private Rigidbody rigid;
    private BoxCollider collider;

    private int currentWayPointID;
    private bool canCheck = true;
    private float timer;

    private bool canAttack = true;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();

        getWaypoints = gameManager.WayPoints;
        foreach(Transform wp in getWaypoints) {
            waypoints.Add(wp);
        }
        waypoints.Reverse();
        
        currentWayPointID = 0;
        currentWayPoint = waypoints[currentWayPointID];
    }

    void Update()
    {
        if(currentEnemy == null) {
            collider.isTrigger = false;
            currentState = states.walking;
        }

        if(life <= 0) {
            Destroy(gameObject);
        }

        if (currentState == states.walking) {
            timer += 0.01f;
            float step = timer * speed;
            transform.position = Vector3.Lerp(waypoints[currentWayPointID].position, waypoints[currentWayPointID + 1].position, step);

            if (transform.position == waypoints[currentWayPointID + 1].position && canCheck && currentWayPointID + 2 != waypoints.Count) {
                canCheck = false;
                currentWayPointID++;
                StartCoroutine("Wait");
            }
        }
        else if(currentState == states.combat) {
            if (canAttack) {
                canAttack = false;
                StartCoroutine("AttackTimer");
                currentEnemy.GetComponent<EnemyBehaviour>().Life -= attack;
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Enemy") {
            currentEnemy = collision.gameObject;
            currentState = states.combat;
            collider.isTrigger = true;
            currentEnemy.GetComponent<BoxCollider>().isTrigger = true;
        }
    } 

    IEnumerator Wait() {
        timer = 0;
        yield return new WaitForSeconds(0.2f);
        canCheck = true;
    }

    IEnumerator AttackTimer() {
        yield return new WaitForSeconds(attackTimer);
        canAttack = true;
    }
}
