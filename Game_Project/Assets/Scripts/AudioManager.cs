using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource effectAudioSource;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip reLoadClip;
    [SerializeField] private AudioClip energyClip;
    [SerializeField] private AudioClip EventChestOpenClip;
    [SerializeField] private AudioClip LevelUpClip;
    [SerializeField] private AudioClip enemyBoom;

    [SerializeField] private AudioSource defaultAudioSource;
    [SerializeField] private AudioSource bosstAudioSource;
    //[SerializeField] private AudioClip EventExplosionSource;
    [SerializeField] private AudioClip shopOpenClip;
   
    [SerializeField] private AudioClip upgradeHeathClip;
    [SerializeField] private AudioClip upgradeClickClip;
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

    //public void EventExplosion()
    //{
    //    effectAudioSource.PlayOneShot(EventExplosionSource);
    //}

    public void EventChestOpen()
    {
        effectAudioSource.PlayOneShot(EventChestOpenClip);
    }

    public void LevelUp()
    {
        effectAudioSource.PlayOneShot(LevelUpClip);
    }
    
    public void EnemyExplosion()
    {
        effectAudioSource.PlayOneShot(enemyBoom);
    }
    public void PlayShopOpenSound()
    {
        effectAudioSource.PlayOneShot(shopOpenClip);
    }

    public void PlayUpgradeSound()
    {
        
        effectAudioSource.PlayOneShot(upgradeClickClip);
    }

    public void HeartZoneSound()
    {

        effectAudioSource.PlayOneShot(upgradeClickClip);
    }
    public void PlayUpgradeHeath()
    {
        
        effectAudioSource.PlayOneShot(upgradeHeathClip);
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
