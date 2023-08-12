using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject controlScheme;
    [SerializeField] private bool isPaused;
    [SerializeField] private GameObject volumeSettingUI;
    [SerializeField] private GameObject youWinUI;
    public PlayerHealth youDiedBool;
    private bool youdiedboolean;
    private bool youwin = false;
    private void Update()
    {
        youdiedboolean = youDiedBool.youdiedbool;
        if (Input.GetKeyDown(KeyCode.Escape) && youdiedboolean == false)
        {
            isPaused = !isPaused;
        }

        if (isPaused && !youwin)
        {
            ActivateMenu();
        }
        else if (youwin)
        {

        }
        else
        {
            DeactivateMenu();
        }
    }

    public void YouWinScreen()
    {
        youwin = true;
        isPaused = true;
        Time.timeScale = 0;
        youWinUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ActivateMenu ()
    {
        Time.timeScale = 0;
        pauseMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void DeactivateMenu ()
    {
        Time.timeScale = 1;
        pauseMenuUI.SetActive(false);
        controlScheme.SetActive(false);
        volumeSettingUI.SetActive(false);
        youWinUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
        youwin = false;
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Restart()
    {
        SceneManager.LoadScene(1);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void Controls()
    {
        controlScheme.SetActive(true);
        pauseMenuUI.SetActive(false);
    }
    public void Back()
    {
        pauseMenuUI.SetActive(true);
        controlScheme.SetActive(false);
    }
    public void Options()
    {
        pauseMenuUI.SetActive(false);
        volumeSettingUI.SetActive(true);
    }
    public void OptionsBack()
    {
        volumeSettingUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }
}
