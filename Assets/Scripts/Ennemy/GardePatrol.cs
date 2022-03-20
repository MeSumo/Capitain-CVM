using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GardePatrol : MonoBehaviour
{
    /// <summary>
    /// Vitesse de l'objet en patrouille
    /// </summary>
    [SerializeField]
    private float _vitesse = 4f;
    /// <summary>
    /// Liste de GO représentant les points à atteindre
    /// </summary>
    [SerializeField]
    private Transform[] _points;
    /// <summary>
    /// Référence vers la cible actuelle de l'objet
    /// </summary>
    private Transform _cible = null;
    /// <summary>
    /// Permet de connaître la position actuelle de la cible dans le tableau
    /// </summary>
    private int _indexPoint;
    /// <summary>
    /// Seuil où l'objet change de cible de déplacement
    /// </summary>
    private float _distanceSeuil = 0.3f;
    /// <summary>
    /// Référence vers le sprite Renderer
    /// </summary>
    private SpriteRenderer _sr;
    /// <summary>
    /// Réfère à l'animator du GO
    /// </summary>
    private Animator _animator;
    /// <summary>
    /// Représente le début du temps à attendre avant de tourner
    /// </summary>
    private float _tempsDebutStop;
    /// <summary>
    /// Décrit la durée du temps à attendre avant de tourner
    /// </summary>
    public const float DelaisStop = 1.5f;
    /// <summary>
    /// En stop
    /// </summary>
    private bool _enStop = false;
    /// <summary>
    /// au point
    /// </summary>
    private bool _auPoint = false;

    // Start is called before the first frame update
    void Start()
    {
        _sr = this.GetComponent<SpriteRenderer>();
        _indexPoint = 0;
        _cible = _points[_indexPoint];
        _animator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = _cible.position - this.transform.position;
        if (!_enStop)
            this.transform.Translate(direction.normalized * _vitesse * Time.deltaTime, Space.World);

        // apres certains temps en stop, cherche autre cible pour commencer a marcher
        if (Time.fixedTime > _tempsDebutStop + DelaisStop && _auPoint)
        {
            _enStop = false;
            _auPoint = false;
            _indexPoint = (++_indexPoint) % _points.Length;
            _cible = _points[_indexPoint];
            Debug.Log("change");
        }
            

        if (direction.x < 0 && !_sr.flipX) _sr.flipX = true;
        else if (direction.x > 0 && _sr.flipX) _sr.flipX = false;

        // si arriver proche d'une cible, fait animation stop
        if (Vector3.Distance(this.transform.position, _cible.position) < _distanceSeuil && !_auPoint)
        {
            _tempsDebutStop = Time.fixedTime;
            _enStop = true;
            _auPoint = true;
            _animator.SetTrigger("StopTime");
            this.transform.Translate(direction.normalized * 0 * Time.deltaTime, Space.World);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // Ligne entre les cibles
        for (int i = 0; i < _points.Length - 1; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_points[i].position,
                _points[i + 1].position);
        }

        // Ligne entre l'ennemi et la cible
        if (_cible != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(this.transform.position, _cible.position);
        }
    }
#endif
}
