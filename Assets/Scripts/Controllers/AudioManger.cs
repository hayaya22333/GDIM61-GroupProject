using UnityEngine;

public class AudioManger : MonoBehaviour
{
    public static AudioManger Instance;

    //public AudioSource audioSource;
    //public AudioClip buttonClip;
    void Start()
    {
        StartController.Instance.PlayAudioPlease += PlayClick;
        PollController.pollController.PlayAudioPleaseP += PlayClick;
        PrepareController.Instance.PlayAudioPleasePP += PlayClick;
    }

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

    public void PlayClick(AudioSource audioSource, AudioClip buttonClip)
    {
        audioSource.PlayOneShot(buttonClip);
    }
}
