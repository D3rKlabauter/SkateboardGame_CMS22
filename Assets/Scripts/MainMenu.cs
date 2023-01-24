using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    /* public void OnStart()
     {
         // Load my game scene here...
         SceneManager.LoadScene(1);
     }

     public void OnQuit()
     {
         // Quit my game here...

 #if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false;
 #else
         Application.Quit();
 #endif
     }*/

    public void GoToScene(string sceneName)
     {
         SceneManager.LoadScene(sceneName);
     }

    public void QuitApp()
     {
         Application.Quit();
         Debug.Log("Application has quit.");
     } 


}

