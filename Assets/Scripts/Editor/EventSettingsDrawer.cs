using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EventSettings))]
public class EventSettingsDrawer : PropertyDrawer
{
    private const float Spacing = 2f;
    
    private static readonly (EventProp flag, string label, string[] fields)[] Rows =
    {
        (EventProp.Delay,         "Delay (Min / Max Time)", new[] { "MinTime", "MaxTime" }),
        (EventProp.TimeLimit,     "Time",                   new[] { "Time" }),
        (EventProp.PressNeeded,   "Press Needed",           new[] { "PressNeeded" }),
        (EventProp.Reward,        "Reward",                 new[] { "Reward" }),
        (EventProp.FailLost,      "FailLost",               new[] { "FailLost" }),
        (EventProp.Chance,        "Chance",                 new[] { "Chance" }),
        (EventProp.DestroyOnFail, "Destroy On Fail",        new[] { "DestroyOnFail" }),        
        
    };
    
    private static bool IsManaged(string name)
    {
        if (name == "Enabled") return true;
        foreach (var row in Rows)
            foreach (var f in row.fields)
                if (f == name) return true;
        return false;
    }
    
    private static IEnumerable<SerializedProperty> UnlistedFields(SerializedProperty prop)
    {
        var it = prop.Copy();
        var end = prop.GetEndProperty();
        bool enter = true;
        while (it.NextVisible(enter) && !SerializedProperty.EqualContents(it, end))
        {
            enter = false;
            if (!IsManaged(it.name)) yield return it.Copy();
        }
    }

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        float line = EditorGUIUtility.singleLineHeight;
        float h = line;
        if (!prop.isExpanded) return h;

        int flags = prop.FindPropertyRelative("Enabled").intValue;
        foreach (var row in Rows)
        {
            h += line + Spacing;
            if ((flags & (int)row.flag) == 0) continue;
            foreach (var f in row.fields)
                h += EditorGUI.GetPropertyHeight(prop.FindPropertyRelative(f), true) + Spacing;
        }

        foreach (var other in UnlistedFields(prop))
            h += EditorGUI.GetPropertyHeight(other, true) + Spacing;

        return h + Spacing;
    }

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        EditorGUI.BeginProperty(pos, label, prop);

        float line = EditorGUIUtility.singleLineHeight;
        var head = new Rect(pos.x, pos.y, pos.width, line);
        prop.isExpanded = EditorGUI.Foldout(head, prop.isExpanded, label, true);

        if (prop.isExpanded)
        {
            EditorGUI.indentLevel++;
            var mask = prop.FindPropertyRelative("Enabled");
            int flags = mask.intValue;
            float y = head.yMax + Spacing;

            foreach (var row in Rows)
            {
                bool on = (flags & (int)row.flag) != 0;
                bool nowOn = EditorGUI.ToggleLeft(new Rect(pos.x, y, pos.width, line), row.label, on);
                if (nowOn != on) flags = nowOn ? flags | (int)row.flag : flags & ~(int)row.flag;
                y += line + Spacing;

                if (!nowOn) continue;
                foreach (var f in row.fields)
                {
                    var child = prop.FindPropertyRelative(f);
                    float h = EditorGUI.GetPropertyHeight(child, true);
                    EditorGUI.PropertyField(new Rect(pos.x, y, pos.width, h), child, true);
                    y += h + Spacing;
                }
            }

            // fields with no tick row yet: always visible
            foreach (var other in UnlistedFields(prop))
            {
                float h = EditorGUI.GetPropertyHeight(other, true);
                EditorGUI.PropertyField(new Rect(pos.x, y, pos.width, h), other, true);
                y += h + Spacing;
            }

            mask.intValue = flags;
            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }
}
