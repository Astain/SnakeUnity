using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuWindow : MonoBehaviour
{
    private void Awake()
    {
        transform.Find("PlayButton").GetComponent<Button>().onClick.AddListener(() => Loader.Load(Loader.Scene.GameScene));
        transform.Find("QuitButton").GetComponent<Button>().onClick.AddListener(Application.Quit);
    }
}
