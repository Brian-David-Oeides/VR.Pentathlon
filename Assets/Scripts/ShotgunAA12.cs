using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunAA12 : MonoBehaviour
{
    public AudioSource gunshotSource1; //reference to Audio clip one
    public AudioSource gunshotSource2; //reference to Audio clip two
    public AudioSource shellEjectSource; //reference to Audio clip three
    public AudioSource shellDropSource; //reference t0 Audio clip four

    // reference to AudioClips
    public AudioClip gunshot1;
    public AudioClip gunshot2;
    public AudioClip shellEject;
    public AudioClip shellDrop;

    void Start()
    {
        // store AudioSources in array
        AudioSource[] sources = GetComponents<AudioSource>();

        //if length of array greater than or equal to 2, assign them
        if (sources.Length >= 4)
        {
            gunshotSource1 = sources[0]; // shot
            gunshotSource2 = sources[1]; // shot reverb
            shellEjectSource = sources[2]; // shell eject
            shellDropSource = sources[3]; // shell drop
        }
    }

    public void FireShotgun()
    {
        gunshotSource1.Play();
        gunshotSource2.Play();
        shellEjectSource.Play();

        // Play shell ejection sound instantly without waiting
        shellEjectSource.PlayOneShot(shellEject);
    }
}