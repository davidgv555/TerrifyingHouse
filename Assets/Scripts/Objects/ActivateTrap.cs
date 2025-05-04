using UnityEngine;
using System.Collections;

public class ActivateTrap : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponentInParent<Animator>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.name == "Player") {
            anim.SetBool("Pressed", true);
            StartCoroutine(ResetPressed());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "Player")
        {
            anim.SetBool("Pressed", true);
            StartCoroutine(ResetPressed());
        }
    }

    private IEnumerator ResetPressed()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Pressed", false);
    }
}
