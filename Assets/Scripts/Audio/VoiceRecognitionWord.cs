using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceRecognitionWord : MonoBehaviour
{
    public float speed = 2f;
    public float angleOpen = 90f; 
    public float angleClosed = 0f;

    private Dictionary<string, System.Action> dicWordAction;
    //private KeywordRecognizer keywordRecognizer;
    private DictationRecognizer dictationRecognizer;
    private Transform pivotDoor;
    private Quaternion rotationTarget;
    private bool doorOpen = false;
    private string currentSpeech = "";

    void Start()
    {
        pivotDoor = transform.parent;

        dictationRecognizer = new DictationRecognizer();

        dictationRecognizer.DictationResult += OnSpeechRecognized;
        dictationRecognizer.DictationHypothesis += OnSpeechHypothesis;
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

    private void OnSpeechRecognized(string text, ConfidenceLevel confidence)
    {
        currentSpeech = text.ToLower();
        //Debug.Log("Jugador dijo: " + text);

        // Verificamos si "abrir" y "puerta" están en la misma frase
        if (ContainsOpenAndDoorWithinMargin(text, "abrir", "puerta"))
        {
            OpenDoor();
        }
        else if (ContainsOpenAndDoorWithinMargin(text, "cerrar", "puerta"))
        {
            CloseDoor();
        }
    }
    private void OnSpeechHypothesis(string text)
    {
        // Mostrar lo que se está reconociendo en tiempo real mientras se habla.
        Debug.Log("Hablando: " + text);
    }

    private bool ContainsOpenAndDoorWithinMargin(string text, string word1, string word2)
    {
        string[] words = text.Split(' ');

        int indexAbrir = Array.IndexOf(words, word1);
        int indexPuerta = Array.IndexOf(words, word2);

        if (indexAbrir != -1 && indexPuerta != -1)
        {
            int wordDistance = Mathf.Abs(indexAbrir - indexPuerta) - 1;
            return wordDistance >= 0 && wordDistance <= 1;
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
}
