using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool isPaused;
    [SerializeField] GameObject pausePanel;
    void Start()
    {
    }

    // Update is called once per frame
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
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        pausePanel.SetActive(isPaused);
    }
}
