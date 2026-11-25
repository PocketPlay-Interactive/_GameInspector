using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace GameInspector
{
    // Entry point. Draws every section under the Inspector header of a single selected GameObject.
    // New feature = new static class with Draw(GameObject) + one line in OnHeader.
    [InitializeOnLoad]
    static class HeaderToolbar
    {
        const string HiddenKey = "GameInspector.Hidden";
        const float LabelIndent = 15f; // aligns the label with the "Prefab" text; tweak if off by a few px

        static HeaderToolbar() => Editor.finishedDefaultHeaderGUI += OnHeader;

        static void OnHeader(Editor editor)
        {
            if (editor.targets.Length != 1 || editor.target is not GameObject go) return;
            if (PrefabUtility.IsPartOfImmutablePrefab(go)) return; // model assets are read-only

            // Small arrow; click arrow or label to show/hide. Hidden by default.
            bool shown;
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(LabelIndent);
                shown = EditorGUILayout.Foldout(!EditorPrefs.GetBool(HiddenKey, true), "GameInspector", true);
            }

            EditorPrefs.SetBool(HiddenKey, !shown);
            if (!shown) return;

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(LabelIndent);
                using (new EditorGUILayout.VerticalScope())
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button("Expand All", EditorStyles.miniButtonLeft)) SetExpanded(go, true);
                        if (GUILayout.Button("Collapse All", EditorStyles.miniButtonRight)) SetExpanded(go, false);
                    }

                    PrefabSection.Draw(go);
                    ComponentSection.Draw(go);
                }
            }
        }

        // Fold/unfold every component block in the Inspector.
        static void SetExpanded(GameObject go, bool expanded)
        {
            foreach (var c in go.GetComponents<Component>())
            {
                if (c) InternalEditorUtility.SetIsInspectorExpanded(c, expanded);
            }

            ActiveEditorTracker.sharedTracker.ForceRebuild();
            GUIUtility.ExitGUI();
        }
    }
}
