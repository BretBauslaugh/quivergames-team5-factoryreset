using UnityEngine;
using UnityEngine.SceneManagement;

namespace UIComponents
{
    public class ButtonActionLoadScene : ButtonAction
    {
        [SerializeField] private string _sceneName = "Some Scene";
        [SerializeField] private float _timeScale = 1f;

        protected override void DoAction()
        {
            SceneManager.LoadScene(_sceneName);
            Time.timeScale = _timeScale;
        }
    }
}