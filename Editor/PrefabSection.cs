using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace GameInspector
{
    // Prefab instance    -> [Apply Overrides] [Revert]
    // Plain scene object -> [Create Prefab] (saved to Assets/Prefabs)
    static class PrefabSection
    {
        // ponytail: fixed folder. Move to a setting if it must differ per user.
        const string ParentFolder = "Assets";
        const string FolderName = "Prefabs";

        public static void Draw(GameObject go)
        {
            if (EditorUtility.IsPersistent(go)) return; // prefab asset: edit via Prefab Mode

            var root = PrefabUtility.GetNearestPrefabInstanceRoot(go);
            if (root)
            {
                DrawInstance(root);
                return;
            }

            if (PrefabStageUtility.GetPrefabStage(go) != null) return; // contents of the prefab being edited
            if (!GUILayout.Button("Create Prefab")) return;
            Create(go);
            GUIUtility.ExitGUI();
        }

        static void DrawInstance(GameObject root)
        {
            var enabled = PrefabUtility.GetPrefabInstanceStatus(root) == PrefabInstanceStatus.Connected
                          && PrefabUtility.HasPrefabInstanceAnyOverrides(root, false);

            using (new EditorGUILayout.HorizontalScope())
            using (new EditorGUI.DisabledScope(!enabled))
            {
                if (GUILayout.Button("Apply Overrides"))
                {
                    PrefabUtility.ApplyPrefabInstance(root, InteractionMode.UserAction);
                    GUIUtility.ExitGUI();
                }

                if (GUILayout.Button("Revert"))
                {
                    PrefabUtility.RevertPrefabInstance(root, InteractionMode.UserAction);
                    GUIUtility.ExitGUI();
                }
            }
        }

        static void Create(GameObject go)
        {
            var folder = $"{ParentFolder}/{FolderName}";
            if (!AssetDatabase.IsValidFolder(folder)) AssetDatabase.CreateFolder(ParentFolder, FolderName);

            var file = string.Join("_", go.name.Split(Path.GetInvalidFileNameChars()));
            var path = AssetDatabase.GenerateUniqueAssetPath($"{folder}/{file}.prefab");
            PrefabUtility.SaveAsPrefabAssetAndConnect(go, path, InteractionMode.UserAction);
        }
    }
}
