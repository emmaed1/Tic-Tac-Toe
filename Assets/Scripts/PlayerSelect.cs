using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelect : MonoBehaviour
{
    public GameObject persistantObj;
    public string gameMode;

    public void onClicked()
    {
        persistantObj.GetComponent<PersistantObject>().gameMode = gameMode;
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
