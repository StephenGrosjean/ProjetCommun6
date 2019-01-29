using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SoldierBehaviour : MonoBehaviour
{
    private enum side { ally, enemy};
    private enum states { combat, walking };
    private enum combatClass { gun, sword };
    public bool ignoreAnim;
    [SerializeField] private AudioClip GunShot, Sword;
    [SerializeField] private combatClass soldierType;
    [SerializeField] private side soldierSide;
    [SerializeField] private float speed, attackTimer;
    [SerializeField] private int attack;
    [SerializeField] private GameObject shootingGun, walkingGun;
    [SerializeField] private Transform shootingPoint;
    public Transform ShootingPoint {
        get { return shootingPoint; }
    }

    private Transform[] getWaypoints;
    private Animator animatorComponent;
    private GameManager gameManager;
    private Transform currentWayPoint;
    public GameObject currentEnemy;
    public GameObject lastEnemy;
    private states currentState;


    [SerializeField] private int life = 100;
    public int Life {
        get { return life; }
        set { life = value; }
    }

    private List<Transform> waypoints = new List<Transform>();
    private Rigidbody rigid;
    private BoxCollider collider;

    private int currentWayPointID;
    private bool canCheck = true;
    private float timer;

    private bool canAttack = true;

    private float rotationSpeed = 10;
    private Quaternion lookRotation;
    private Vector3 direction;
    private bool isDead, called;

    void Start()
    {
        if (!ignoreAnim) {
            animatorComponent = GetComponent<Animator>();
        }
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rigid = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();

        getWaypoints = gameManager.WayPoints;
        
        foreach(Transform wp in getWaypoints) {
            waypoints.Add(wp);
        }

        if (soldierSide == side.ally) {
            waypoints.Reverse();
        }

        currentWayPointID = 0;
        currentWayPoint = waypoints[currentWayPointID];
    }

    void Update()
    {
        
        if (!isDead) {
            if (!ignoreAnim && soldierType == combatClass.gun) {
                if (currentState == states.combat) {
                    walkingGun.SetActive(false);
                    shootingGun.SetActive(true);
                }
                else {
                    walkingGun.SetActive(true);
                    shootingGun.SetActive(false);
                }
            }

            if (currentEnemy == null) {
                if (soldierSide == side.enemy) {
                    gameObject.layer = LayerMask.NameToLayer("Enemy");
                }
                else {
                    gameObject.layer = LayerMask.NameToLayer("Ally");
                }

                collider.isTrigger = false;
                currentState = states.walking;
                rigid.constraints = RigidbodyConstraints.None;
            }
            else {
                lastEnemy = currentEnemy;
                gameObject.layer = LayerMask.NameToLayer("InCombat");
            }

            if (!ignoreAnim) {
                if (soldierType == combatClass.gun) {
                    animatorComponent.SetBool("isWalkingGun", currentEnemy == null);
                }
                else {
                    animatorComponent.SetBool("isWalkingSword", currentEnemy == null);

                }

                if (soldierType == combatClass.sword) {
                    animatorComponent.SetBool("isAttacking", !(currentEnemy == null));
                }
                else {
                    animatorComponent.SetBool("isShooting", !(currentEnemy == null));
                }
            }
            if (life <= 0) {
                gameManager.Money += 15;
                isDead = true;
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
            else if (currentState == states.combat) {
                if (canAttack) {
                    canAttack = false;
                    int RDMN = Random.Range(0, 100);

                    if (RDMN > 80) {
                        if (soldierType == combatClass.gun) {
                            Camera.main.GetComponent<AudioSource>().PlayOneShot(GunShot);
                        }
                        else {
                            Camera.main.GetComponent<AudioSource>().PlayOneShot(Sword);
                        }
                    }
                    StartCoroutine("AttackTimer");
                    currentEnemy.GetComponent<SoldierBehaviour>().Life -= attack;

                }
            }

            direction = (new Vector3(waypoints[currentWayPointID + 1].position.x, transform.position.y, waypoints[currentWayPointID + 1].position.z) - transform.position).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
        else {
            if (!called) {
                called = true;
                if (currentEnemy != null) {
                    currentEnemy.GetComponent<SoldierBehaviour>().currentEnemy = null;
                }
                transform.GetComponent<BoxCollider>().isTrigger = true;
                rigid.constraints = RigidbodyConstraints.FreezeAll;
                animatorComponent.SetBool("isAttacking", false);
                animatorComponent.SetBool("isShooting", false);
                animatorComponent.SetBool("isWalkingSword", false);
                animatorComponent.SetBool("isWalkingGun", false);
                animatorComponent.SetBool("isDead", true);
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
       
        if (collision.gameObject.layer != LayerMask.NameToLayer("InCombat") && !isDead) {
            if (soldierSide == side.enemy) {
                if (collision.gameObject.tag == "Ally") {
                    currentEnemy = collision.gameObject;
                    currentState = states.combat;
                    collider.isTrigger = true;
                    currentEnemy.GetComponent<BoxCollider>().isTrigger = true;
                    rigid.constraints = RigidbodyConstraints.FreezeAll;

                }
            }
            else if (soldierSide == side.ally) {
                if (collision.gameObject.tag == "Enemy") {
                    currentEnemy = collision.gameObject;
                    currentState = states.combat;
                    collider.isTrigger = true;
                    currentEnemy.GetComponent<BoxCollider>().isTrigger = true;
                    rigid.constraints = RigidbodyConstraints.FreezeAll;

                }
            }
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
