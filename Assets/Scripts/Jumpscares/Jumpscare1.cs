using System.Collections;
using UnityEngine;

public class Jumpscare1 : MonoBehaviour
{
    public GameObject player;
    public GameObject image;
    private AudioSource audio;

    private bool actionDone = false;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!actionDone && other.name == "Player")
        {
            actionDone = true;
            DoAction();
        }
    }

    private void DoAction()
    {
        StartCoroutine(FinishAction());
        image.SetActive(true);
        image.GetComponent<Animator>().SetBool("Action",true);
        audio.Play();
        player.GetComponent<PlayerController>().canMove = false;
    }

    IEnumerator FinishAction()
    {
        yield return new WaitForSeconds(2.5f);
        image.SetActive(false);
        player.GetComponent<PlayerController>().canMove = true;

    }
}
