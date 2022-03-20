using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectablesUI : MonoBehaviour
{
    /// <summary>
    /// Liste de GO représentant le nombre de chaque collectable collecté
    /// </summary>
    [SerializeField]
    private GameObject[] _textes;
    /// <summary>
    /// Liste du nombre de collectable collecté selon le playerData
    /// // index 0 -> convention, 1 -> cartemembre, 2 -> paycheck
    /// </summary>
    private int[] _nbCollectablesList;

    // Start is called before the first frame update
    void Start()
    {
        this._nbCollectablesList = GameManager.Instance.PlayerData.ListeNombreCollectables;
        for(int i = 0; i < _textes.Length; i++)
        {
            this._textes[i].GetComponent<TextMeshProUGUI>().text = _nbCollectablesList[i].ToString();
        }
    }

}
