using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntermittentSounds : MonoBehaviour
{
    [SerializeField]
    private AudioSource _Speaker01;
    private AudioLowPassFilter _lpFilter;
    [Range(0f, 1f)]
    public float minVol, maxVol, SourceVol;
    [Range(0f, 30f)]
    public float minTime, maxTime;
    [Range(0, 50)]
    public int distRand, maxDist;
    [Range(0f, 1.1f)]
    public float spatialBlend;

    public AudioClip[] pcmData;
    public bool enablePlayMode;
    private AudioRolloffMode sourceRolloffMode = AudioRolloffMode.Custom;

    void Awake()
    {
    _Speaker01 = GetComponent<AudioSource>();
    if (_Speaker01 == null)
        {   
        _Speaker01 = gameObject.AddComponent<AudioSource>();
        }
    }

void Start()
    {
    _Speaker01.playOnAwake = false;
    _Speaker01.loop = false;
    _Speaker01.volume = 0.1f;
    }
// Update is called once per frame

void Update()
{
    if (!enablePlayMode)
    {
        Debug.Log("NotPlaying");

    if (Input.GetKeyDown(KeyCode.Alpha1))
        {
        enablePlayMode = true;
        StartCoroutine("Waitforit");
        }
    }
    else if (enablePlayMode)
    {
    
    if (Input.GetKeyDown(KeyCode.Alpha2))
    {
//enablePlayMode = false;
StopSound();
    }
    }
}

public void SetSourceProperties(AudioClip audioData, float minVol, float maxVol, int minDist, int maxDist, float SpatialBlend)
    {
    _Speaker01.loop = false;
    _Speaker01.maxDistance = maxDist - Random.Range(0f, distRand);
    _Speaker01.rolloffMode = sourceRolloffMode;
    _Speaker01.spatialBlend = spatialBlend;
    _Speaker01.clip = audioData;
    _Speaker01.volume = SourceVol + Random.Range(minVol, maxVol);
    }

    void PlaySound()
    {
        SetSourceProperties(pcmData[Random.Range(0, pcmData.Length)], minVol, maxVol, distRand, maxDist, spatialBlend);
        _Speaker01.Play();
        Debug.Log("back in it");
        StartCoroutine("Waitforit");
    }

    IEnumerator Waitforit()
    {
    float waitTime = Random.Range(minTime, maxTime);
    Debug.Log(waitTime);

    if (_Speaker01.clip == null) //used for the first time, before a clip has been assigned, just use the random time value.
           {
    yield return new WaitForSeconds(waitTime);
         }

    else // Once a clip has been assigned, add the cliptlength’s to the random time interval for the wait between clips.
       
         {
    yield return new WaitForSeconds(_Speaker01.clip.length + waitTime);
        }

    if (enablePlayMode)
        {
        PlaySound();
        }
    }

    void StopSound()
    {
    enablePlayMode = false;
    Debug.Log("stop");
    }
}
