using System;
using UnityEditor.MaterialPropertyDrawers;
using UnityEngine;

namespace UnityEditor.ShaderGraph.Samples
{
    public class HRDecorator : CustomMaterialPropertyDrawerBase
    {
        private bool hasHR;
        private string keywordName;
        private bool hasKeyword;
        private string header;
        private bool hasHeader;
        private float topPadding;

        public HRDecorator()
        {
            hasHR = true;
        }

        public HRDecorator(string hasHR, string header)
        {
            bool.TryParse(hasHR, out this.hasHR);
            this.header = header;
            this.hasHeader = !string.IsNullOrEmpty(header);
        }

        public HRDecorator(string hasHR, string header, string keyword)
        {
            bool.TryParse(hasHR, out this.hasHR);
            this.header = header;
            this.keywordName = keyword;
            this.hasHeader = !string.IsNullOrEmpty(header);
            this.hasKeyword = !string.IsNullOrEmpty(keyword);
        }

        public HRDecorator(string hasHR, string header, string keyword, float topPadding)
        {
            bool.TryParse(hasHR, out this.hasHR);
            this.header = header;
            this.keywordName = keyword;
            this.hasHeader = !string.IsNullOrEmpty(header);
            this.hasKeyword = !string.IsNullOrEmpty(keyword);
            this.topPadding = topPadding;
        }

        public HRDecorator(string hasHR, string header, float layerIndex)
        {
            bool.TryParse(hasHR, out this.hasHR);
            this.header = header;
            this.hasHeader = !string.IsNullOrEmpty(header);
            this.hasKeyword = true;
            this.layerIndex = (int)layerIndex;
        }

        public HRDecorator(string hasHR, string header, float layerIndex, float topPadding)
        {
            bool.TryParse(hasHR, out this.hasHR);
            this.header = header;
            this.hasHeader = !string.IsNullOrEmpty(header);
            this.hasKeyword = true;
            this.layerIndex = (int)layerIndex;
            this.topPadding = topPadding;
        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
        {
            base.GetPropertyHeight(prop, label, editor);
            if (hasKeyword && (!CheckKeyword(prop.targets, keywordName) && !CheckKeywordArray(prop.targets))) return 0;
            if (hasHeader) return 28 + topPadding;
            return 8 + topPadding;
        }

        public override void OnGUI(Rect position, MaterialProperty prop, String label, MaterialEditor editor)
        {
            if (!isVisible) return;

            if (hasKeyword && (!CheckKeyword(prop.targets, keywordName) && !CheckKeywordArray(prop.targets))) return;

            if(hasHR)
            {
                var hrRect = position;
                hrRect.y += 3;
                if (hasHeader) hrRect.y += 20;
                hrRect.y += topPadding;
                hrRect.height = 2f;
                EditorGUI.DrawRect(hrRect, Color.gray3);
            }

            if(hasHeader)
            {
                var headerRect = position;
                headerRect.height = 20f;
                headerRect.y += topPadding;
                GUI.Label(headerRect, header, new GUIStyle(EditorStyles.whiteLargeLabel));
            }
        }
    }
}
