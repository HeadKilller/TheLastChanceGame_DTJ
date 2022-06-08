using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuHandler : MonoBehaviour
{

    [SerializeField] GameObject PauseMenu_Canvas;

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

    // Update is called once per frame
   

    public void OpenClose_Menu(InputAction.CallbackContext context)
    {

        PauseMenu_Canvas.SetActive(!PauseMenu_Canvas.activeSelf);

        if (PauseMenu_Canvas.activeSelf)
        {

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;

        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 1f;
        }

    }

    public void Continue()
    {

        PauseMenu_Canvas.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

    }

    public void Options()
    {
        Debug.Log("Options");
    }

    public void Exit()
    {

        Debug.Log("Quit Game");
        Application.Quit();

    }


}
