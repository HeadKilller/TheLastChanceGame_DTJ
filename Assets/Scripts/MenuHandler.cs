using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject OptionsMenu;
    [SerializeField] GameObject DeathMenu;
    [SerializeField] GameObject TakeDmg;
    [SerializeField] Player Player;
    float timer;
    bool startTimer;
    PlayerInputControl playerInputControl;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        playerInputControl = new PlayerInputControl();

        playerInputControl.UI.Enable();

        playerInputControl.UI.PauseMenu.performed += OpenClose_Menu;

    }

    private void Update()
    {
        if(startTimer)timer += Time.deltaTime;
    }


    public void OpenClose_Menu(InputAction.CallbackContext context)
    {

        PauseMenu.SetActive(!PauseMenu.activeSelf);

        if (PauseMenu.activeSelf)
        {
            PlayerGun.instance.isPauseMenuActivated = true;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;

        }
        else
        {
            PlayerGun.instance.isPauseMenuActivated = false;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 1f;
        }

    }

    public void Continue()
    {
        PlayerGun.instance.isPauseMenuActivated = false;

        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

    }

    public void ChangeTo_Pause_Options()
    {
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }
    public void ChangeTo_Options_Pause()
    {
        OptionsMenu.SetActive(false);
        PauseMenu.SetActive(true);
    }

    public void DeathScreen()
    {
            PlayerGun.instance.isPauseMenuActivated = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
            DeathMenu.SetActive(true);     
    }
    public void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name, LoadSceneMode.Single);
    }
    public void Exit()
    {

        //Debug.Log("Quit Game");
        SceneManager.LoadScene("Menu");

    }
    public void TakeDmgScreen()
    {
        TakeDmg.SetActive(true);
        startTimer = true;
        
        if(timer> 1f)
        {
            TakeDmg.SetActive(false);
            startTimer = false;
        }

    }


}