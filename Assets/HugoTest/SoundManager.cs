using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip clip1;
    public AudioClip clip2;

    private AudioClip clipFinal;

    private void Start()
    {
        clipFinal = Combine(clip1, clip2);
        audioSource.clip = clipFinal;
        if (audioSource.clip == null) audioSource.Play();
        else Debug.Log("No clip assigned !");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            audioSource.Play();
        }
    }

    public static AudioClip Combine(params AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
            return null;

        int length = 0;
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i] == null)
                continue;

            length += clips[i].samples * clips[i].channels;
        }

        float[] data = new float[length];
        length = 0;
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i] == null)
                continue;

            float[] buffer = new float[clips[i].samples * clips[i].channels];
            clips[i].GetData(buffer, 0);
            //System.Buffer.BlockCopy(buffer, 0, data, length, buffer.Length);
            buffer.CopyTo(data, length);
            length += buffer.Length;
        }

        if (length == 0)
            return null;

        //AudioClip result = AudioClip.Create("Combine", length / 2, 2, 44100, false, false);
        AudioClip result = AudioClip.Create("Combined", length / 2, 2, 44100, false);
        result.SetData(data, 0);

        return result;
    }

}
