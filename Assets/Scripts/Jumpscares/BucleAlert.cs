using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BucleAlert : MonoBehaviour
{
    private Animator anim;
    private AudioSource audio;
    private bool actionDone = false;
    private int cont = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
        audio = GetComponentInChildren<AudioSource>();
    }
    private void OnEnable()
    {
        PlayerTp.OnPlayerTpRequest += AnimateAgain;
    }
    private void OnDisable()
    {
        PlayerTp.OnPlayerTpRequest -= AnimateAgain;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!actionDone && other.name == "Player")
        {
            actionDone = true;
            DoAction();
        }
    }
    private void AnimateAgain(float f)
    {
        actionDone = false;
    }
    private void DoAction()
    {
        StartCoroutine(SwitchLight());
        audio.Play();
    }

    IEnumerator SwitchLight()
    {
        yield return new WaitForSeconds(0.3f);
        if (cont == 0)
        {
            anim.SetBool("Action", true);
        }
        audio.Play();
        if (cont < 2) {
            StartCoroutine(SwitchLight());
            cont++;
        } else
        {
            anim.SetBool("Action", false);
            cont = 0;
        }

    }

}
