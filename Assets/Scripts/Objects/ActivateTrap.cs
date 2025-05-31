using UnityEngine;
using System.Collections;

public class ActivateTrap : MonoBehaviour
{

    public static event System.Action OnPlayerTakeDmgByTrapRequest;

    private Animator anim;
    private AudioSource audio;

    void Start()
    {
        anim = GetComponentInParent<Animator>();
        audio = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && other.transform == other.transform.root) {     
            anim.SetBool("Pressed", true);
            audio.Play();
            StartCoroutine(ResetPressed());
            OnPlayerTakeDmgByTrapRequest?.Invoke();
        }
    }

    private IEnumerator ResetPressed()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Pressed", false);
    }
}
