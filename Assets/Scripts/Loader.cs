using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public static Loader Instance;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public enum Scene
    {
        MainMenuScene,
        LobbyScene,
        PlayersWaitScene,
        GameScene,
    }
    public void Load(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }
}
