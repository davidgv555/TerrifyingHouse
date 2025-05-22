using UnityEngine;
using System.Collections;

public class ActivateTrap : MonoBehaviour
{
    private Animator anim;
    public static event System.Action OnPlayerTakeDmgByTrapRequest;

    void Start()
    {
        anim = GetComponentInParent<Animator>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && other.transform == other.transform.root) {     
            anim.SetBool("Pressed", true);
            StartCoroutine(ResetPressed());
            OnPlayerTakeDmgByTrapRequest?.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.transform == other.transform.root)
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
