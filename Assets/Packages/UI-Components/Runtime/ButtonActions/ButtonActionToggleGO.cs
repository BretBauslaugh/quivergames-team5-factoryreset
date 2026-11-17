using UnityEngine;

namespace UIComponents
{
    public class ButtonActionToggleGO : ButtonAction
    {
        [SerializeField] private GameObject[] _toDisable;
        [SerializeField] private GameObject[] _toEnable;

        protected override void DoAction()
        {
            foreach (GameObject go in _toDisable)
            {
                go.SetActive(false);
            }

            foreach (GameObject go in _toEnable)
            {
                go.SetActive(true);
            }
        }
    }
}