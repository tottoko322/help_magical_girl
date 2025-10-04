using UnityEngine;
using UnityEngine.SceneManagement;

public class button1: MonoBehaviour
{
    public static bool t = false;

    void Start()
    {
 
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            t = true;
        }
    }

    void OnMouseDown()
    {
        if(t)
        {    
            tittle_to();
        }
    }


    void tittle_to()
    {
        SceneManager.LoadScene("title");
    }
}
