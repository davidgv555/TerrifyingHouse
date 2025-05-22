using UnityEngine;
using System.Collections;

public class Revealer : MonoBehaviour
{
    public Material hiddenMaterial;
    public Material revealedMaterial;
    public float revealDuration = 1f;

    private Renderer render;
    private Coroutine hideCoroutine;

    void Start()
    {
        render = GetComponent<Renderer>();
    }



    public void Reveal()
    {
        render.material = revealedMaterial;

        /*if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }*/

    }
    public void Hide()
    {
        //hideCoroutine = StartCoroutine(HideAfterDelay());
        render.material = hiddenMaterial;
    }

 
    /*private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(revealDuration);
        DesactiveReveal();
        hideCoroutine = null; 
    }*/

}
