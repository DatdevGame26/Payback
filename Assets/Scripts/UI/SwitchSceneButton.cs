using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//  Nút chuyển sang cảnh khác
public class SwitchSceneButton : MonoBehaviour
{
    [SerializeField] int nextSceneIndex;

    public void switchScene()
    {
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}
