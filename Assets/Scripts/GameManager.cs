using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int money;
    public int Money {
        get { return money; }
        set { money = value; UpdateMoney(); }
    }
    [SerializeField] private GameObject pauseMenu;

    [Header("Ally")]
    [SerializeField] private GameObject ally_Gun;
    [SerializeField] private GameObject ally_Sword;

    [Header("Enemy")]
    [SerializeField] private GameObject enemy_Gun;
    [SerializeField] private GameObject enemy_Sword;

    [Header("Prices")]
    [SerializeField] private int gun_Price;
    [SerializeField] private int sword_Price;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI healthAllyText;
    [SerializeField] private TextMeshProUGUI healthEnemyText;

    [Header("Healths")]
    [SerializeField] private int healthAlly;
    public int HealthAlly {
        get { return healthAlly; }
        set { healthAlly = value; UpdateHealth(); }
    }
    [SerializeField] private int healthEnemy;
    public int HealthEnemy {
        get { return healthEnemy; }
        set { healthEnemy = value; UpdateHealth(); }
    }

    [Space(20)]
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private Transform allySpawnPoint, enemySpawnPoint;


    [SerializeField] private TowerPlacement towerScript;

    public Transform[] WayPoints {
        get { return wayPoints; }
    }

    private bool isPaused;

    private void Start() {
        UpdateMoney();
        UpdateHealth();
    }

    private void Update() {
        if(healthEnemy < 0) {
            SceneManager.LoadScene("WinScene");
        }
        if(healthAlly < 0) {
            SceneManager.LoadScene("LooseScene");
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            PauseMenu();   
        }
    }

    public void Spawn_Gun() {
        if(money >= gun_Price) {
            money -= gun_Price;
            spawnSoldier(ally_Gun, allySpawnPoint);
            UpdateMoney();
        }
    }

    public void Spawn_Sword() {
        if (money >= sword_Price) {
            money -= sword_Price;
            spawnSoldier(ally_Sword, allySpawnPoint);
            UpdateMoney();
        }
    }

    public void spawnSoldier(GameObject whatToSpawn, Transform whereToSpawn) {
        Instantiate(whatToSpawn, whereToSpawn.position, Quaternion.identity);
    }

    public void EnableTowerPlacement() {
        towerScript.IsSelected = true;
    }

    public void UpdateMoney() {
        moneyText.text = "Money : " + money;
    }

    void UpdateHealth() {
        healthAllyText.text = healthAlly.ToString();
        healthEnemyText.text = healthEnemy.ToString();

    }

    public void PauseMenu() {
        isPaused = !isPaused;
        pauseMenu.SetActive(!pauseMenu.active);
    }

}
