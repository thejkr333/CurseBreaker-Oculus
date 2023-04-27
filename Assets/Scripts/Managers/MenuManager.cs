using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
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
    }

    public void LoadGame()
    {
        GameManager.Instance.LoadScene("Gameplay");
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
