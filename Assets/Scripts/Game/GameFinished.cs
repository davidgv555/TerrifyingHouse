using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFinished : MonoBehaviour
{
    public GameObject player;
    public GameObject image;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            DoAction();
        }
    }

    private void DoAction()
    {
        StartCoroutine(FinishAction());
        image.SetActive(true);
        image.GetComponent<Animator>().SetBool("Action", true);
        player.GetComponent<PlayerController>().canMove = false;
    }

    IEnumerator FinishAction()
    {
        yield return new WaitForSeconds(10f);
        GameProgress.ResetMilestones();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MenuInicial");
    }
}
