﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Health : MonoBehaviour
{
    [SerializeField] private GameObject entity;
    [SerializeField] private GameObject[] bars;
    [SerializeField] private Material enemyMat, allyMat;
    [SerializeField] private float offset;

    
    private int currentHealth;
    private SpriteRenderer spriteHealth;

    //Look at camera
    private Transform target;
    private float rotationSpeed = 10;
    private Quaternion lookRotation;
    private Vector3 direction;

    private int val = 10;

    void Start()
    {
        target = Camera.main.transform;
        spriteHealth = GetComponent<SpriteRenderer>();

        if (entity.tag == "Enemy") {
            foreach(GameObject go in bars) {
                go.GetComponent<MeshRenderer>().material = enemyMat;
            }
        }

        if (entity.tag == "Ally") {
            foreach (GameObject go in bars) {
                go.GetComponent<MeshRenderer>().material = allyMat;
            }
        }
        
        InvokeRepeating("ChangeLife", 0.5f, 0.5f);
    }

    void Update()
    {
        float step = 1 * Time.deltaTime;
        if (entity.tag == "Enemy" || entity.tag == "Ally") {
            currentHealth = entity.GetComponent<SoldierBehaviour>().Life;
        }

        //Look at cam
        direction = (new Vector3(target.position.x, transform.position.y, target.position.z) - transform.position).normalized;
        lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        

        if(currentHealth < 0.0f) {
            Destroy(gameObject);
        }
    }


    void ChangeLife() {
        val = valueLife(currentHealth);

        for (int i = 0; i < val+1; i++) {
            bars[i].SetActive(true);
        }
        for (int ii = val + 1; ii < bars.Length; ii++) {
            bars[ii].SetActive(false);
        }

    }


    int valueLife(float health) {
        if(health > 90) {
            return 9;
        }
        else if(health <= 90 && health >= 80) {
            return 8;
        }
        else if (health <= 80 && health >= 70) {
            return 7;
        }
        else if (health <= 70 && health >= 60) {
            return 6;
        }
        else if (health <= 60 && health >= 50) {
            return 5;
        }
        else if (health <= 50 && health >= 40) {
            return 4;
        }
        else if (health <= 40 && health >= 30) {
            return 3;
        }
        else if (health <= 30 && health >= 20) {
            return 2;
        }
        else if (health <= 20 && health >= 10) {
            return 1;
        }
        else if (health <= 10) {
            return 0;
        }
        else { return 0; }
    }
}
