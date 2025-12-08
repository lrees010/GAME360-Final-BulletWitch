using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class DialogueLine //class to store things like speaker name, speaker text, and path to the portrait image in Resources
{
    public string speaker;
    public string text;
    public string faceImagePath;
}

[System.Serializable]
public class Dialogue //list of dialogue lines
{
    public List<DialogueLine> lines;
}

public class DialogueManager : MonoBehaviour
{
    //dialogue ui
    public GameObject dialoguePanel;
    public Text speakerNameText;
    public Text speechText;
    public RawImage faceImage;

    private Queue<DialogueLine> dialogueQueue;
    private CanvasGroup panelCanvasGroup;
    public bool isDialogueActive = false;
    

    void Start()
    {
        RefreshReferences();

        
        dialogueQueue = new Queue<DialogueLine>(); //create new queue
        HidePanel();

        EventManager.Subscribe("OnReload",Reload);
        //events

    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe("OnReload", Reload);
    }

    private void Reload()
    {
        RefreshReferences();
        
        EndDialogue();
        
    }
    public static DialogueManager Instance { get; private set; } //singleton
    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }


    }
    public void StartConversation(object data)
    {
        if (isDialogueActive == true|| dialoguePanel == null) //if dialogue is already happening or panel doesnt exist, don't continue
        {
            return;
        }

        string jsonString = ((TextAsset)data).text; //convert data to json string

        Dialogue dialogue = JsonUtility.FromJson<Dialogue>(jsonString); //create new dialogue from class

        StartDialogue(dialogue);
    }

    void StartDialogue(Dialogue dialogue)
    {
        nextLineTime = (Time.time) + CalculateLineSpeed(dialogue.lines[0].text.Length); //time to wait until, based on length of speech text
        isDialogueActive = true;
        dialogueQueue.Clear();

        foreach (DialogueLine line in dialogue.lines) //add all to queue
        {
            dialogueQueue.Enqueue(line);
        }

        ShowPanel();

        StartCoroutine(DelayDisplayNext());
    }
    private IEnumerator DelayDisplayNext()
    {
        do
        {
            DisplayNextLine();
            yield return new WaitForSeconds(CalculateLineSpeed(speechText.text.Length));
        }
        while (isDialogueActive);
    }

    public void DisplayNextLine()
    {
        if (dialogueQueue.Count == 0) //if no more lines to display, end the dialogue
        {
            EndDialogue();
            return;
        }
        DialogueLine line = dialogueQueue.Dequeue(); //new line

        speakerNameText.text = line.speaker; //update ui texts
        speechText.text = line.text;



        if (!string.IsNullOrEmpty(line.faceImagePath)) //if face portrait path exists, use it
        {
            Texture2D texture = Resources.Load<Texture2D>(line.faceImagePath);
            if (texture != null)
            {
                faceImage.texture = texture;
                faceImage.gameObject.SetActive(true);
            }
            else
            {
                faceImage.gameObject.SetActive(false);
            }
        }

 

    }

 

    public void EndDialogue()
    {
        Debug.Log("enddialgue");
        HidePanel();
        isDialogueActive = false;
        dialogueQueue.Clear();
        StopAllCoroutines();
    }

    void ShowPanel()
    {
        //dialoguePanel.SetActive(true);
        //animation later
        panelCanvasGroup.alpha = 1;
    }

    void HidePanel()
    {
        panelCanvasGroup.alpha = 0;
    }

    float nextLineTime = 0f;
    float dialogueSpeed = 1;

    float CalculateLineSpeed(int lineLength)
    {
        return Mathf.Clamp(((float)lineLength / 14) * dialogueSpeed,3f,7f); //calculates line speed based on amount of characters
    }
    /*
    private void Update()
    {
        if (isDialogueActive &&
            Time.time>=nextLineTime
            )
        {
            int lineLength = (speechText.text).Length;

            nextLineTime = (Time.time) + CalculateLineSpeed(lineLength);
            DisplayNextLine();
        }
    } */

    public void RefreshReferences() //refresh gameobject references for UI
    {
        faceImage = GameObject.Find("Face")?.GetComponent<RawImage>();
        speakerNameText = GameObject.Find("SpeakerName")?.GetComponent<Text>();
        speechText = GameObject.Find("Speech")?.GetComponent<Text>();

        dialoguePanel = GameObject.Find("DialoguePanel");
        panelCanvasGroup = dialoguePanel.GetComponent<CanvasGroup>();
    }
}
