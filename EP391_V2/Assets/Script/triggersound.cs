using UnityEngine;

public class PlayAudioOnTriggerEnter : MonoBehaviour
{
    public AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Play the audio source when the player enters the trigger zone
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
    }
}
