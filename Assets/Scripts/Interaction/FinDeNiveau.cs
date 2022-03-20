using UnityEngine;
using UnityEngine.SceneManagement;

public class FinDeNiveau : MonoBehaviour
{
    /// <summary>
    /// Nom de la scene qu'on vient de completer
    /// </summary>
    private string _name;

    // verifie si player rentre en collision avec bloc de fin
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Félicitation, le niveau est terminé.");
            _name = SceneManager.GetActiveScene().name;
            GameManager.Instance.PlayerData.AjouterNiveauComplete(_name);
            GameManager.Instance.SaveData();
            if (SceneManager.GetActiveScene().buildIndex == 3) // si rendu au niveau 3, retourne au menu
                SceneManager.LoadScene("MainMenu");
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // load prochain niveau
        }
    }
}
