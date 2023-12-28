using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyAnimationEvent : MonoBehaviour
{
    public string mainSceneName = "Main";
    public void ToMainScene()
    {
        SceneManager.LoadScene(mainSceneName);
    }
}
