using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public AudioSource audioSource;

    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private AudioClip betSound;
    [SerializeField] private AudioClip cardSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private AudioClip flipCardSound;
    public static SoundEffect Instance;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    public void ButtonSound()
    {
        audioSource.clip = buttonSound;
        audioSource.Play();
    }

    public void BetSound()
    {
        audioSource.clip = betSound;
        audioSource.Play();
    }

    public void DrawCardSound()
    {
        audioSource.clip = cardSound;
        audioSource.Play();
    }
    public void WinSound()
    {
        audioSource.clip = winSound;
        audioSource.Play();
    }
    public void LoseSound()
    {
        audioSource.clip = loseSound;
        audioSource.Play();
    }
    public void FlipCardSound()
    {
        audioSource.clip = flipCardSound;
        audioSource.Play();
    }
}
