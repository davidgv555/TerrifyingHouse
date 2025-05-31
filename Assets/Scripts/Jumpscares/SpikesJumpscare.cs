using System.Collections;
using UnityEngine;

public class SpikesJumpscare : MonoBehaviour
{

    public GameObject player;
    public GameObject images;
    public AudioClip[] audioClips;
    public GameObject linterna;
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

    public void DoAction()
    {
        StartCoroutine(ChangeImage1());
        images.transform.GetChild(2).gameObject.SetActive(true);
        audio.clip = audioClips[2];
        audio.Play();
        player.GetComponent<PlayerController>().canMove = false;
        linterna.transform.GetChild(0).GetComponent<Light>().intensity = 500;
    }

    IEnumerator ChangeImage1()
    {
        yield return new WaitForSeconds(0.5f);
        player.GetComponent<PlayerController>().canMove = true;
        images.transform.GetChild(2).gameObject.SetActive(false); ;

        /*yield return new WaitForSeconds(1f);
        StartCoroutine(ChangeImage2());
        audio.clip = audioClips[1];
        audio.Play();
        images.transform.GetChild(1).gameObject.SetActive(true);
        images.transform.GetChild(0).gameObject.SetActive(false);*/

    }
    /*
    IEnumerator ChangeImage2()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(ChangeImage3());
        audio.clip = audioClips[2];
        audio.Play();
        images.transform.GetChild(2).gameObject.SetActive(true);
        images.transform.GetChild(1).gameObject.SetActive(false);

    }
    IEnumerator ChangeImage3()
    {
        yield return new WaitForSeconds(0.5f);
        player.GetComponent<PlayerController>().canMove = true;
        images.transform.GetChild(2).gameObject.SetActive(false); ;

    }*/
}
