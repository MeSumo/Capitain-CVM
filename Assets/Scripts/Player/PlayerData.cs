using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Représente les données de jeu
/// </summary>
[System.Serializable]
public class PlayerData
{
    /// <summary>
    /// Niveau sélectionné par l'utilisateur pour le vol. général
    /// </summary>
    [Range(-80, 0)]
    private float _volumeGeneral = 0;
    public float VolumeGeneral { get { return _volumeGeneral; } set { _volumeGeneral = value; } }

    /// <summary>
    /// Niveau sélectionné par l'utilisateur pour le vol. de la musique
    /// </summary>
    [Range(-80, 0)]
    private float _volumeMusique = 0;
    public float VolumeMusique { get { return _volumeMusique; } set { _volumeMusique = value; } }

    /// <summary>
    /// Niveau sélectionné par l'utilisateur pour le vol. de la musique
    /// </summary>
    [Range(-80, 0)]
    private float _volumeEffet = 0;
    public float VolumeEffet { get { return _volumeEffet; } set { _volumeEffet = value; } }

    /// <summary>
    /// Représente le nombre de points de vie du personnage
    /// </summary>
    private int _vie;
    /// <summary>
    /// Représente le nombre d'énergie (entre 0 et 4)
    /// </summary>
    private int _energie;
    /// <summary>
    /// Représente le score obtenu
    /// </summary>
    private int _score;
    /// <summary>
    /// Liste des coffres ouverts dans le jeu
    /// </summary>
    private List<string> _chestOpenList;
    /// <summary>
    /// Représente le maximum d'énergie du personnage
    /// </summary>
    public const int MAX_ENERGIE = 4;
    /// <summary>
    /// Liste contenant le nombre cueilli pour chaque collectable
    /// </summary>
    private List<int> _nbCollectablesList;
    /// <summary>
    /// Liste contenant les niveaux complete
    /// </summary>
    private List<string> _niveauCompleteList;

    /// <summary>
    /// Permet d'identifier les actions sur le UI à réaliser
    /// lors de la perte d'énergie
    /// </summary>
    public System.Action UIPerteEnergie;
    /// <summary>
    /// Permet d'identifier les actions sur le UI à réaliser
    /// lors de la perte d'énergie
    /// </summary>
    public System.Action UIPerteVie;
    /// <summary>
    /// Permet d'identifier les actions à réaliser lors d'un gameover
    /// </summary>
    public System.Action Gameover;

    public int Energie { get { return this._energie; } }
    public int Vie { get { return this._vie; } }
    public int Score { get { return this._score; } }
    public string[] ListeCoffreOuvert { get { return this._chestOpenList.ToArray(); } }
    public int[] ListeNombreCollectables { get { return this._nbCollectablesList.ToArray(); } }
    public string[] ListeNiveauComplete { get { return this._niveauCompleteList.ToArray(); } }
    public PlayerData()
    {
        this._vie = 0;
        this._energie = 0;
        this._score = 0;
        this._volumeGeneral = 0;
        this._volumeMusique = 0;
        this._volumeEffet = 0;
        this.UIPerteEnergie = null;
        this.UIPerteVie = null;
        this.Gameover = null;
        this._chestOpenList = new List<string>();
        // index 0 -> convention, 1 -> cartemembre, 2 -> paycheck
        this._nbCollectablesList = new List<int>() {0, 0, 0};
        this._niveauCompleteList = new List<string>();
    }

    public PlayerData(int vie = 1, int energie = 2, int score = 0,
        float volumeGeneral = 0, float volumeMusique = 0, float volumeEffet = 0,
        System.Action uiPerteEnergie = null, System.Action uiPerteVie = null,
        System.Action gameOver = null, List<string> ChestList = null, 
        List<int> CollectablesList = null, List<string> NiveauList = null)
    {
        this._vie = vie;
        this._energie = energie;
        this._score = score;
        this._volumeGeneral = volumeGeneral;
        this._volumeMusique = volumeMusique;
        this._volumeEffet = volumeEffet;
        this.UIPerteEnergie += uiPerteEnergie;
        this.UIPerteVie += uiPerteVie;
        this.Gameover += gameOver;
        this._chestOpenList = new List<string>();
        if (ChestList != null)
            this._chestOpenList = ChestList;
        this._nbCollectablesList = new List<int>() {0, 0, 0};
        if (CollectablesList != null)
            this._nbCollectablesList = CollectablesList;
        this._niveauCompleteList = new List<string>();
        if (NiveauList != null)
            this._niveauCompleteList = NiveauList;
    }

    /// <summary>
    /// Diminue l'énergie du personnage
    /// </summary>
    /// <param name="perte">Niveau de perte (par défaut 1)</param>
    public void DecrEnergie(int perte = 1)
    {
        this._energie -= perte;
        this.UIPerteEnergie();
        if (this._energie <= 0)
        {
            this.DecrVie();
        }
    }

    /// <summary>
    /// Permet de réduire la vie d'un personnage
    /// </summary>
    public void DecrVie()
    {
        this._vie--;
        this.UIPerteVie();
        if (this._vie <= 0)
            this.Gameover();

        else
        {
            this.IncrEnergie(MAX_ENERGIE);
            GameManager.Instance.RechargerNiveau();
        }
    }

    /// <summary>
    /// Permet d'augmenter l'énergie jusqu'à MAX_ENERGIE
    /// </summary>
    /// <param name="gain">Gain d'augmentation</param>
    public void IncrEnergie(int gain)
    {
        this._energie += gain;
        if (this._energie > MAX_ENERGIE)
        {
            this._energie = 1;
            this.IncrVie();
        }
        
        this.UIPerteEnergie();
    }

    /// <summary>
    /// Permet d'augmenter la vie
    /// </summary>
    /// <param name="gain">Gain d'augmentation</param>
    public void IncrVie(int gain = 1)
    {
        this._vie += gain;
        this.UIPerteVie();
    }

    /// <summary>
    /// Augmente le score du joueur
    /// </summary>
    /// <param name="gain">Point gagné</param>
    public void IncrScore(int gain = 1)
    {
        this._score += gain;
    }

    /// <summary>
    /// Ajoute le nom du coffre à la liste
    /// </summary>
    /// <param name="nom">Nom du coffre à ajouter</param>
    public void AjouterCoffreOuvert(string nom)
    {
        this._chestOpenList.Add(nom);
    }

    /// <summary>
    /// Détermine si le coffre est contenu dans la liste
    /// des coffres ouverts
    /// </summary>
    /// <param name="nom">Nom du coffre à vérifier</param>
    /// <returns>true si le coffre est ouvert, false sinon</returns>
    public bool AvoirOuvertureCoffre(string nom)
    {
        return this._chestOpenList.Contains(nom);
    }

    /// <summary>
    /// Incrémente la quantité collectionnée de ce collectable
    /// </summary>
    /// <param name="collectable">Position du collectable dans la liste</param>
    public void IncrCollectable(int collectable)
    {
        this._nbCollectablesList[collectable] = (this._nbCollectablesList[collectable] += 1);
    }

    /// <summary>
    /// Ajoute le nom de la scene complete
    /// </summary>
    /// <param name="nom">Nom de la scene a ajouter</param>
    public void AjouterNiveauComplete(string nom)
    {
        if (!this._niveauCompleteList.Contains(nom))
            this._niveauCompleteList.Add(nom);
    }

}
