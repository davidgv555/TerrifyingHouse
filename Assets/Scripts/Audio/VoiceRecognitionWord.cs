using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceRecognitionWord : MonoBehaviour
{
    public float speed = 2f;
    public ActionDoor door;
    public GameObject player;
    public GameObject enemy;


    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, System.Action> dicWordAction;
    private bool playerInPosition = false;
    private GameObject enemyAnim;
    private GameObject camera;
    private Animator anim;
    private bool actionDone =false;

    //Reconocimiento de Voz activa
    //private DictationRecognizer dictationRecognizer;
    //private string currentSpeech = "";


    void Start()
    {
        enemyAnim = transform.GetChild(0).gameObject;
        camera = transform.GetChild(1).gameObject;
        anim = GetComponent<Animator>();

        dicWordAction = new Dictionary<string, System.Action>();
        dicWordAction.Add("Teresa Teresa Teresa", OpenDoor);
        keywordRecognizer = new KeywordRecognizer(dicWordAction.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognzeWord;
        keywordRecognizer.Start();

        //Reconocimiento de Voz activa
        /*dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.DictationResult += OnSpeechRecognized;
        dictationRecognizer.DictationHypothesis += OnSpeechHypothesis;
        dictationRecognizer.DictationComplete += OnDictationComplete;
        dictationRecognizer.DictationError += OnDictationError;
        dictationRecognizer.Start();*/
    }

    private void RecognzeWord(PhraseRecognizedEventArgs word)
    {
        Debug.Log(word.text);
        dicWordAction[word.text].Invoke();
    }

    private void OpenDoor()
    {
        if (!actionDone & playerInPosition)
        {
            actionDone = true;
            StartCoroutine(AnimationDone());
            StartCoroutine(RecoverPlayer());
            camera.SetActive(true);
            enemyAnim.SetActive(true);
            enemyAnim.GetComponent<AudioSource>().Play();
            player.SetActive(false);
            anim.SetBool("Action", true);
        }
    }
    IEnumerator RecoverPlayer()
    {
        yield return new WaitForSeconds(3f);
        player.SetActive(true);
        camera.SetActive(false);

    }
    IEnumerator AnimationDone()
    {
        yield return new WaitForSeconds(4f);
        enemyAnim.SetActive(false);
        door.OpenDoorWithSound();
        enemy.SetActive(true);
        GameProgress.SaveMilestone("puerta3");
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

    //Reconocimiento de Voz activa
    /*private void OnDictationComplete(DictationCompletionCause completionCause)
    {
        if (completionCause != DictationCompletionCause.Complete)
        {
            Debug.LogWarning("Dictation completed unexpectedly: " + completionCause);
            dictationRecognizer.Stop();
            dictationRecognizer.Start();
        }
    }*/

    /*private void OnDictationError(string error, int hresult)
    {
        Debug.LogError($"Dictation error: {error}; HResult = {hresult}");
        dictationRecognizer.Stop();
        dictationRecognizer.Start();
    }*/

    /*private void OnSpeechRecognized(string text, ConfidenceLevel confidence)
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

    }*/
    /*private void OnSpeechHypothesis(string text)
    {
        // Mostrar lo que se está reconociendo en tiempo real mientras se habla.
        //Debug.Log("Hablando: " + text);
    }*/

    /*private bool ContainsTwoWordsWithinMargin(string text, string word1, string word2, string word3)
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
    }*/

    /* private bool ContainsThreeWordsExact(string text, string word1, string word2, string word3)
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
     }*/
}
