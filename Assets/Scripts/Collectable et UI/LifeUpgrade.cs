using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUpgrade : MonoBehaviour
{
    /// <summary>
    /// Valeur de la vie regagner au contact
    /// </summary>
    [SerializeField]
    private int _regainLife = 1;
    [SerializeField]
    private AudioClip _clip;

    /// <summary>
    /// Nom du paycheck utiliser dans le fichier de sauvegarde
    /// </summary>
    private string _name;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            GameManager.Instance.AudioManager
                .PlayClipAtPoint(_clip, this.transform.position);
            GameManager.Instance
                .PlayerData.IncrVie(this._regainLife);
            GameManager.Instance.PlayerData.IncrCollectable(2);
            GameObject.Destroy(this.gameObject);
        }
    }
}
