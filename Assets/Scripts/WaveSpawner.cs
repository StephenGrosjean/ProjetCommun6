using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    [SerializeField] private Transform spawnPos;
    [SerializeField] private GameObject enemy_Sword, enemy_Gun;
   
    private enum soldierType { gun, sword};
    [System.Serializable]
    class wave {
        public soldierType type;
        public int number;
        public float spawnRate;
        public int timeBeforeNextWave;
    }

    [SerializeField] private List<wave> waves;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("spawnWaves");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator spawnWaves() {
        foreach (wave wv in waves) {
            if (wv.type == soldierType.sword) {
                for (int i = 0; i < wv.number; i++) {
                    Instantiate(enemy_Sword, spawnPos.position, Quaternion.identity);
                    yield return new WaitForSeconds(wv.spawnRate);
                }
            }

            if (wv.type == soldierType.gun) {
                for (int i = 0; i < wv.number; i++) {
                    Instantiate(enemy_Gun, spawnPos.position, Quaternion.identity);
                    yield return new WaitForSeconds(wv.spawnRate);
                }
            }

            yield return new WaitForSeconds(wv.timeBeforeNextWave);
        }
    }
}
