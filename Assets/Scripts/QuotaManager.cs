using PrimeTween;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuotaManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private Resource _resource;
    [SerializeField] private ResourceType _canister;
    [SerializeField] private ResourceType _coin;
    [Header("Labels")]
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _quotaText;
    [SerializeField] private TMP_Text _nextRoundText;
    [Header("Label Animations")]
    [SerializeField] private ShakeSettings _timerAnimation;
    [SerializeField] private ShakeSettings _quotaAnimation;
    [Header("Panels")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _victoryPanel;
    [SerializeField] private GameObject _nextRoundPanel;
    [Header("Values")]
    [SerializeField] private float _roundTime = 120f;
    [SerializeField] private int _startQuota = 1000;
    [SerializeField, Range(0f, 1f)] private float _baseGrowth = 0.10f;
    [SerializeField] private float _overshootWeight = 0.5f;
    [SerializeField] private float _maxGrowth = 1.0f;
    [SerializeField] private float _intermissionTime = 3f;
    [SerializeField] private int _coinBase = 10;
    [SerializeField] private int _coinPerRound = 5;
    [SerializeField] private float _coinOvershootWeight = 1f;
    [SerializeField] private float _maxCoinMultiplier = 3f;
    [SerializeField] private float _timePerRound = 15f;
    [SerializeField] private float _maxRoundTime = 300f;

    private float _timeLeft;
    private float _currentRoundTime;
    private float _timeChanged;
    private int _quota;
    private int _round = 1;
    private bool _running;

    void Start()
    {
        Time.timeScale = 1f;
        _quota = _startQuota;
        _currentRoundTime = _roundTime;
        _timeLeft = _currentRoundTime;
        _running = true;
        if (_gameOverPanel) _gameOverPanel.SetActive(false);
        if (_victoryPanel) _victoryPanel.SetActive(false);
        if (_nextRoundPanel) _nextRoundPanel.SetActive(false);
        RefreshUI();
    }

    void Update()
    {
        // time
        if (!_running) return;
        //_timeLeft = _timeChanged;
        _timeLeft -= Time.deltaTime;
        //Tween.PunchScale(_timerText.transform, _timerAnimation);
        if (_timeLeft <= 0f) 
        { 
            Resolve();
            return;
        }
        //

        // canisters quota
        if (_resource.Get(_canister) >= _quota)
        {
            Resolve();
            return;
        }
        //
        RefreshUI();
    }

    private void Resolve()
    {
        int total = _resource.Get(_canister);
        if(total >= _quota)
        {
            float overshoot = (total - _quota) / (float)_quota;
            float growth = Mathf.Min(_baseGrowth + _overshootWeight * overshoot, _maxGrowth);
            float coinMult = Mathf.Min(1f + overshoot * _coinOvershootWeight, _maxCoinMultiplier);
            int reward = Mathf.RoundToInt((_coinBase + _coinPerRound * (_round - 1)) * coinMult);
            _resource.Add(_coin, reward);
            _resource.Add(_canister, -total);
            _quota = Mathf.RoundToInt(total * (1f + growth));
            _round++;
            _currentRoundTime = Mathf.Min(_currentRoundTime + _timePerRound, _maxRoundTime);
            StartCoroutine(Intermission(reward));
        }
        else
        {
            _running = false;
            _timeLeft = 0f;
            Time.timeScale = 0f;
            if (_gameOverPanel) _gameOverPanel.SetActive(true);
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        _resource.ResetToStart();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void RefreshUI()
    {
        if(_timerText)
        {
            int t = Mathf.CeilToInt(Mathf.Max(_timeLeft, 0f));
            _timerText.text = $"<b>{t / 60}:{t % 60:00}</b>";
        }
        if (_quotaText)
        {
            _quotaText.text = $"<b>Round #{_round}: </b>{_resource.Get(_canister)} / {_quota}";
        }
    }

    private IEnumerator Intermission(int reward)
    {
        _running = false;
        Time.timeScale = 0f;
        if (_nextRoundPanel) _nextRoundPanel.SetActive(true);
        if (_nextRoundText) _nextRoundText.text = $"<size=90>Quota Met!</size>\n+{reward} Coins\nNext quota: {_quota}";

        yield return new WaitForSecondsRealtime(_intermissionTime);

        if (_nextRoundPanel) _nextRoundPanel.SetActive(false);
        Time.timeScale = 1f;
        _timeLeft = _currentRoundTime;
        _running = true;
    }
}