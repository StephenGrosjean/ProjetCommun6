using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void Restart() {
        SceneManager.LoadScene("MainLevel");
    }

    public void Menu() {
        SceneManager.LoadScene("Menu");
    }

    public void Quit() {
        Application.Quit();
    }

}
