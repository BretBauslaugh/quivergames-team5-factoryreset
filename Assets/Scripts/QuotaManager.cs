using PrimeTween;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class QuotaManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private Resource _resource;
    [SerializeField] private ResourceType _canister;
    [SerializeField] private ResourceType _coin;
    [SerializeField] private Animator _scrappyAnimator;
    [SerializeField] private Shop _shop;
    [Header("Labels")]
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _quotaText;
    [SerializeField] private TMP_Text _roundText;
    [SerializeField] private TMP_Text _canisterText;
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
    [Header("FMOD")]
    [SerializeField] private EventReference canisterCollectSfx;
    [SerializeField] private EventReference quotaMetSfx;

    private float _timeLeft;
    private float _currentRoundTime;
    private float _timeChanged;
    private int _quota;
    private int _round = 1;
    private bool _running;
    private string _previousCanister;
    private string _extra;

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
        _previousCanister = $"{_resource.Get(_canister)}";
        RefreshUI();
        _scrappyAnimator.SetBool("Happy", false);
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
        // 
        /* Only have this if the round should end if Quotas are reached. 
        if (_resource.Get(_canister) >= _quota)
        {
            Resolve();
            return;
        }*/
        RefreshUI();
    }

    private void Resolve()
    {
        int total = _resource.Get(_canister);
        if(total >= _quota)
        {
            float overshoot = (total - _quota) / (float)_quota;
            float growth = 1.2f;
            float coinMult = Mathf.Min(1f + overshoot * _coinOvershootWeight, _maxCoinMultiplier);
            int reward = Mathf.RoundToInt((_coinBase + _coinPerRound * (_round - 1)) * coinMult);
            _resource.Add(_coin, reward);
            _resource.Add(_canister, -total);
            _quota = Mathf.RoundToInt(_quota * (1f + growth));
            _round++;
            CheckRoundPoolChanges();
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

    private void CheckRoundPoolChanges()
    {
        switch (_round)
        {
            case 3:
                _shop.currentPool = 2;
                _shop.UpdateShopPool();
                _extra = "\nMore items unlocked!";
                break;
            case 6:
                _shop.currentPool = 3;
                _shop.UpdateShopPool();
                _extra = "\nMore items unlocked!";
                break;
            default:
                _extra = "";
                break;
        }
            
        if (_round == 3) _shop.currentPool = 2;
        if (_round == 6) _shop.currentPool = 3;
        
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
        if (_roundText) _roundText.text = $"<b>Round #{_round}:</b>";
        if (_canisterText)
        {
            _canisterText.text = $"{_resource.Get(_canister)}";
            if (_previousCanister != _canisterText.text)
            {
                Tween.PunchScale(_canisterText.transform, _quotaAnimation);
                Playsfx(canisterCollectSfx);
            }
            _previousCanister = _canisterText.text;
        }
        if (_quotaText) _quotaText.text = $"/ {_quota}";
    }

    private IEnumerator Intermission(int reward)
    {
        _running = false;
        Time.timeScale = 0f;
        if (_nextRoundPanel) _nextRoundPanel.SetActive(true);
        if (_nextRoundText)
        {
            _scrappyAnimator.SetBool("Happy", true);
            _nextRoundText.text = $"<size=90>Quota Met!</size>\n+{reward} Coins\nNext quota: {_quota}{_extra}";
            Playsfx(quotaMetSfx);
        }

        yield return new WaitForSecondsRealtime(_intermissionTime);

        if (_nextRoundPanel) _nextRoundPanel.SetActive(false);
        Time.timeScale = 1f;
        _scrappyAnimator.SetBool("Happy", false);
        _timeLeft = _currentRoundTime;
        _running = true;
    }

    public void Playsfx(EventReference sfx)
    {
        if (sfx.IsNull)
        {
            Debug.LogError("ERROR: [FMOD] Event not found.");
            return;
        }
        EventInstance instance = RuntimeManager.CreateInstance(sfx);
        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position)); //added to prevent warnings
        RuntimeManager.PlayOneShot(sfx, transform.position);
    }

    public void AddCoins(int i){
        _resource.Add(_coin, i);
    }
}