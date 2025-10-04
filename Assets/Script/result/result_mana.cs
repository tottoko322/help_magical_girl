using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class result_mana : MonoBehaviour
{
    public GameObject secretary_sd;
    public GameObject magic_girl_sd;
    public GameObject title;
    public GameObject button;
    public TMP_Text score_text;


    public Sprite[] chara_sec = new Sprite[5];
    public Sprite[] chara_magic = new Sprite[5];
    public Sprite[] titleList = new Sprite[5];

    void Start()
    {

        button.SetActive(false);

        if(GameManager.score == 10)
        {
            secretary_sd.GetComponent<SpriteRenderer>().sprite = chara_sec[0];
            magic_girl_sd.GetComponent<SpriteRenderer>().sprite = chara_magic[0];
            title.GetComponent<SpriteRenderer>().sprite = titleList[0];
        }
        else if(GameManager.score > 6)
        {
            secretary_sd.GetComponent<SpriteRenderer>().sprite = chara_sec[1];
            magic_girl_sd.GetComponent<SpriteRenderer>().sprite = chara_magic[1];
            title.GetComponent<SpriteRenderer>().sprite = titleList[1];
        }
        else if(GameManager.score > 3)
        {
            secretary_sd.GetComponent<SpriteRenderer>().sprite = chara_sec[2];
            magic_girl_sd.GetComponent<SpriteRenderer>().sprite = chara_magic[2];
            title.GetComponent<SpriteRenderer>().sprite = titleList[2];
        }
        else if(GameManager.score > 0)
        {
            secretary_sd.GetComponent<SpriteRenderer>().sprite = chara_sec[3];
            magic_girl_sd.GetComponent<SpriteRenderer>().sprite = chara_magic[3];
            title.GetComponent<SpriteRenderer>().sprite = titleList[3];
        }
        else
        {
            secretary_sd.GetComponent<SpriteRenderer>().sprite = chara_sec[4];
            magic_girl_sd.GetComponent<SpriteRenderer>().sprite = chara_magic[4];
            title.GetComponent<SpriteRenderer>().sprite = titleList[4];
        }

        score_text.text = "スコア: "+GameManager.score.ToString() + "点";

    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            button1.t = true;
            button.SetActive(true);
        }
    }
}
