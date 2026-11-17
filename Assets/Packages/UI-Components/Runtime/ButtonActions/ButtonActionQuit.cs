using UnityEngine;

namespace UIComponents
{
    public class ButtonActionQuit : ButtonAction
    {
        protected override void DoAction()
        {
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#endif
        }
    }
}