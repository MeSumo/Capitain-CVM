using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInteractivity : MonoBehaviour
{

    /// <summary>
    /// Liste de GO représentant les boutons 
    /// </summary>
    [SerializeField]
    private Button[] _boutons;
    /// <summary>
    /// Liste des niveaux complétés
    /// </summary>
    private List<string> _niveauComplete;

    // Start is called before the first frame update
    void Start()
    {
        this._niveauComplete = new List<string>(GameManager.Instance.PlayerData.ListeNiveauComplete);
        if (this._niveauComplete.Contains("Level1"))
            _boutons[0].interactable = true;
        if (this._niveauComplete.Contains("Level2"))
            _boutons[1].interactable = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
