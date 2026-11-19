using System.Collections;
using UnityEngine;
using TMPro;

public class ErrorText : MonoBehaviour
{
    public static ErrorText Instance { get; private set; }

    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _showTime = 1f;

    private Coroutine _hide;

    private void Awake()
    {
        Instance = this;
        _text.gameObject.SetActive(false);
    }
    void Start()
    {
        
    }    
    void Update()
    {
        
    }

    public static void Show(string mesg)
    {
        if (Instance != null) Instance.SetupText(mesg);
        else Debug.LogWarning(mesg);
    }

    public void SetupText(string mesg)
    {
        _text.text = mesg;
        _text.gameObject.SetActive(true);

        if(_hide != null) StopCoroutine(_hide);
        _hide = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSecondsRealtime(_showTime);
        _text.gameObject.SetActive(false);
    }
}
