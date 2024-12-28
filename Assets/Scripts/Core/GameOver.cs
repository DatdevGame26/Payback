using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] Animator animator;
    string gameResult;
    void Start()
    {
        gameResult = PlayerPrefs.GetString("Game Result", "Win");
        InitGameOver();
    }

    void InitGameOver()
    {
        if(gameResult == "Win")
        {
            animator.Play("game_over_win");
            AudioManager.Instance.PlaySound("game_over_win", gameObject, false);
        }
        else
        {
            animator.Play("game_over_lose");
            AudioManager.Instance.PlaySound("game_over_lose", gameObject, false);
        }
        Cursor.lockState = CursorLockMode.None;
    }

}
