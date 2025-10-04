using UnityEngine;
using UnityEngine.SceneManagement;

public class start_script : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log("hey");
        if(rule_script.selected == 0) 
        {
            Game_start();
        }
    }


    void Game_start()
    {
        SceneManager.LoadScene("play_scene");
    }
}
