using UnityEngine;

public class rule_script : MonoBehaviour
{
	public GameObject Rule_image;
	public static int selected;
	bool armed = false;

	public void Start()
	{
		Rule_image.SetActive(false);
		selected = 0;

	}

	public void Update()
	{
		if (selected == 1 && !armed)
        {
            if (!Input.GetMouseButton(0)) armed = true;
            return;
        }
		if(Input.GetMouseButtonDown(0) && selected == 1 && armed)
		{
			Rule_image.SetActive(false);
			selected = 0;
		}
	}


    void OnMouseDown()
    {
	    if(rule_script.selected == 0) 
		{
			Rule();
		}
    }


    void Rule()
    {
        Rule_image.SetActive(true);
		selected = 1;
		armed = false;
    }
}