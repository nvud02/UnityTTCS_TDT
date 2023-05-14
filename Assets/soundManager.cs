using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour
{
    public static soundManager Instance { get; private set; }
    private AudioSource source;
    private void Awake()
    {
        Instance = this;
        source = GetComponent<AudioSource>();
    }
    public void playSound(AudioClip _sound)
    {
        source.PlayOneShot(_sound);
    }

}
