using System;
using UnityEditor;
using UnityEditor.MaterialPropertyDrawers;
using UnityEngine;
using UnityEngine.Rendering;

public class MiniThumbnailDrawer : CustomMaterialPropertyDrawerBase
{
    public MiniThumbnailDrawer() { }

    public MiniThumbnailDrawer(float layer)
    {
        layerIndex = (int)layer;
    }

    public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
    {
        base.GetPropertyHeight(prop, label, editor);
        if (!isVisible) return 0;
        if (layerIndex >= 0 && !CheckKeywordArray(prop.targets)) return 0;
        return 20;
    }

    public override void OnGUI(Rect position, MaterialProperty prop, String label, MaterialEditor editor)
    {
        if(!isVisible) return;

        if (prop.propertyType != ShaderPropertyType.Texture)
        {
            editor.DefaultShaderProperty(prop, label);
            return;
        }

        if (layerIndex >= 0 && !CheckKeywordArray(prop.targets)) return;

        editor.TexturePropertyMiniThumbnail(position, prop, label, null);
    }
}