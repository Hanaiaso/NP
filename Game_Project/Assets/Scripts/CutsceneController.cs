using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class CutsceneController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Cutscene Data")]
    [SerializeField] private Sprite[] sceneImages;
    [TextArea(3, 5)]
    [SerializeField] private string[] dialogues;
    [SerializeField] private float textSpeed = 0.03f;

    [HideInInspector] public System.Action onCutsceneEnd; // 🔔 Callback khi cutscene xong

    private int currentIndex = 0;
    private bool isTyping = false;
    private bool canContinue = false;
    private string currentLine = "";

    public void BeginCutscene()
    {
        currentIndex = 0;
        gameObject.SetActive(true);
        ShowCurrentScene();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = currentLine;
                isTyping = false;
                canContinue = true;
            }
            else if (canContinue)
            {
                NextScene();
            }
        }
    }

    private void ShowCurrentScene()
    {
        if (currentIndex < sceneImages.Length)
        {
            backgroundImage.sprite = sceneImages[currentIndex];
            StartCoroutine(TypeDialogue(dialogues[currentIndex]));
        }
    }

    private IEnumerator TypeDialogue(string line)
    {
        isTyping = true;
        canContinue = false;
        currentLine = line;
        dialogueText.text = "";

        foreach (char a in line)
        {
            dialogueText.text += a;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
        canContinue = true;
    }

    private void NextScene()
    {
        currentIndex++;

        if (currentIndex >= dialogues.Length)
        {
            onCutsceneEnd?.Invoke(); // 🔔 Gọi callback từ GameManage
            gameObject.SetActive(false);
        }
        else
        {
            ShowCurrentScene();
        }
    }
}
