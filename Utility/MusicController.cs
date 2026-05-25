using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class MusicController : MonoBehaviour
{
    public AudioClip[] ambientMusic;
    public AudioClip[] combatMusic;
    public AudioClip loadingScreen;

    private AudioSource audioPlayer;

    private void Awake() {
        audioPlayer = GetComponent<AudioSource>();
        int clipNumber = Random.Range(0, ambientMusic.Length - 1);
        audioPlayer.clip = ambientMusic[clipNumber];
        audioPlayer.Play();
    }
}
