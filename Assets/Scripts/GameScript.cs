using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameScript : MonoBehaviour
{
    public QuestionList[] questions;
    QuestionList crntQ;
    public Text[] answersText;
    public Text qText;
    List<object> qList;
    int randQ;
    public Button[] answerBttns = new Button[3];
    public Sprite[] TFIcons = new Sprite[2];
    public Image TFIcon;
    public Text TFText;
    public GameObject headPanel;
    public Button buttonPlay;


    private void Start()
    {
        TFIcon.sprite = TFIcons[2];
        TFText.text = null;
    }
    public void OnClickPlay()
    {
        buttonPlay.interactable = false;
        qList = new List<object>(questions);
        questionGenerate();
        if (!headPanel.GetComponent<Animator>().enabled)
        {
            headPanel.GetComponent<Animator>().enabled = true;
        }
        else
        {
            headPanel.GetComponent<Animator>().SetTrigger("Down");
        }
        StartCoroutine(animBttns());
        //qText.text = questions[Random.Range(0, questions.Length)].question;
    }

    IEnumerator animBttns()
    {
        for (int i = 0; i < answerBttns.Length; i++) answerBttns[i].interactable = false;
        yield return new WaitForSeconds(0.5f);
        int a = 0;
        while (a < answerBttns.Length)
        {
            if (!answerBttns[a].gameObject.activeSelf) answerBttns[a].gameObject.SetActive(true);
            a++;
            yield return new WaitForSeconds(0.5f);
        }
        for (int i = 0; i < answerBttns.Length; i++) answerBttns[i].interactable = true;
        yield break;
    }

    void questionGenerate()
    {
        if (qList.Count > 0)
        {
            randQ = Random.Range(0, qList.Count);
            crntQ = qList[randQ] as QuestionList;
            qText.text = crntQ.question;
            List<string> answers = new List<string>(crntQ.answers);

            for (int i = 0; i < crntQ.answers.Length; i++)
            {
                int rand = Random.Range(0, answers.Count);
                answersText[i].text = answers[rand];
                answers.RemoveAt(rand);
                //answersText[i].text = crntQ.answers[i];
            }
        }
        else
        {
            print("Чики-Пуки!.");
        }
    }
    IEnumerator DisableAnsButtons()
    {
        answerBttns[0].interactable = false;
        answerBttns[1].interactable = false;
        answerBttns[2].interactable = false;
        yield return new WaitForSeconds(2.5f);
        answerBttns[0].interactable = true;
        answerBttns[1].interactable = true;
        answerBttns[2].interactable = true;
        yield break;
    }

    IEnumerator trueOrFalse(bool check)
    {
        StartCoroutine(DisableAnsButtons());
        if (check)
        {
            TFIcon.sprite = TFIcons[0];
            TFText.text = "Верно!";
            yield return new WaitForSeconds(1);
            buttonPlay.interactable = true;
            TFText.text = null;
            TFIcon.sprite = TFIcons[2];
            qList.RemoveAt(randQ);
            questionGenerate();
            yield break;
        }
        else
        {
            TFIcon.sprite = TFIcons[1];
            TFText.text = "Неверно!";
            yield return new WaitForSeconds(0.1f);
            buttonPlay.interactable = true;
            TFText.text = null;
            TFIcon.sprite = TFIcons[2];
            headPanel.GetComponent<Animator>().SetTrigger("Up");
            yield break;
        }
    }

        public void answersBtns(int index)
    {
        /*qList.RemoveAt(randQ);
        questionGenerate();*/
        if (answersText[index].text.ToString() == crntQ.answers[0])
        {
            StartCoroutine(trueOrFalse(true));
        }
        else
        {
            StartCoroutine(trueOrFalse(false));
        }
    }
}

[System.Serializable]
public class QuestionList 
{
    public string question;
    public string[] answers = new string[3];
}
