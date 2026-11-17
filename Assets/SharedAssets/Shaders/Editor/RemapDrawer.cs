using System;
using UnityEditor.MaterialPropertyDrawers;
using UnityEngine;

namespace UnityEditor.ShaderGraph.Samples
{
    public class RemapDrawer : CustomMaterialPropertyDrawerBase
    {
        private Vector2 range = Vector2.up;
        private bool drawFields;

        public RemapDrawer()
        {
            range.x = 0;
            range.y = 1;
            drawFields = false;
        }

        public RemapDrawer(float minLimit, float maxLimit)
        {
            range.x = minLimit;
            range.y = maxLimit;
            drawFields = false;
        }

        public RemapDrawer(float minLimit, float maxLimit, string drawFields)
        {
            range.x = minLimit;
            range.y = maxLimit;
            bool.TryParse(drawFields, out this.drawFields);
        }

        public RemapDrawer(float minLimit, float maxLimit, float limitSlide, string drawFields)
        {
            range.x = minLimit - limitSlide;
            range.y = maxLimit - limitSlide;
            bool.TryParse(drawFields, out this.drawFields);
        }

        public RemapDrawer(float minLimit, float maxLimit, float limitSlide, string drawFields, float layer)
        {
            range.x = minLimit - limitSlide;
            range.y = maxLimit - limitSlide;
            bool.TryParse(drawFields, out this.drawFields);
            layerIndex = (int)layer;
        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
        {
            base.GetPropertyHeight(prop, label, editor);
            if (layerIndex >= 0 && !CheckKeywordArray(prop.targets)) return 0;
            return 20;
        }

        public override void OnGUI(Rect position, MaterialProperty prop, String label, MaterialEditor editor)
        {
            if (!isVisible) return;

            if (layerIndex >= 0 && !CheckKeywordArray(prop.targets)) return;

            editor.BeginAnimatedCheck(position, prop);
            using (new EditorGUI.DisabledScope((prop.propertyFlags & UnityEngine.Rendering.ShaderPropertyFlags.PerRendererData) != 0))
            {
                MaterialEditor.BeginProperty(position, prop);
                float labelWidth = EditorGUIUtility.labelWidth;

                var labelRect = position;
                var sliderRect = position;
                var minRect = position;
                var maxRect = position;

                float fieldWidth = drawFields ? 30f : 0f;
                float frontPadding = drawFields ? 8f : 2f;
                float backPadding = drawFields ? 14f : 4f;
                sliderRect.x += labelWidth + frontPadding + fieldWidth;
                sliderRect.width -= labelWidth + backPadding + 2f* fieldWidth;
                minRect.x += labelWidth + 2f;
                minRect.width = fieldWidth;
                maxRect.x += labelWidth + fieldWidth + sliderRect.width + 14f;
                maxRect.width = fieldWidth;

                Vector2 v = prop.vectorValue;
                float min = v.x, max = v.y;

                EditorGUIUtility.labelWidth = 0f;
                EditorGUI.LabelField(labelRect, label);

                EditorGUI.showMixedValue = prop.hasMixedValue;

                EditorGUI.BeginChangeCheck();

                if (!prop.hasMixedValue)
                {
                    range.x = EditorGUI.FloatField(minRect, range.x);
                    range.y = EditorGUI.FloatField(maxRect, range.y);
                    EditorGUI.MinMaxSlider(sliderRect, ref min, ref max, range.x, range.y);
                }

                if (EditorGUI.EndChangeCheck())
                {
                    prop.vectorValue = new Vector2(min, max);
                }

                EditorGUI.showMixedValue = false;

                EditorGUIUtility.labelWidth = labelWidth;
                MaterialEditor.EndProperty();
            }
            editor.EndAnimatedCheck();
        }
    }
}
