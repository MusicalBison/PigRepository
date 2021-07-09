using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Метод, который открывает сцену под номером index
    public void OpenScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
