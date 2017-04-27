using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GoRecycler.Editor
{
    internal static class GoRecyclerEditorHelper
    {

        #region Editor Integration 

        [MenuItem("GoRecycler/Create Utility")]
        private static void CreateManager()
        {
            var mm = Object.FindObjectOfType(typeof(GoRecyclerUtility));
            if (!mm)
            {
                GameObject gorecyclerManager = new GameObject("Go Recycler Manager", new System.Type[] { typeof(GoRecyclerUtility) });
                if (gorecyclerManager != null)
                {
                    Debug.Log("[GoRecycler] GoRecyclerUtility Created Correctly");
                    Selection.activeGameObject = gorecyclerManager;
                }
            }
            else
            {
                Debug.LogWarning("[GoRecycler] Can't create other: The scene already has an GoRecyclerUtlity");
            }
        }

        [MenuItem("GoRecycler/About")]
        private static void AboutGo()
        {
            EditorUtility.DisplayDialog("About GoRecycler", "GoRecycler: Object Pooling solution for unity. v1.7", "OK");
        }

        #endregion

        public static void BeginBox(string text , int fontsize = 12)
        {
            GUILayout.BeginVertical("", EditorStyles.helpBox);

            EditorGUILayout.LabelField(text, new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = fontsize
            });
            EditorGUILayout.Space();
        }

        public static void BeginBox()
        {
            GUILayout.BeginVertical("", EditorStyles.helpBox);
            GUIStyle style = new GUIStyle(EditorStyles.label);
            style.alignment = TextAnchor.MiddleCenter;
            style.fontStyle = FontStyle.Bold;
            style.fontSize = 12;
            EditorGUILayout.Space();
        }

        
        public static void Endbox()
        {
            EditorGUILayout.Space();
            GUILayout.EndVertical();
        }

        public static void DrawLine()
        {
            GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1.8f) });
        }
    }
}