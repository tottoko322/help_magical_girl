using UnityEngine;

public class OptionsOnClick : MonoBehaviour
{
    public int id;

    void OnMouseDown()
    {
        GameManager.id_managed = id;
        GameManager.watched = GameManager.options[id];
        Debug.Log(GameManager.watched);
        if(GameManager.watched == 1 && GameManager.addOK)
        {
            GameManager.score++;
        }

    }
}