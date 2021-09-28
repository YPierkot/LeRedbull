using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(MultiShotsSO))]
public class MultiShotSOEditor : Editor {
    public override void OnInspectorGUI() {
        DrawBaseClas();
        
        GUILayout.Space(4);
        DrawMultishotClass();
    }

    /// <summary>
    /// Draw the base class
    /// </summary>
    private void DrawBaseClas() {
        GUI.skin.label.fontSize = 17;
        GUILayout.Label(serializedObject.FindProperty("weaponName").stringValue + " Class :");
        GUI.skin.label.fontSize = 12;
        
        using (new GUILayout.VerticalScope(EditorStyles.helpBox)) {
            GUI.skin.label.fontSize = 10;
            GUILayout.Label("Base Class Data:");
            GUI.skin.label.fontSize = 12;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("weaponName"));

            GUILayout.Space(4);
            using (new GUILayout.VerticalScope(EditorStyles.helpBox)) {
                GUI.skin.label.fontSize = 10;
                GUILayout.Label("Shot Data :");
                GUI.skin.label.fontSize = 12;
                
                EditorGUILayout.PropertyField(serializedObject.FindProperty("fireRate"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("damage"));
            }
            
            GUILayout.Space(4);
            using (new GUILayout.VerticalScope(EditorStyles.helpBox)) {
                GUI.skin.label.fontSize = 10;
                GUILayout.Label("Bullet Data :");
                GUI.skin.label.fontSize = 12;
                
                EditorGUILayout.PropertyField(serializedObject.FindProperty("bulletStartSpeed"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("bulletGam"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("bulletDeathTime"));
            }
            GUILayout.Space(2);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
    
    /// <summary>
    /// Draw the burst class variables
    /// </summary>
    private void DrawMultishotClass() {
        using (new GUILayout.VerticalScope(EditorStyles.helpBox)) {
            GUI.skin.label.fontSize = 10;
            GUILayout.Label("MultiShot Data :");
            GUI.skin.label.fontSize = 12;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("moveSpawnShot"));
            GUILayout.Space(2);
        }
        serializedObject.ApplyModifiedProperties();
    }
    
}
