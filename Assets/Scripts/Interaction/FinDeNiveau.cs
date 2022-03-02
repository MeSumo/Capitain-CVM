using UnityEngine;
using UnityEngine.SceneManagement;

public class FinDeNiveau : MonoBehaviour
{
    // verifie si player rentre en collision avec bloc de fin
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Félicitation, le niveau est terminé.");
            GameManager.Instance.SaveData();
            SceneManager.LoadScene("MainMenu");
        }
    }
}
