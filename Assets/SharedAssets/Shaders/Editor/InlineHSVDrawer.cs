using System;
using UnityEditor;
using UnityEditor.MaterialPropertyDrawers;
using UnityEngine;
using UnityEngine.Rendering;

public class InlineHSVDrawer : CustomMaterialPropertyDrawerBase
{
    string hsvPropertyName;
    public InlineHSVDrawer() { }

    public InlineHSVDrawer(string extraProperty1)
    {
        this.hsvPropertyName = extraProperty1;
    }

    public InlineHSVDrawer(string extraProperty1, float layer)
    {
        this.hsvPropertyName = extraProperty1;
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
        if (prop.propertyType != ShaderPropertyType.Texture)
        {
            editor.DefaultShaderProperty(prop, label);
            return;
        }

        if(layerIndex >= 0 && !CheckKeywordArray(prop.targets)) return;

        var properties = MaterialEditor.GetMaterialProperties(prop.targets);
        MaterialProperty hsv = FindProperty(hsvPropertyName, properties);

        using (new EditorGUI.DisabledScope(!isEnabled || (prop.propertyFlags & ShaderPropertyFlags.PerRendererData) != 0))
        {
            EditorGUI.BeginChangeCheck();
            float labelWidth = EditorGUIUtility.labelWidth;
            editor.TexturePropertyMiniThumbnail(position, prop, label, null);
            Vector3 hsvValues = hsv.vectorValue;
            var fieldRect = position;
            fieldRect.width -= labelWidth;
            fieldRect.x += labelWidth;
            float[] values = new[] { hsvValues.x, hsvValues.y, hsvValues.z };
            EditorGUI.MultiFloatField(fieldRect, new GUIContent[] {new GUIContent("Hue"), new GUIContent("Sat"), new GUIContent("Val")}, values);
            if (EditorGUI.EndChangeCheck())
            {
                hsv.vectorValue = new Vector4(values[0], values[1], values[2], 0f);
            }
        }
            
    }
}