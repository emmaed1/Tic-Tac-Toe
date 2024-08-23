using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    [SerializeField]
    public Button MenuBtn, PlayAgainBtn;

    public void Setup(string score)
    {
        gameObject.SetActive(true);
        scoreText.text = score.ToString();
    }

    public void PlayAgain()
    {
        
        SceneManager.LoadScene("playerSelect");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
