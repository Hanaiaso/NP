using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource effectAudioSource;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip reLoadClip;
    [SerializeField] private AudioClip energyClip;
    [SerializeField] private AudioSource defaultAudioSource;
    [SerializeField] private AudioSource bosstAudioSource;

    public void PlayShootSound()
    {
        effectAudioSource.PlayOneShot(shootClip);
    }
    public void PlayReloadSound()
    {
        effectAudioSource.PlayOneShot(reLoadClip);
    }
    public void PlayEnergySound()
    {
        effectAudioSource.PlayOneShot(energyClip);
    }
    public void PlayDefaultAudio()
    {
        bosstAudioSource.Stop();
        defaultAudioSource.Play();
    }
    public void PlayBossAudio()
    {
        defaultAudioSource.Stop();
        bosstAudioSource.Play();
    }
    public void StopAudioGame()
    {
        effectAudioSource.Stop();
        defaultAudioSource.Stop();
        bosstAudioSource.Stop();
    }
}
