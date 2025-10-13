using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using TMPro;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int score;
    public static int number_of_problems;
    public static int nowProblemNumber;

    public GameObject timer_board;
    public GameObject bar;
    public GameObject conv_context;

    public TMP_Text conv_context_txt;
        
    public GameObject question_board;
    public GameObject secretary;
    public GameObject witch;

    public TMP_Text textA;
    public TMP_Text textB;
    public TMP_Text textC;
    public TMP_Text timer;

    public GameObject three;
    public GameObject two;
    public GameObject one;
    public GameObject Go;

    public GameObject OptionA;
    public GameObject OptionB;
    public GameObject OptionC;

    public int i = 0;

    public static int[] options = new int[3];
    public static int[] options_shuffle = new int[3];
    public static int[] options_shuffle_reversed = new int[3];
    public string[] answer_problem = new string[3];
    public static string[] temp_explanations = new string[3];
    public static int watched;
    public static int id_managed;
    public static bool addOK;
    public static bool answered;

    //[SerializeField] private TMP_Text targetText;

    public Sprite[] secretaryList;
    public Sprite[] witchList;
    public Sprite[] Board_direction;
    public string[] explanations = new string[3];
    // private string[] answer_problem = new string[3];


    [System.Serializable]
    public class TextData
    {
        public int speaking;        // �N�������Ă邩
        public int emoOfSecretary;  // �鏑�̕\��
        public int emoOfWitch;      // �����̕\��
        public string context;      // ��b���e
    }

    [System.Serializable]
    public class TextData2
    {
        public string problem_context;
        public string option1;
        public string option2;
        public string option3;
        public int answer;      
        public int emoOfSecretary1;  // �鏑�̕\��
        public int emoOfWitch1;      // �����̕\��
        public int emoOfSecretary2;  // �鏑�̕\��
        public int emoOfWitch2;      // �����̕\��
        public int emoOfSecretary3;  // �鏑�̕\��
        public int emoOfWitch3;      // �����̕\��
        public int emoOfSecretary4;  // �鏑�̕\��
        public int emoOfWitch4;      // �����̕\��
        public string explanation1;      // ��b���e
        public string explanation2;      // ��b���e
        public string explanation3;      // ��b���e
    }

    [SerializeField] private GraphicRaycaster uiRaycaster;

    public int gameStatus = 0;
    private List<TextData> conversationArray = new List<TextData>();
    private List<TextData2> ProblemsArray = new List<TextData2>();
    public List<int> random_index = new List<int>();

    public int currentLineIndex = 0;

    void Start()
    {
        score = 0;

        //scene: result�ŏ����Ȃ��悤��
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        
        switch(gameStatus) {
            //���ׂẴ��[�h (0)
            case 0:
                beforeConversation();
                break;
            case 1:
                //(�t�F�[�h�C��) (1)
                //fadeIn();
                break;
            case 6:
                //�t�F�[�h�C����
                afterFadeIn("mediumConv");
                break;
            case 2:
                //��b�t�F�[�Y (2)
                conversation();
                break;
            case 3:
                //�J�E���g�_�E�� (3)
                countDown("mediumQues");
                break;
            case 4:
                //���o��t�F�[�Y (4)
                question();
                break;
            case 5:
                //�I�� & (�t�F�[�h�A�E�g) (5)
                gameStatus = 7;
                finishQuestion();
                //fadeOut();
                break;
        }
    }

    void beforeConversation()
    {
        timer_board.SetActive(false);
        bar.SetActive(false);
        three.SetActive(false);
        two.SetActive(false);
        one.SetActive(false);
        Go.SetActive(false);

        gameStatus = 6;
    }

    void afterFadeIn(string t)
    {
        LoadTextData(t);
        if(conversationArray.Count>currentLineIndex)
        {
            convUpdate();
        }

        gameStatus = 2;
    }

    void conversation()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("hidari");
            if (conversationArray.Count>currentLineIndex)
            {
                convUpdate();
            }
            else
            {
                gameStatus = 3;
            }
        }
    }

    void convUpdate()
    {
        //�����o���̌���, ��b���e
        question_board.GetComponent<SpriteRenderer>().sprite = Board_direction[conversationArray[currentLineIndex].speaking];
        conv_context_txt.text = conversationArray[currentLineIndex].context;
        //�鏑�̕\��
        secretary.GetComponent<SpriteRenderer>().sprite = secretaryList[conversationArray[currentLineIndex].emoOfSecretary];
        //�����̕\��
        witch.GetComponent<SpriteRenderer>().sprite = witchList[conversationArray[currentLineIndex].emoOfWitch];
        currentLineIndex++;
    }

    // CSV�t�@�C����ǂݍ���
    private void LoadTextData(string t)
    {
        string path = Path.Combine(Application.streamingAssetsPath, t+".csv");
        string[] lines = File.ReadAllLines(path);

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] fields = line.Split(',');
            if (fields.Length < 4)
            {
                Debug.LogError($"�����ȍs�̌`��: {line}");
                continue;
            }

            TextData data = new TextData
            {
                speaking       = int.Parse(fields[0].Trim('\"')),
                emoOfSecretary = int.Parse(fields[1].Trim('\"')),
                emoOfWitch     = int.Parse(fields[2].Trim('\"')),
                context        = fields[3].Trim('\"')
            };

            Debug.Log(data.speaking);
            Debug.Log(data.emoOfSecretary);
            Debug.Log(data.emoOfWitch);
            Debug.Log(data.context);

            conversationArray.Add(data);
        }
    }

    private bool isCountingDown = false;

    void countDown(string t)
    {
        if (isCountingDown) return;               // ���d�J�n�h�~
        StartCoroutine(CountDownRoutine(t));
    }

    private IEnumerator CountDownRoutine(string t)
    {
        question_board.SetActive(false);
        conv_context.SetActive(false);

        isCountingDown = true;

        // �O�̂��߈�U�S��OFF
        if (three) three.SetActive(false);
        if (two)   two.SetActive(false);
        if (one)   one.SetActive(false);
        if (Go)    Go.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        if (three) three.SetActive(true);
        yield return new WaitForSeconds(1f);
        if (three) three.SetActive(false);

        if (two) two.SetActive(true);
        yield return new WaitForSeconds(1f);
        if (two) two.SetActive(false);

        if (one) one.SetActive(true);
        yield return new WaitForSeconds(1f);
        if (one) one.SetActive(false);

        if (Go) Go.SetActive(true);
        yield return new WaitForSeconds(1f);
        if (Go) Go.SetActive(false);

        yield return new WaitForSeconds(0.3f);

        gameStatus = 4;          // ���̃t�F�[�Y��
        nowProblemNumber = 0;
        LoadTextData2(t);
        addOK=false;
        //Debug.Log(ProblemsArray[0].problem_context);
        isCountingDown = false;  // �t���O����
    }

    private bool isAnswering = false;
    private bool awaitingAnswer = false;
    private int chosenOption = -1;

    private bool explanation_state = false;
    public static bool explanationWriting = false;

    void question()
    {
        if (isAnswering)
        {
            return;
        }

        if(!explanation_state)
        {
            question_board.SetActive(true);
            conv_context.SetActive(true);
            timer_board.SetActive(true);
            bar.SetActive(true);


            isAnswering = true;
            answered = false;

        
            if(nowProblemNumber<ProblemsArray.Count)
            {

                options_shuffle = new int[] { 0, 1, 2 };

                options = new int[] { 0, 0, 0 };

                System.Random rand = new System.Random();

                for (int i=1; i<3; i++)
                {
                    int j = rand.Next(i + 1);
                    (options_shuffle[i], options_shuffle[j]) = (options_shuffle[j], options_shuffle[i]);
                }

                for(int i=0; i<3; i++)
                {
                    options_shuffle_reversed[options_shuffle[i]] = i;
                }

                watched = -1;


                options[options_shuffle[0]] = 1;
                

                conv_context_txt.text = ProblemsArray[nowProblemNumber].problem_context;

                answer_problem[0] = ProblemsArray[nowProblemNumber].option1;
                answer_problem[1] = ProblemsArray[nowProblemNumber].option2;
                answer_problem[2] = ProblemsArray[nowProblemNumber].option3;

                textA.text = answer_problem[options_shuffle_reversed[0]];
                textB.text = answer_problem[options_shuffle_reversed[1]];
                textC.text = answer_problem[options_shuffle_reversed[2]];
                Debug.Log(options_shuffle_reversed[0]);
                Debug.Log(textA.text);
                Debug.Log(options_shuffle_reversed[1]);
                Debug.Log(textB.text);
                Debug.Log(options_shuffle_reversed[2]);
                Debug.Log(textC.text);

                //random_index.Add(0);
                //random_index.Add(1);
                //random_index.Add(2);
                //Debug.Log(random_index);

                //i = Random.Range(0, random_index.Count);
                //Debug.Log(i);
                //textA.text = answer_problem[random_index[i]];
                //random_index.RemoveAt(i);
                //Debug.Log(random_index);

                //i = Random.Range(0, random_index.Count);
                //textB.text = answer_problem[random_index[i]];
                //random_index.RemoveAt(i);
                //Debug.Log(i);
                //Debug.Log(random_index);

                //textC.text = answer_problem[random_index[0]];
                //Debug.Log(conv_context_txt.text );
                StartCoroutine(inGameTimer());
            }
            else
            {
                gameStatus = 5;
                return;
            }

            //�鏑�̕\��
            secretary.GetComponent<SpriteRenderer>().sprite = secretaryList[ProblemsArray[nowProblemNumber].emoOfSecretary1];
            //�����̕\��
            witch.GetComponent<SpriteRenderer>().sprite = witchList[ProblemsArray[nowProblemNumber].emoOfWitch1];

            return;
        }
        if (!explanationWriting)
        {

            temp_explanations = new string[] { ProblemsArray[nowProblemNumber].explanation1, ProblemsArray[nowProblemNumber].explanation2, ProblemsArray[nowProblemNumber].explanation3 };

            explanations = new string[] { temp_explanations[options_shuffle_reversed[0]], temp_explanations[options_shuffle_reversed[1]], temp_explanations[options_shuffle_reversed[2]] };

            conv_context_txt.text = explanations[id_managed];

            if (!answered)
            {
                conv_context_txt.text = "���߂�̂��x�����܂��I";
            }
            if (id_managed == 0)
            {
                //�鏑�̕\��
                secretary.GetComponent<SpriteRenderer>().sprite = secretaryList[ProblemsArray[nowProblemNumber].emoOfSecretary2];
                //�����̕\��
                witch.GetComponent<SpriteRenderer>().sprite = witchList[ProblemsArray[nowProblemNumber].emoOfWitch2];
            }
            else if (id_managed == 1)
            {
                //�鏑�̕\��
                secretary.GetComponent<SpriteRenderer>().sprite = secretaryList[ProblemsArray[nowProblemNumber].emoOfSecretary3];
                //�����̕\��
                witch.GetComponent<SpriteRenderer>().sprite = witchList[ProblemsArray[nowProblemNumber].emoOfWitch3];
            }
            else
            {
                //�鏑�̕\��
                secretary.GetComponent<SpriteRenderer>().sprite = secretaryList[ProblemsArray[nowProblemNumber].emoOfSecretary4];
                //�����̕\��
                witch.GetComponent<SpriteRenderer>().sprite = witchList[ProblemsArray[nowProblemNumber].emoOfWitch4];
            }
            explanationWriting = true;
            return;
        }
        if(Input.GetMouseButtonDown(0))
        {
            nowProblemNumber++;
            explanationWriting = false;
            explanation_state = false;
        }
        
    }

    public int left;

    private IEnumerator inGameTimer()
    {
        addOK=true;
        Debug.Log(nowProblemNumber);
        left = 50;
        while(left > 0)
        {
            if(watched == -1)
            {
                double dtimer = left/10.0;
                timer.text = dtimer.ToString();
                left--;
            }
            else
            {
                answered = true;
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        explanation_state = true;
        isAnswering = false;
        addOK=false;
    }

    private void LoadTextData2(string t)
    {
        string path = Path.Combine(Application.streamingAssetsPath, t+".csv");
        string[] lines = File.ReadAllLines(path);

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] fields = line.Split(',');
            if (fields.Length < 10)
            {
                Debug.LogError($"�����ȍs�̌`��: {line}");
                continue;
            }

            TextData2 data = new TextData2
            {
                problem_context = fields[0].Trim('\"'),
                option1 = fields[1].Trim('\"'),
                option2 = fields[2].Trim('\"'),
                option3 = fields[3].Trim('\"'),
                answer = int.Parse(fields[4].Trim('\"')),
                emoOfSecretary1 = int.Parse(fields[5].Trim('\"')),
                emoOfWitch1 = int.Parse(fields[6].Trim('\"')),
                emoOfSecretary2 = int.Parse(fields[7].Trim('\"')),
                emoOfWitch2 = int.Parse(fields[8].Trim('\"')),
                emoOfSecretary3 = int.Parse(fields[9].Trim('\"')),
                emoOfWitch3 = int.Parse(fields[10].Trim('\"')),
                emoOfSecretary4 = int.Parse(fields[11].Trim('\"')),
                emoOfWitch4 = int.Parse(fields[12].Trim('\"')),
                explanation1 = fields[13].Trim('\"'),
                explanation2 = fields[14].Trim('\"'),
                explanation3 = fields[15].Trim('\"')
            };

            ProblemsArray.Add(data);
        }
    }

    void finishQuestion()
    {
        SceneManager.LoadScene("result");
    }
}