using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Text carrotText;
    public Button[] chooseLvlButtons;
    // Метод, который открывает сцену под номером index
    public void OpenScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("Carrots")) PlayerPrefs.SetInt("Carrots", 0);
        if (!PlayerPrefs.HasKey("Lvl")) PlayerPrefs.SetInt("Lvl", 1);

        for (int i = 0; i < chooseLvlButtons.Length; i++)
        {
            if (PlayerPrefs.GetInt("Lvl") > i) chooseLvlButtons[i].interactable = true;
            else chooseLvlButtons[i].interactable = false;
        }
    }

    void Update()
    {
        if (PlayerPrefs.HasKey("Carrots")) carrotText.text = PlayerPrefs.GetInt("Carrots").ToString();
    }
}
