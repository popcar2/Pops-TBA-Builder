using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.SceneManagement;

public class TextPrompt : MonoBehaviour
{
    [SerializeField] int standardTypingSpeed = 60;
    [SerializeField] int spedUpTypingSpeed = 180;
    int currentTypingSpeed;

    // Cached Variables
    TextMeshProUGUI textComponent;
    TMP_InputField inputField;
    InputParser inputParser;

    // Helper variables
    private bool isPrinting = false;
    private bool gameOver = false;
    private Queue<string> printQueue;

    void Awake()
    {
        // Awake is used instead of Start because TextPrompt is the first thing that needs to be initialized before printing text
        textComponent = GetComponent<TextMeshProUGUI>();
        textComponent.text = "";

        inputField = FindObjectOfType<TMP_InputField>();
        inputParser = FindObjectOfType<InputParser>();

        printQueue = new Queue<string>();

        inputField.ActivateInputField();

        currentTypingSpeed = standardTypingSpeed;
    }

    void Update()
    {
        if (gameOver && !isPrinting && Input.anyKeyDown)
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrWhiteSpace(inputField.text) && !gameOver)
        {
            // Input text
            string userInput = inputField.text;
            inputField.text = "";
            printText("\n\n> " + userInput);
            inputParser.parseInput(userInput);
        }

        if (Input.GetKeyDown(KeyCode.Space) && string.IsNullOrWhiteSpace(inputField.text))
        {
            currentTypingSpeed = spedUpTypingSpeed;
            inputField.text = null;
        }

        inputField.ActivateInputField();
    }

    public void printText(string text)
    {
        printQueue.Enqueue(text);
        if (!isPrinting)
        {
            StartCoroutine(printQueuedText());
        }
    }

    public IEnumerator printTextAfterDelay(string text, float time)
    {
        yield return new WaitForSeconds(time);
        printText(text);
    }

    public void killPlayer()
    {
        gameOver = true;
        // For some reason inputfield doesn't deactivate so I just moved it off-screen lmao
        inputField.transform.position = new Vector2(0, -100);
    }

    public void winGame()
    {
        gameOver = true;
        // For some reason inputfield doesn't deactivate so I just moved it off-screen lmao
        inputField.transform.position = new Vector2(0, -100);
    }

    private IEnumerator printTextCoroutine(string text)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            int textLength = text.Length;
            StringBuilder printedText = new StringBuilder(textComponent.text);
            for (int i = 0; i < textLength; i++)
            {
                yield return new WaitForSeconds(1 / (float)currentTypingSpeed);
                printedText.Append(text[i]);
                updateText(printedText);
            }
        }
    }

    private void updateText(StringBuilder textBuilder)
    {
        textComponent.text = textBuilder.ToString();
    }

    private IEnumerator printQueuedText()
    {
        isPrinting = true;
        while (printQueue.Count != 0)
        {
            string currentText = printQueue.Dequeue();
            yield return StartCoroutine(printTextCoroutine(currentText));
        }
        isPrinting = false;
        currentTypingSpeed = standardTypingSpeed;
    }
}
