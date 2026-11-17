using UnityEditor.MaterialPropertyDrawers;
using UnityEngine;

namespace UnityEditor.MaterialPropertyDrawers
{
    public class CustomEnumDrawer : CustomMaterialPropertyDrawerBase
    {
        private string[] options = new string[] { "Vertex Painting", "Top-Down Projection", "LEGC Mask" };

        public CustomEnumDrawer()
        {
        }

        public CustomEnumDrawer(float layerIndex)
        {
            this.layerIndex = (int)layerIndex;
        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
        {
            base.GetPropertyHeight(prop, label, editor);
            return 0;
        }

        public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
        {
            if (!isVisible) return;

            if (CheckKeywordArray(prop.targets))
            {
                EditorGUI.BeginChangeCheck();

                int selected = (int)prop.floatValue;
                selected = EditorGUILayout.Popup(label, selected, options);

                if(EditorGUI.EndChangeCheck())
                {
                    prop.floatValue = selected;
                }
            }
        }
    }
}