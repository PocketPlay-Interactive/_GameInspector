using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace GameInspector
{
    // Always-visible copy/paste buttons, no cog menu needed.
    // ponytail: rows sit under the GameObject header, not inside each component header (that needs
    // InspectorWindow reflection). Upgrade only if that placement matters.
    static class ComponentSection
    {
        const string FoldKey = "GameInspector.ComponentSection.Open";

        public static void Draw(GameObject go)
        {
            var open = EditorGUILayout.Foldout(SessionState.GetBool(FoldKey, true), "Component Clipboard", true);
            SessionState.SetBool(FoldKey, open);
            if (!open) return;

            if (GUILayout.Button("Paste Component As New"))
            {
                Report(ComponentUtility.PasteComponentAsNew(go));
                GUIUtility.ExitGUI(); // component list changed
            }

            foreach (var c in go.GetComponents<Component>())
            {
                if (c) DrawRow(c); // null = missing script
            }
        }

        static void DrawRow(Component c)
        {
            var oldIconSize = EditorGUIUtility.GetIconSize();
            EditorGUIUtility.SetIconSize(new Vector2(16, 16));

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label(new GUIContent(c.GetType().Name, EditorGUIUtility.ObjectContent(c, c.GetType()).image), GUILayout.Height(18));

                if (GUILayout.Button("Copy", EditorStyles.miniButtonLeft, GUILayout.ExpandWidth(false)))
                    ComponentUtility.CopyComponent(c);

                if (GUILayout.Button("Paste Values", EditorStyles.miniButtonMid, GUILayout.ExpandWidth(false)))
                {
                    Undo.RecordObject(c, "Paste Component Values");
                    var pasted = ComponentUtility.PasteComponentValues(c);
                    if (pasted) PrefabUtility.RecordPrefabInstancePropertyModifications(c);
                    Report(pasted);
                }

                using (new EditorGUI.DisabledScope(c is Transform)) // Transform cannot be removed
                {
                    if (GUILayout.Button("Remove", EditorStyles.miniButtonRight, GUILayout.ExpandWidth(false)))
                    {
                        Undo.DestroyObjectImmediate(c); // undoable; Unity logs an error if another component requires it
                        GUIUtility.ExitGUI(); // component list changed
                    }
                }
            }

            EditorGUIUtility.SetIconSize(oldIconSize);
        }

        static void Report(bool ok)
        {
            if (!ok) Debug.LogWarning("[GameInspector] Clipboard has no compatible component. Copy one first.");
        }
    }
}
