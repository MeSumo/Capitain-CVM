﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

/// <summary>
/// Représente un élément audio avec un nom (étiquette)
/// </summary>
[System.Serializable]
struct AudioElement // afin d'avoir un audio (background sonore) par scene
{
    public string Nom;
    public AudioClip Clip;
}

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// Représente la playliste de la trame sonore
    /// </summary>
    [SerializeField]
    private AudioElement[] _playlist;
    /// <summary>
    /// Représente le AudioMixer pour le son
    /// </summary>
    [SerializeField]
    private AudioMixerGroup _soundGroup;
    /// <summary>
    /// Distance maximale entre l'objet est la source audio
    /// à jouer
    /// </summary>
    [SerializeField]
    private float _effetDistanceMax;
    /// <summary>
    /// Représente le mixer du jeu
    /// </summary>
    [SerializeField]
    private AudioMixer _mixer;

    /// <summary>
    /// Référence vers l'audio source de l'objet
    /// </summary>
    private AudioSource _source;

    private void Start()
    {
        // Récupère la référence
        _source = this.gameObject.GetComponent<AudioSource>();
        SceneManager.activeSceneChanged += ChangementScene;
        // Pour l'exécuter imédiatement
        ChangementScene(new Scene(), SceneManager.GetActiveScene());

        // _mixer est de type AudioMixer
        _mixer.SetFloat("Master", GameManager.Instance.PlayerData.VolumeGeneral);
        _mixer.SetFloat("Musique", GameManager.Instance.PlayerData.VolumeMusique);
        _mixer.SetFloat("Effets", GameManager.Instance.PlayerData.VolumeEffet);
    }

    void ChangementScene(Scene current, Scene next)
    {
        // Joue la musique pour la scène
        AudioClip clip = null;
        foreach (AudioElement clipData in _playlist)
        {
            if (clipData.Nom.Equals(next.name))
                clip = clipData.Clip;
        }
        if (clip != null)
            _source.clip = clip;
        else
            _source.clip = _playlist[0].Clip;

        Play(0.3f);
    }

    public void StopAudio(float delay = 0)
    {
        if (delay == 0)
            _source.Stop();
        else
            StartCoroutine(FadeOut(delay));
    }

    private IEnumerator FadeOut(float FadeTime)
    {
        float startVolume = _source.volume;

        while (_source.volume > 0)
        {
            _source.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        _source.Stop();
        _source.volume = startVolume;
    }

    private IEnumerator FadeIn(float FadeTime)
    {
        float maxVolume = _source.volume;
        _source.volume = 0;
        _source.Play();

        while (_source.volume < maxVolume)
        {
            _source.volume += Time.deltaTime / FadeTime;

            yield return null;
        }
    }

    public void Play(float delay = 0)
    {
        if (delay == 0)
            _source.Play();
        else
            StartCoroutine(FadeIn(delay));
    }

    public AudioSource PlayClipAtPoint(AudioClip clip, Vector3 pos)
    {
        if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, pos) <= _effetDistanceMax)
        {
            GameObject go = new GameObject("Effet Source");
            go.transform.position = pos;
            AudioSource audio = go.AddComponent<AudioSource>();
            audio.clip = clip;
            audio.outputAudioMixerGroup = _soundGroup;
            audio.spatialBlend = 0;
            audio.Play();
            Destroy(go, clip.length);
            return audio;
        }

        return null;
    }
}
