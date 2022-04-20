using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu_Canvas;
    [SerializeField] TextMeshProUGUI currentScene_Text;

    private PlayerInputControl playerInputControl;

    // Start is called before the first frame update
    void Start()
    {
        playerInputControl = new PlayerInputControl();

        currentScene_Text.text = "Current Scene: Game Map";

        playerInputControl.UI.Enable();
        playerInputControl.UI.PauseMenu.performed += Activate_DeActivate_PauseMenu;
    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Activate_DeActivate_PauseMenu(InputAction.CallbackContext context)
    {

        PauseMenu_Canvas.SetActive(!PauseMenu_Canvas.activeSelf);
      
        if (PauseMenu_Canvas.activeSelf)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void ChangeScene()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(1);
            currentScene_Text.text = "Current Scene: Game Map";
            Time.timeScale = 1f;
        }
        else
        {
            SceneManager.LoadScene(0);
            currentScene_Text.text = "Current Scene: Testing Scene";
            Time.timeScale = 1f;
        }

    }
}
