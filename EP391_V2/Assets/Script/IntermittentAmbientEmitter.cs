using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is versatile and can be used in any game objects, it's a never ending loop that plays ambient emitters

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioLowPassFilter))]

public class IntermittentAmbientEmitter : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] AudioClip[] _clips; //public diff than serialised, you can serialized to xml, json files, text notation that represents things in the game world
    // Unity by default evry public has been serialized, shows in inspector, but not technically public, it's private and safe

    [Space(5)]
    [Header("Randomize Attribute: Amplitude")]
    [SerializeField] bool _randomizeAmplitude = true; 
    [Range(0.25f, 0.75f)] //Sliders, try not to overcross minimum and maximum in the sliders 
    [SerializeField] float _minAmplitude = 0.75f;
    [Range(0.75f, 1.0f)]
    [SerializeField] float _maxAmplitude = 1.0f;

    [Space(5)]
    [Header("Randomize Attribute: Pitch")]
    [SerializeField] bool _randomizePitch = true;
    [Range(0.5f, 1.0f)] 
    [SerializeField] float _minPitch = 0.8f;
    [Range(1.0f, 2.0f)]
    [SerializeField] float _maxPitch = 1.2f;

    [Space(5)]
    [Header("Randomize Attribute: Low Pass Filter")]
    [SerializeField] bool _randomizeCutoff = true;
    [Range(200.0f, 12000.0f)]
    [SerializeField] float _minCutoff = 8000.0f;
    [Range(12000.0f, 22000.0f)]
    [SerializeField] float _maxCutoff = 20000.0f;

    [Space(5)]
    [Header("Randomize Attribute: Time Between Clips")]
    [Range(0.5f, 5.0f)]
    [SerializeField] float _maxTimeGapBetweenClips = 2.0f;

    [Space(5)]
    [Header("Randomize Transform")]
    [SerializeField] Transform _playerTransform;
    [SerializeField] bool _randomizePosition = true;
    [Range(0.1f, 20.0f)]
    [SerializeField] float _minRadius = 3.0f;
    [Range(20.0f, 50.0f)]
    [SerializeField] float _maxRadius = 30.0f;

    AudioSource _source;
    AudioLowPassFilter _lpf;
    
    int _currentIndex;

    void Start()
    {
        _source = GetComponent<AudioSource>();
        _lpf = GetComponent<AudioLowPassFilter>(); 

        _source.loop = false;
        _source.spatialBlend = 1.0f; //3D
        _source.maxDistance = _maxRadius; // + offset (if you want) 

        SetAudioProperties();
    }

    /// <summary>
    /// Pick an audio clip, and randomize parameters so that it can be played. 
    /// </summary>
    void SetAudioProperties()
    {
        // To avoid repetition. update the previously selected index, needs two lines 
        _currentIndex = LoadRandomIndex(_clips.Length, _currentIndex);
        Debug.Log(_currentIndex);

        _source.clip = _clips[_currentIndex];

        // Randomize AudioCourse attributes
        // Amplitude
        if (_randomizeAmplitude)
            _source.volume = Random.Range(_minAmplitude, _maxAmplitude);

        if (_randomizePitch)
            _source.pitch = Random.Range(_minPitch, _maxPitch);

        if (_randomizeCutoff)
            _lpf.cutoffFrequency = Random.Range(_minCutoff, _maxCutoff);

        if (_randomizePosition)
            transform.position = GenerateRelativeRandomPos(_playerTransform, _minRadius, _maxRadius);

        // Tell unity we are using a coroutine 
        StartCoroutine(PlaySoundAndWait());  
    }

    //Coroutine 
    IEnumerator PlaySoundAndWait()
    {
        _source.Play();

        // Skip a frame, when the loop reaches this function again, it will execute from this point onwards 
        yield return null;

        //interupter: yield, skip number of frames but using seconds  
        yield return new WaitForSeconds(
            Random.Range(_source.clip.length, _source.clip.length + _maxTimeGapBetweenClips)
        );

        SetAudioProperties(); 
    }

    /// <summary>
    /// Function to pick a random index different from the previously selected index
    /// </summary>
    /// <param name="arrayLength"></param>
    /// <param name="previousIndex"></param>
    /// <returns></returns>
    public static int LoadRandomIndex(int arrayLength, int previousIndex)
    {
        int currentIndex;
        // do while is a varition of a while loop, but it only executes once then checks, it's an exit controlled loop rather than entry controlled loop
        // 'Passing condition' is when a condition is true 
        do
        {
            currentIndex = Random.Range(0, arrayLength);
        } while (previousIndex == currentIndex);

        return currentIndex; 
    }

    public static Vector3 GenerateRelativeRandomPos(Transform pos, float minRadius, float maxRadius) //A class function, not particularly related to an object
    {
        // meter
        float radius = Random.Range(minRadius, maxRadius);

        float angleHorizontal = Random.Range(0, 2 * Mathf.PI);
        float angleVertical = Random.Range(0, Mathf.PI); //

        return new Vector3(
            pos.position.x + Mathf.Cos(angleHorizontal) * radius, // cos and sin only take in radians
            pos.position.y + Mathf.Sin(angleVertical) * radius, // only sin is all positive values
            pos.position.z + Mathf.Sin(angleHorizontal) * radius
        );                                                          

    }
}
