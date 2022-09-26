using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip _jumpClip;
    [SerializeField] private AudioClip _pickUpCheeseClip;
    [SerializeField] private AudioClip _deathClip;

    private AudioSource _playerAudioSource;

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _playerAudioSource = GetComponent<AudioSource>();
    }

    public void PlayJumpSound()
    {
        _playerAudioSource.Stop();
        _playerAudioSource.PlayOneShot(_jumpClip);
    }

    public void PlayPickUpCheeseSound()
    {
        _playerAudioSource.Stop();
        _playerAudioSource.PlayOneShot(_pickUpCheeseClip);
    }

    public void PlayDeathSound()
    {
        _playerAudioSource.Stop();
        _playerAudioSource.PlayOneShot(_deathClip);
    }
}
