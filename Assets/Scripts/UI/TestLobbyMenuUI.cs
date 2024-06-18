using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestLobbyMenuUI : MonoBehaviour
{
    [SerializeField] private Button enterMatchButton;

    private void Awake()
    {
        enterMatchButton.onClick.AddListener(() => 
        {
            Loader.Instance.Load(Loader.Scene.PlayersWaitScene);
        });
    }
}
