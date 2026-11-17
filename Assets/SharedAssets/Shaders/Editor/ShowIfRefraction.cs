using UnityEngine;

namespace UnityEditor.MaterialPropertyDrawers
{
    public class ShowIfRefraction : CustomMaterialPropertyDrawerBase
    {
        private const string _planar = "_REFRACTION_PLANE";
        private const string _sphere = "_REFRACTION_SPHERE";
        private const string _thin = "_REFRACTION_THIN";
        private bool _planarAndSphere;

        public ShowIfRefraction()
        {
        }

        public ShowIfRefraction(string planarAndSphere)
        {
            _planarAndSphere = bool.Parse(planarAndSphere);
        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
        {
            base.GetPropertyHeight(prop, label, editor);
            return 0;
        }

        public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
        {
            if (!isVisible) return;

            if (!CheckKeyword(prop.targets, _thin) &&
                !CheckKeyword(prop.targets, _planar) &&
                !CheckKeyword(prop.targets, _sphere)) return;

            if(CheckKeyword(prop.targets, _thin) && _planarAndSphere) return;

            editor.DefaultShaderProperty(prop, label);
        }
    }
}