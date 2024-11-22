using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;  // Music AudioSource
    [SerializeField] private AudioSource SFKSource;   // SFK (Sound Effects or other) AudioSource
    [SerializeField] private AudioClip background;    // Background music clip

    private void Start()
    {
        // Check if musicSource is assigned in the Inspector
        if (musicSource != null && background != null)
        {
            musicSource.clip = background;
            musicSource.loop = true; // Enable looping for background music
            musicSource.Play(); // Play the background music
        }
        else
        {
            Debug.LogError("Missing AudioSource or AudioClip assignment!");
        }
    }

    // You can also add methods for controlling SFKSource or changing volume, muting, etc.
    public void PlaySFX(AudioClip sfx)
    {
        if (SFKSource != null)
        {
            SFKSource.PlayOneShot(sfx);
        }
    }
}
