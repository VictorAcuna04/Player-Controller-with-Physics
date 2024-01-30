using UnityEngine;

public class TeleportationScript : MonoBehaviour
{
    public Transform teleportDestination;
    public AudioClip newSong;
    private AudioSource audioSource;

    private void Start()
    {

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();

        if (playerRigidbody != null)
        {
            StopCurrentSong();
            PlayNewSong();
            TeleportPlayer(other.transform);
        }
    }

    private void TeleportPlayer(Transform playerTransform)
    {
        playerTransform.position = teleportDestination.position;
    }

    private void StopCurrentSong()
    {
        // Check if there is a current song playing and stop it
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void PlayNewSong()
    {
        // Check if a new song is assigned, then play it
        if (newSong != null)
        {
            audioSource.clip = newSong;
            audioSource.Play();
        }
    }
}
