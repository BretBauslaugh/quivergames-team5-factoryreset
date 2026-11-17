using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor.MaterialPropertyDrawers
{
    public class NormalMapDrawer : CustomMaterialPropertyDrawerBase
    {
        string strengthProperty;

        public NormalMapDrawer() { }

        public NormalMapDrawer(string strengthProperty)
        {
            this.strengthProperty = strengthProperty;
        }

        public NormalMapDrawer(string strengthProperty, float layer)
        {
            this.strengthProperty = strengthProperty;
            layerIndex = (int)layer;
        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
        {
            base.GetPropertyHeight(prop, label, editor);
            if (layerIndex >= 0 && !CheckKeywordArray(prop.targets)) return 0;
            return 20;
        }

        public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
        {
            if (!isVisible) return;
            if (prop.propertyType != ShaderPropertyType.Texture)
            {
                editor.DefaultShaderProperty(prop, label);
                return;
            }

            if (layerIndex >= 0 && !CheckKeywordArray(prop.targets)) return;

            var properties = MaterialEditor.GetMaterialProperties(prop.targets);
            MaterialProperty strengthProp = FindProperty(strengthProperty, properties);

            bool showStrengthProperty = strengthProp != null;

            using (new EditorGUI.DisabledScope(!isEnabled || (prop.propertyFlags & ShaderPropertyFlags.PerRendererData) != 0))
            {
                EditorGUI.BeginChangeCheck();
                float labelWidth = EditorGUIUtility.labelWidth;
                editor.TexturePropertyMiniThumbnail(position, prop, label, null);
                var labelRect = position;
                labelRect.x += labelWidth;
                labelRect.width = 60f;
                EditorGUI.LabelField(labelRect, "Strength");
                var sliderRect = position;
                sliderRect.x += labelWidth + labelRect.width;
                sliderRect.width -= labelWidth + labelRect.width;
                float strength = strengthProp.floatValue;
                strength = EditorGUI.Slider(sliderRect, strength, 0f, 4f);
                if(EditorGUI.EndChangeCheck())
                {
                    strengthProp.floatValue = strength;
                }
            }
        }
    }
}