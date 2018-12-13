using System;
using UnityEngine;


public class Sounds
{
    private AudioSource audioSource;

    private AudioClip itemShufflingClip;
    private AudioClip matchMadeClip;
    private AudioClip matchNotMadeClip;
    private AudioClip gameOverClip;
    public Sounds(AudioSource audioSource)
    {
        this.audioSource = audioSource;
        itemShufflingClip = Resources.Load<AudioClip>(Res.ItemShufflingClip);
        matchMadeClip = Resources.Load<AudioClip>(Res.MatchMadeClip);
        matchNotMadeClip = Resources.Load<AudioClip>(Res.MatchNotMadeClip);
        gameOverClip = Resources.Load<AudioClip>(Res.GameOverClip);
    }

    public void PlayItemShufflingSound()
    {
        audioSource.clip =itemShufflingClip;
        audioSource.loop = true;
        audioSource.Play();

    }

    public void StopItemShufflingSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void PlayMatchMadeSound()
    {
        audioSource.clip = matchMadeClip;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void PlayMatchNotMadeSound()
    {
        audioSource.clip = matchNotMadeClip;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void PlayGameOverSound()
    {
        audioSource.clip = gameOverClip;
        audioSource.loop = true;
        audioSource.Play();
    }
}
