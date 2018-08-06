using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Esc : MonoBehaviour
{
    [SerializeField] bool isMenu;
	
	void Update ()
    {
        Escape();
    }

    private void Escape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isMenu)
            {
                Application.Quit();
            }
            else
            {
                SceneManager.LoadScene("Menu");
            }
        }
    }
}
