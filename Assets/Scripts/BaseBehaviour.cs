using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBehaviour : MonoBehaviour
{
    private enum baseSide { ally, enemy};
    [SerializeField] private baseSide side;
    [SerializeField] private GameManager GM;

    private void OnTriggerEnter(Collider other) {
        if(side == baseSide.ally && other.tag == "Enemy") {
            GM.HealthAlly -= 1;
            Destroy(other.gameObject);
        }
        if (side == baseSide.enemy && other.tag == "Ally") {
            GM.HealthEnemy -= 1;
            Destroy(other.gameObject);
        }
    }
}
