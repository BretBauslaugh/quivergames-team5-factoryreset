using UnityEngine;

namespace UnityEditor.MaterialPropertyDrawers
{
    public class ShowIfBoolKeyword : CustomMaterialPropertyDrawerBase
    {
        private string keywordName;

        public ShowIfBoolKeyword()
        {
        }

        public ShowIfBoolKeyword(string keywordName)
        {
            this.keywordName = keywordName;
        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
        {
            base.GetPropertyHeight(prop, label, editor);
            return 0;
        }

        public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
        {
            if (!isVisible) return;

            if(!CheckKeyword(prop.targets, keywordName)) return;
            else editor.DefaultShaderProperty(prop, label);
        }
    }
}