using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class advanceLevel : MonoBehaviour
{
	void Start ()
    {
        Invoke("advanceToMainMenu", 5f);
		
	}

    private void advanceToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
