using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuotaManager : MonoBehaviour
{
    [SerializeField] private Resource _resource;
    [SerializeField] private ResourceType _canister;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _quotaText;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private float _roundTime = 120f;
    [SerializeField] private int _startQuota = 1000;
    [SerializeField, Range(0f, 1f)] private float _baseGrowth = 0.10f;
    [SerializeField] private float _overshootWeight = 0.5f;
    [SerializeField] private float _maxGrowth = 1.0f;
    private float _timeLeft;
    private int _quota;
    private int _round = 1;
    private bool _running;

    void Start()
    {
        Time.timeScale = 1f;
        _quota = _startQuota;
        _timeLeft = _roundTime;
        _running = true;
        if (_gameOverPanel) _gameOverPanel.SetActive(false);
        RefreshUI();
    }

    void Update()
    {
        if (!_running) return;
        _timeLeft -= Time.deltaTime;
        if (_timeLeft <= 0f) Resolve();
        RefreshUI();
    }

    private void Resolve()
    {
        int total = _resource.Get(_canister);
        if(total >= _quota)
        {
            float overshoot = (total - _quota) / (float)_quota;
            float growth = Mathf.Min(_baseGrowth + _overshootWeight * overshoot, _maxGrowth);
            _quota = Mathf.RoundToInt(total * (1f + growth));
            _round++;
            _timeLeft = _roundTime;
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
            _timerText.text = $"Time Remaining: {t / 60}:{t % 60:00}";
        }
        if (_quotaText)
        {
            _quotaText.text = $"Quota #{_round}: {_resource.Get(_canister)} / {_quota}";
        }
    }
}