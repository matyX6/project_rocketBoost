using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{

	void Start ()
    {
        Invoke("LoadMenuScene", 3f);
	}

    private void LoadMenuScene() //string referenced
    {
        SceneManager.LoadScene("Menu");
    }
}
