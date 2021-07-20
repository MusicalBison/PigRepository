using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Text carrotText;
    public Player player;
    public Image[] hearts; // Массив сердечек
    public Sprite luis_hp, ldis_hp, rdis_hp, ruis_hp, luno_hp, ldno_hp, rdno_hp, runo_hp;


    public GameObject winPanel, losePanel, losePanel2, enterPanel, barrierPanel;

    public GameObject exit;

    void Start()
    {
        if (!PlayerPrefs.HasKey("Carrots")) PlayerPrefs.SetInt("Carrots", 0);
        if (!PlayerPrefs.HasKey("Lvl")) PlayerPrefs.SetInt("Lvl", 1);
        Time.timeScale = 0;
        player.enabled = false; 
    }

    // Метод, при вызове которого текущая сцена перезагружается
    public void Lose()
    {
        losePanel.SetActive(true);
        Time.timeScale = 0;
        player.enabled = false;
    }

    public void Win()
    {
        if (!player.isLeaves) losePanel2.SetActive(true);
        else
        {
            winPanel.SetActive(true);
            PlayerPrefs.SetInt("Carrots", PlayerPrefs.GetInt("Carrots") + player.GetCarrots());
            if (GetActiveSceneIndex() >= PlayerPrefs.GetInt("Lvl")) PlayerPrefs.SetInt("Lvl", PlayerPrefs.GetInt("Lvl") + 1);
        }

        Time.timeScale = 0;
        player.enabled = false;
    }

    public void NextLvl()
    {
        SceneManager.LoadScene(GetActiveSceneIndex() + 1);
    }    

    void Update()
    {
        carrotText.text = player.GetCarrots().ToString();
        for (int i = 0; i < hearts.Length; i++)
        {
            if (player.GetHearts() >= i + 1)
            {
                if (hearts[i].name == "left_up") hearts[i].sprite = luis_hp;
                else if (hearts[i].name == "left_down") hearts[i].sprite = ldis_hp;
                else if (hearts[i].name == "right_down") hearts[i].sprite = rdis_hp;
                else if (hearts[i].name == "right_up") hearts[i].sprite = ruis_hp;

            }
            else
            {
                if (hearts[i].name == "left_up") hearts[i].sprite = luno_hp;
                else if (hearts[i].name == "left_down") hearts[i].sprite = ldno_hp;
                else if (hearts[i].name == "right_down") hearts[i].sprite = rdno_hp;
                else if (hearts[i].name == "right_up") hearts[i].sprite = runo_hp;
            }
        }
        /*if (player.GetHearts() > 0)
        {
            hearts[0].sprite = is_hp;
            if (player.GetHearts() > 1)
            {
                hearts[1].sprite = is_hp;
                if (player.GetHearts() > 2)
                {
                    hearts[2].sprite = is_hp;
                }
                else hearts[2].sprite = no_hp;
            }
            else hearts[1].sprite = no_hp;
        }
        else hearts[0].sprite = no_hp;
        */
    }

    public void OpenScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Enter()
    {
        Time.timeScale = 1;
        player.enabled = true;
        enterPanel.SetActive(false);
    }

    public void Barrier()
    {
        Time.timeScale = 0;
        player.enabled = false;
        barrierPanel.SetActive(true);
    }

    public void ToExit()
    {
        player.transform.position = new Vector3(exit.transform.position.x, exit.transform.position.y, player.transform.position.z);
        Time.timeScale = 1;
        player.enabled = true;
        barrierPanel.SetActive(false);
    }   
    
    public int GetActiveSceneIndex()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        return index;
    }
}
