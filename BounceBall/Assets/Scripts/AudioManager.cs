using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource backgroundMusic;

    void Awake()
    {
        var soundManagers = FindObjectsOfType<AudioManager>();
        if (soundManagers.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 배경 음악을 재생하는 함수
    private void Start()
    {
        backgroundMusic.Play();
    }
}
