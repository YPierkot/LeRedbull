using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShotGunSO))]
public class ShotgunSOEditor : Editor {
    public override void OnInspectorGUI() {
        DrawBaseClas();
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
                
                EditorGUILayout.PropertyField(serializedObject.FindProperty("bulletStartSize"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("bulletGam"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("bulletDeathTime"));
            }
            GUILayout.Space(2);
            
            serializedObject.ApplyModifiedProperties();
        }

        GUILayout.Space(4);
        
        using (new GUILayout.VerticalScope(EditorStyles.helpBox)) {
            GUI.skin.label.fontSize = 10;
            GUILayout.Label("Shotgun Class Data:");
            GUI.skin.label.fontSize = 12;
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("numberOfBulletToSpawn"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("burstAngle"));
            GUILayout.BeginHorizontal();
            GUILayout.Label("Min Start Speed");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("minStartBulletSpeed"), GUIContent.none);
            GUILayout.Label("Max Start Speed");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maxStartBulletSpeed"), GUIContent.none);
            GUILayout.EndHorizontal();
        }
    }
}
