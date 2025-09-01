using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [Header("Audio clips")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameplyMusic;

    [Header("Sources")]
    [Tooltip("First source, for fading.")]
    [SerializeField] private AudioSource musicSourceA;
    [Tooltip("Second source, for fading.")]
    [SerializeField] private AudioSource musicSourceB;

    [Header("Settings")]
    [Tooltip("Fade time in s.")]
    [SerializeField] private float fadeDuration = 1.0f;

    private AudioSource activeMusic;

    private void OnEnable()
    {
        GameManager.OnStateChanged += HandleStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnStateChanged -= HandleStateChanged;
    }

    void HandleStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Playing)
        {
            PlayMusic(gameplyMusic);
        }
        else
        {
            PlayMusic(menuMusic);
        }
    }

    private void PlayMusic(AudioClip clip)
    {
        if (activeMusic != null && activeMusic.clip == clip && activeMusic.isPlaying)
        {
            return;
        }

        // Determine active and inactive
        AudioSource sourceFadeOut = (activeMusic == musicSourceA) ? musicSourceA : musicSourceB;
        AudioSource sourceFadeIn = (activeMusic == musicSourceA) ? musicSourceB : musicSourceA;

        activeMusic = sourceFadeIn;

        StartCoroutine(CrossfadeRoutine(sourceFadeIn, sourceFadeOut, clip));
    }

    private IEnumerator CrossfadeRoutine(AudioSource fadeIn, AudioSource fadeOut, AudioClip clip)
    {
        // start new clip on fadein src
        fadeIn.clip = clip;
        fadeIn.volume = 0;
        fadeIn.Play();

        float elapsedTime = 0f;
        float startFadeOutVol = fadeOut.volume;

        while (elapsedTime < fadeDuration)
        {
            // calc volume for new sources
            fadeIn.volume = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            fadeOut.volume = Mathf.Lerp(startFadeOutVol, 0, elapsedTime / fadeDuration);

            elapsedTime += Time.deltaTime;
            yield return null; //wait
        }

        // ensure final volumes
        fadeIn.volume = 1;
        fadeOut.Stop();
        fadeOut.volume = 0;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
