using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartMenus : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsSubMenu;

    public void PlayButtonMainMenu()
    {
        SceneManager.LoadScene("DevTestingScene");
    }

    public void ExitButtonMainMenu()
    {
        Application.Quit();
    }

    public void OptionsButtonMainMenu()
    {
        mainMenu.SetActive(false);
        optionsSubMenu.SetActive(true);
    }

    public void BackButtonOptionsSubMenu()
    {
        optionsSubMenu.SetActive(false);
        mainMenu.SetActive(true);
    }


}
