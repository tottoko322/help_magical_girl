using UnityEngine;

public class end_script : MonoBehaviour
{
    void OnMouseDown()
    {
        if(rule_script.selected == 0) 
        {
            Game_end();
        }
    }


	public void Game_end()
	{
		#if UNITY_EDITOR
		    UnityEditor.EditorApplication.isPlaying = false;
	    #else
		    Application.Quit();
		#endif
	}
}
