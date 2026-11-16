using UnityEngine;

namespace UnityEditor.MaterialPropertyDrawers
{
    public class ShowIfBool : CustomMaterialPropertyDrawerBase
    {
        private string boolName;

        public ShowIfBool()
        {
        }

        public ShowIfBool(string boolName)
        {
            this.boolName = boolName;
        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
        {
            base.GetPropertyHeight(prop, label, editor);
            return 0;
        }

        public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
        {
            if (!isVisible) return;

            var properties = MaterialEditor.GetMaterialProperties(prop.targets);
            if (!GetBoolValue(boolName, properties)) return;
            else editor.DefaultShaderProperty(prop, label);
        }
    }
}