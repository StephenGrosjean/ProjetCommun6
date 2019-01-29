using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject bullet, spawnPos;
    [SerializeField] private float radius;
    [SerializeField] private GameObject target;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject smoke;
    [SerializeField] private AudioClip boom;
    private SphereCollider detector;

    private bool canShoot = true;

    private float rotationSpeed = 10;
    private Quaternion lookRotation;
    private Vector3 direction;


    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void Start()
    {
        detector = GetComponent<SphereCollider>();
        detector.radius = radius/2;
    }

    void Update()
    {
        if (canShoot && target != null) {
            Camera.main.GetComponent<AudioSource>().PlayOneShot(boom);
            GameObject smokeO = Instantiate(smoke, shootPoint.position, shootPoint.transform.rotation);
            Destroy(smokeO, 1.5f);
            canShoot = false;
            StartCoroutine("ShootCountdown");
            GameObject obj = Instantiate(bullet, spawnPos.transform.position, Quaternion.identity);
            obj.GetComponent<Rigidbody>().velocity = (target.transform.position - obj.transform.position) * 8;
        }

        if (target != null) {

            direction = (new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation * Quaternion.AngleAxis(180, Vector3.up), Time.deltaTime * rotationSpeed);
        }
    }

    private void OnTriggerEnter(Collider collision) {
        if(collision.gameObject.tag == "Enemy" && target == null) {
            target = collision.gameObject.GetComponent<SoldierBehaviour>().ShootingPoint.gameObject;
        }
    }

    IEnumerator ShootCountdown() {
        yield return new WaitForSeconds(2);
        canShoot = true;
    }
}
