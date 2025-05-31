using System.Collections;
using UnityEngine;

public class CatJumpscare : MonoBehaviour
{
    public ActionDoor door;
    public GameObject cat;

    private Animator anim;
    private bool actionDone = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (actionDone) return;

        if (door.isOpen)
        {
            actionDone = true;
            DoAction();
        }
    }
    private void DoAction()
    {
        StartCoroutine(AnimationDone());
        StartCoroutine(CatSound());
        cat.SetActive(true);
        anim.SetBool("Action", true);
    }
    IEnumerator CatSound()
    {
        yield return new WaitForSeconds(0.5f);
        cat.GetComponent<AudioSource>().volume = 5f;
        cat.GetComponent<AudioSource>().Play();

    }
    IEnumerator AnimationDone()
    {
        yield return new WaitForSeconds(4f);
        cat.SetActive(false);

    }

}
