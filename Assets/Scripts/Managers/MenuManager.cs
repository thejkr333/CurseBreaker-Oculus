using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] Slider sfx, music;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        var lights = FindObjectsOfType<Light>();

        foreach ( Light light in lights )
        {
            if(light.transform.parent.name != "PotV1") light.intensity = 2f;
        }
    }

    private void Start()
    {
        sfx.value = AudioManager.Instance.SfxVolume;
        music.value = AudioManager.Instance.MusicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        AudioManager.Instance.SetSFXVolume(volume);
    }

    public void SetMusicVolume(float volume)
    {
        AudioManager.Instance.SetMusicVolume(volume);
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        GameManager.Instance.LoadScene("Gameplay");
        GameManager.Instance.StartGame();
    }

    public void LoadGame()
    {
        GameManager.Instance.LoadScene("Gameplay");
        GameManager.Instance.StartGame();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
                 Application.Quit();
#endif
    }
}
