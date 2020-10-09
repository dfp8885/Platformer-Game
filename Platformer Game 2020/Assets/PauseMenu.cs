using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject winMenuUI;

    // Update is called once per frame
    void Update()
    {
        bool win = FloorComplete.win;
        if (Input.GetButtonDown("Cancel")) {
            if (win){
                LoadMenu();
            }
            else if (GameIsPaused) {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if (win) {
            WinGame();
        }
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu() {
        Time.timeScale = 1f;
        GameIsPaused = false;
        FloorComplete.win = false;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame() { 
        Debug.Log("Quit");
        Application.Quit();
    }

    public void WinGame() {
        winMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Restart() {
        FloorComplete.win = false;
        Time.timeScale = 1f;
        GameIsPaused = false;
        winMenuUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
