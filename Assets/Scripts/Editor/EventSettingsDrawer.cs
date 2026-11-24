using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EventSettings))]
public class EventSettingsDrawer : PropertyDrawer
{
    private const float Spacing = 2f;

    // flag, label shown to designers, field names it reveals
    private static readonly (EventProp flag, string label, string[] fields)[] Rows =
    {
        (EventProp.Delay,         "Delay (Min / Max Time)", new[] { "MinTime", "MaxTime" }),
        (EventProp.TimeLimit,     "Time",             new[] { "Time" }),
        (EventProp.PressNeeded,   "Multiple Press Needed",           new[] { "PressNeeded" }),
        (EventProp.Reward,        "Reward",                 new[] { "Reward" }),
        (EventProp.FailLost,        "FailLost",                 new[] { "FailLost" }),
        (EventProp.DestroyOnFail, "Destroy On Fail",        new[] { "DestroyOnFail" }),
    };

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

            mask.intValue = flags;
            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }
}
