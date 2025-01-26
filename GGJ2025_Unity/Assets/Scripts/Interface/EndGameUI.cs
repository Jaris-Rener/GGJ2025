using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private GraphHandler _graphPrefab;
    [SerializeField] private CanvasGroup _canvasGroup;
    
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _score;
    
    [SerializeField] private TextMeshProUGUI _beachBoughtCount;
    [SerializeField] private TextMeshProUGUI _suburbBoughtCount;
    [SerializeField] private TextMeshProUGUI _cityBoughtCount;
    
    [SerializeField] private TextMeshProUGUI _beachSoldCount;
    [SerializeField] private TextMeshProUGUI _suburbSoldCount;
    [SerializeField] private TextMeshProUGUI _citySoldCount;

    [SerializeField] private TextMeshProUGUI _grossIncome;
    [SerializeField] private TextMeshProUGUI _totalSpend;
    [SerializeField] private TextMeshProUGUI _totalTaxed;

    private void Start()
    {
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 0;

        GlobalStepManager.Instance.OnEndStep += Show;
    }

    private void OnDestroy()
    {
        GlobalStepManager.Instance.OnEndStep -= Show;
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Show()
    {
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.DOFade(1.0f, 0.15f);

        bool win = PlayerAssetManager.Instance.money >= PlayerAssetManager.Instance.startingMoney;
        
        // populate info
        _title.text = win ? "Venture Complete" : "Empire Crumbled";
        _score.text = $"${PlayerAssetManager.Instance.money:N0}K";
        
        _beachBoughtCount.text = PlayerAssetManager.Instance.beachPropertiesBought.ToString();
        _suburbBoughtCount.text = PlayerAssetManager.Instance.suburbPropertiesBought.ToString();
        _cityBoughtCount.text = PlayerAssetManager.Instance.cityPropertiesBought.ToString();
        
        _beachSoldCount.text = PlayerAssetManager.Instance.beachPropertiesSold.ToString();
        _suburbSoldCount.text = PlayerAssetManager.Instance.suburbPropertiesSold.ToString();
        _citySoldCount.text = PlayerAssetManager.Instance.cityPropertiesSold.ToString();
        
        _grossIncome.text = $"${PlayerAssetManager.Instance.totalProfit:N0}K";
        _totalSpend.text = $"${PlayerAssetManager.Instance.totalLosses:N0}K";
        _totalTaxed.text = $"${PlayerAssetManager.Instance.totalTaxPaid:N0}K";

        SetupGraph();
    }

    private void SetupGraph()
    {
        // Activate the GraphHandlerPrefab and create the graph
        if (_graphPrefab != null)
        {
            _graphPrefab.gameObject.SetActive(true);

            float moneyMin = float.MaxValue;
            float moneyMax = float.MinValue;

            for (int i = 0; i < PlayerAssetManager.moneyChanged.Count; i++)
            {
                _graphPrefab.CreatePoint(new Vector2(i, PlayerAssetManager.moneyChanged[i]));
                moneyMax = Mathf.Max(moneyMax, PlayerAssetManager.moneyChanged[i]);
                moneyMin = Mathf.Min(moneyMin, PlayerAssetManager.moneyChanged[i]);
            }

            _graphPrefab.SetCornerValues(new Vector2(-1f, moneyMin), new Vector2(PlayerAssetManager.moneyChanged.Count + 1, moneyMax));
            _graphPrefab.UpdateGraph();

            Debug.Log("GraphHandlerPrefab updated after delay.");
        }
        else
        {
            Debug.LogWarning("GraphHandlerPrefab is not assigned.");
        }
    }
}