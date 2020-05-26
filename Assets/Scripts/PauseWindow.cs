using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseWindow : MonoBehaviour
{
    private static PauseWindow instance;
    private void Awake()
    {
        instance = this;
        transform.Find("ResumeButton").GetComponent<Button>().onClick.AddListener(GameHandler.ResumeGame);
        transform.Find("QuitButton").GetComponent<Button>().onClick.AddListener(Application.Quit);
        Hide();
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    public static void ShowStatic()
    {
        instance.Show();
    }
    public static void HideStatic()
    {
        instance.Hide();
    }
}
