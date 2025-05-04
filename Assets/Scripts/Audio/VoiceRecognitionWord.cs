using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceRecognitionWord : MonoBehaviour
{
    public float speed = 2f;
    public float angleOpen = 90f; 
    public float angleClosed = 0f;
    public Transform pivotDoor;

    private Dictionary<string, System.Action> dicWordAction;
    //private KeywordRecognizer keywordRecognizer;
    private DictationRecognizer dictationRecognizer;
    private Quaternion rotationTarget;
    private bool doorOpen = false;
    private string currentSpeech = "";
    private bool playerInPosition = false;


    void Start()
    {
        //pivotDoor = transform.parent;

        dictationRecognizer = new DictationRecognizer();

        dictationRecognizer.DictationResult += OnSpeechRecognized;
        dictationRecognizer.DictationHypothesis += OnSpeechHypothesis;
        dictationRecognizer.DictationComplete += OnDictationComplete;
        dictationRecognizer.DictationError += OnDictationError;
        dictationRecognizer.Start();
        /*dicWordAction = new Dictionary<string, System.Action>();
        dicWordAction.Add("abrir puerta", OpenDoor);
        dicWordAction.Add("cerrar puerta", CloseDoor);

        keywordRecognizer = new KeywordRecognizer(dicWordAction.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognzeWord;
        keywordRecognizer.Start();*/
    }

    void Update()
    {
        if((!doorOpen && pivotDoor.transform.rotation.y != angleClosed) || (doorOpen && pivotDoor.transform.rotation.y != angleOpen))
        {
            pivotDoor.rotation = Quaternion.RotateTowards(
                  pivotDoor.rotation,
                  rotationTarget,
                  speed * Time.deltaTime * 100
              );
        }
      
    }

    private void OnDictationComplete(DictationCompletionCause completionCause)
    {
        if (completionCause != DictationCompletionCause.Complete)
        {
            Debug.LogWarning("Dictation completed unexpectedly: " + completionCause);
            dictationRecognizer.Stop();
            dictationRecognizer.Start();
        }
    }

    private void OnDictationError(string error, int hresult)
    {
        Debug.LogError($"Dictation error: {error}; HResult = {hresult}");
        dictationRecognizer.Stop();
        dictationRecognizer.Start();
    }

    private void OnSpeechRecognized(string text, ConfidenceLevel confidence)
    {
        currentSpeech = text.ToLower();
        //Debug.Log("Jugador dijo: " + text);

        // Verificamos si "abrir" y "puerta" están en la misma frase
        if (ContainsThreeWordsExact(currentSpeech, "teresa", "teresa", "teresa"))
        {
            if (playerInPosition)
            {
                OpenDoor();
            }
            
        }
        /*else if (ContainsTwoWordsWithinMargin(text, "cerrar", "puerta"))
        {
            CloseDoor();
        }*/
    }
    private void OnSpeechHypothesis(string text)
    {
        // Mostrar lo que se está reconociendo en tiempo real mientras se habla.
        Debug.Log("Hablando: " + text);
    }

    private bool ContainsTwoWordsWithinMargin(string text, string word1, string word2, string word3)
    {
        string[] words = text.Split(' ');

        int indexW1 = Array.IndexOf(words, word1);
        int indexW2 = Array.IndexOf(words, word2);

        if (indexW1 != -1 && indexW2 != -1)
        {
            int wordDistance = Mathf.Abs(indexW1 - indexW2) - 1;
            return wordDistance >= 0 && wordDistance <= 1;
        }

        return false;
    }

    private bool ContainsThreeWordsExact(string text, string word1, string word2, string word3)
    {
        string[] words = text.Split(' ');

        for (int i = 0; i < words.Length - 2; i++)
        {
            if (words[i] == word1 && words[i + 1] == word2 && words[i + 2] == word3)
            {
                return true;
            }
        }

        return false;
    }

    private void RecognzeWord(PhraseRecognizedEventArgs word)
    {
        Debug.Log(word.text);
        dicWordAction[word.text].Invoke();
    }

    private void OpenDoor()
    {
        doorOpen=true;
        rotationTarget = Quaternion.Euler(0, angleOpen, 0);
    }
    private void CloseDoor()
    {
        doorOpen = false;
        rotationTarget = Quaternion.Euler(0, angleClosed, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            playerInPosition = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            playerInPosition = false;
        }
    }
}
