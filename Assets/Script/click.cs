using UnityEngine;

public class sound : MonoBehaviour
{
     void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GetComponent<AudioSource>().Play(); 
        }
    }
}