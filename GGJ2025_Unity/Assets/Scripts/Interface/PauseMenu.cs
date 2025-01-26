using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup; 
    private bool _paused;

    private void Start()
    {
        _paused = false;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 0;
    }

    public void Quit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (_paused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        _paused = true;
        _canvasGroup.blocksRaycasts = true;
        var fade = _canvasGroup.DOFade(1.0f, 0.15f);
        fade.timeScale = 1;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        _paused = false;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.DOFade(0.0f, 0.15f);
        Time.timeScale = 1;
    }
}