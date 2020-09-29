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

    /// <summary>
    /// Adds text to the text queue and begins printing if it isn't already.
    /// </summary>
    /// <param name="text"></param>
    public void printText(string text)
    {
        printQueue.Enqueue(text);
        if (!isPrinting)
        {
            StartCoroutine(printQueuedText());
        }
    }

    /// <summary>
    /// Begins printing text after a set delay.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="time">Seconds before printing</param>
    /// <returns></returns>
    public IEnumerator printTextAfterDelay(string text, float time)
    {
        yield return new WaitForSeconds(time);
        printText(text);
    }

    /// <summary>
    /// Lists kill message and restarts the game when any key is pressed.
    /// </summary>
    public void killPlayer()
    {
        gameOver = true;
        // For some reason inputfield doesn't deactivate so I just moved it off-screen lmao
        inputField.transform.position = new Vector2(0, -100);
    }

    /// <summary>
    /// Lists win message and restarts the game when any key is pressed. 
    /// </summary>
    public void winGame()
    {
        gameOver = true;
        // For some reason inputfield doesn't deactivate so I just moved it off-screen lmao
        inputField.transform.position = new Vector2(0, -100);
    }

    /// <summary>
    /// The coroutine which prints the text. Use printText for general use.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Updates the text prompt.
    /// </summary>
    /// <param name="textBuilder"></param>
    private void updateText(StringBuilder textBuilder)
    {
        textComponent.text = textBuilder.ToString();
    }

    /// <summary>
    /// Repeatedly calls printTextCoroutine while printQueue isn't empty. Use printText for general use.
    /// </summary>
    /// <returns></returns>
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
