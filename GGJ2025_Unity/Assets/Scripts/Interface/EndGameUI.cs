using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    private void Start()
    {
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 0;
    }

    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Show()
    {
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.DOFade(1.0f, 0.15f);
    }
}