using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor.MaterialPropertyDrawers
{
    public class ShowIfLayerDrawer : CustomMaterialPropertyDrawerBase
    {
        public ShowIfLayerDrawer() { }

        public ShowIfLayerDrawer(float layerIndex)
        {
            base.layerIndex = (int)layerIndex;
        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
        {
            base.GetPropertyHeight(prop, label, editor);
            return 0;
        }

        public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
        {
            if (!isVisible) return;

            if (CheckKeywordArray(prop.targets)) editor.DefaultShaderProperty(prop, label);
        }
    }
}