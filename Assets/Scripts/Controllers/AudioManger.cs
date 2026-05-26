using UnityEngine;

public class AudioManger : MonoBehaviour
{
    public static AudioManger Instance;

    public AudioSource audioSource;
    public AudioClip buttonClip;

    void Awake()
    {   
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayClick()
    {
        audioSource.PlayOneShot(buttonClip);
    }
    
}
