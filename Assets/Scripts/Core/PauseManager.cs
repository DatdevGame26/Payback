using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool isPaused;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] Shop shop;
    void Start()
    {
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        string soundPlay = isPaused ? "pause_game" : "continue_game";
        AudioManager.Instance.PlaySound(soundPlay, gameObject, false);


        Time.timeScale = isPaused ? 0.0f : 1.0f;
        if (shop.IsShopOpened()) Time.timeScale = 0;

        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        if (shop.IsShopOpened()) Cursor.lockState = CursorLockMode.None;


        pausePanel.SetActive(isPaused);

        tutorialPanel.SetActive(false);
    }
}
