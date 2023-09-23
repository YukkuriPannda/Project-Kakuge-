using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(ItemDataBase))]
public class ItemDataBaseEditor : Editor {
    bool isOpen;
    ItemCategory category;
    public override void OnInspectorGUI() {
        serializedObject.Update();
        ItemDataBase itemDataBase = target as ItemDataBase;

        SerializedProperty items = serializedObject.FindProperty("items");
        EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUI.indentLevel =1;
            for(int i = 0;i < items.arraySize;i ++){
                EditorGUILayout.PropertyField(items.GetArrayElementAtIndex(i));
            }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginHorizontal();
        {
            category = (ItemCategory)EditorGUILayout.EnumPopup(category);
            if(GUILayout.Button("Add",GUILayout.Width(64))){
                items.InsertArrayElementAtIndex(items.arraySize!=0?items.arraySize-1:0);
                items.GetArrayElementAtIndex(items.arraySize-1).FindPropertyRelative("id").intValue = items.arraySize-1;
                items.GetArrayElementAtIndex(items.arraySize-1).FindPropertyRelative("category").intValue = (int)category;
                items.GetArrayElementAtIndex(items.arraySize-1).FindPropertyRelative("uniqueParameters").ClearArray();
                switch(category){
                    case ItemCategory.Sword:
                        AddUniquePrams("Modelid",0,0);
                        AddUniquePrams("damage",0,100);
                        AddUniquePrams("Magic efficiency",0,100);
                        AddUniquePrams("exp",0,100);
                        items.GetArrayElementAtIndex(items.arraySize-1).FindPropertyRelative("spritePath").stringValue = "ItemIcons/Weapons";
                    break;
                    case ItemCategory.MagicBookFlame:
                        AddUniquePrams("MagicName",0,0);
                        AddUniquePrams("exp",0,100);
                        items.GetArrayElementAtIndex(items.arraySize-1).FindPropertyRelative("spritePath").stringValue = "ItemIcons/MagicBooks/FlameMagicBook";
                    break;
                    case ItemCategory.MagicBookAqua:
                        AddUniquePrams("MagicName",0,0);
                        AddUniquePrams("exp",0,100);
                        items.GetArrayElementAtIndex(items.arraySize-1).FindPropertyRelative("spritePath").stringValue = "ItemIcons/MagicBooks/AquaMagicBook";
                    break;
                    case ItemCategory.MagicBookElectro:
                        AddUniquePrams("MagicName",0,0);
                        AddUniquePrams("exp",0,100);
                        items.GetArrayElementAtIndex(items.arraySize-1).FindPropertyRelative("spritePath").stringValue = "ItemIcons/MagicBooks/ElectroMagicBook";
                    break;
                    case ItemCategory.MagicBookTerra:
                        AddUniquePrams("MagicName",0,0);
                        AddUniquePrams("exp",0,100);
                        items.GetArrayElementAtIndex(items.arraySize-1).FindPropertyRelative("spritePath").stringValue = "ItemIcons/MagicBooks/TerraMagicBook";
                    break;
                }
                EditorGUILayout.PropertyField(items);
            }
            GUILayout.Space(10);
            
            if(GUILayout.Button("Remove",GUILayout.Width(64))){
                items.DeleteArrayElementAtIndex(items.arraySize-1);
            }
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);
        SerializedProperty itemPrefabDatas = serializedObject.FindProperty("itemPrefabDatas");
        EditorGUILayout.BeginVertical(GUI.skin.box);{
            
            EditorGUI.indentLevel =1;
            for(int i = 0;i < itemPrefabDatas.arraySize;i ++){
                EditorGUILayout.PropertyField(itemPrefabDatas.GetArrayElementAtIndex(i));
            }
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                if(GUILayout.Button("Add",GUILayout.Width(64))){
                    Debug.Log(itemPrefabDatas.arraySize);
                    itemPrefabDatas.InsertArrayElementAtIndex(itemPrefabDatas.arraySize!=0?itemPrefabDatas.arraySize-1:0);
                    itemPrefabDatas.GetArrayElementAtIndex(itemPrefabDatas.arraySize-1).FindPropertyRelative("ID").intValue = itemPrefabDatas.arraySize-1;
                }
                GUILayout.Space(10);
                
                if(GUILayout.Button("Remove",GUILayout.Width(64))){
                    itemPrefabDatas.DeleteArrayElementAtIndex(itemPrefabDatas.arraySize-1);
                }
            }
            EditorGUILayout.EndHorizontal();
        }EditorGUILayout.EndVertical();
        
        GUILayout.Space(10);
        SerializedProperty specialMagicMotions = serializedObject.FindProperty("specialMagicMotions");
        EditorGUILayout.BeginVertical(GUI.skin.box);{
            
            EditorGUI.indentLevel =1;
            for(int i = 0;i < specialMagicMotions.arraySize;i ++){
                EditorGUILayout.PropertyField(specialMagicMotions.GetArrayElementAtIndex(i));
            }
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                if(GUILayout.Button("Add",GUILayout.Width(64))){
                    Debug.Log(specialMagicMotions.arraySize);
                    specialMagicMotions.InsertArrayElementAtIndex(specialMagicMotions.arraySize!=0?specialMagicMotions.arraySize-1:0);
                    specialMagicMotions.GetArrayElementAtIndex(specialMagicMotions.arraySize-1).FindPropertyRelative("magicKind").enumValueIndex = specialMagicMotions.arraySize-1;
                }
                GUILayout.Space(10);
                
                if(GUILayout.Button("Remove",GUILayout.Width(64))){
                    specialMagicMotions.DeleteArrayElementAtIndex(specialMagicMotions.arraySize-1);
                }
            }
            EditorGUILayout.EndHorizontal();
        }EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();

        void AddUniquePrams(string name,float value,float max){
            items.GetArrayElementAtIndex(items.arraySize-1).FindPropertyRelative("uniqueParameters").InsertArrayElementAtIndex(0);
            items.GetArrayElementAtIndex(items.arraySize-1).FindPropertyRelative("uniqueParameters").GetArrayElementAtIndex(0).FindPropertyRelative("name").stringValue = name;
            items.GetArrayElementAtIndex(items.arraySize-1).FindPropertyRelative("uniqueParameters").GetArrayElementAtIndex(0).FindPropertyRelative("value").floatValue = value;
            items.GetArrayElementAtIndex(items.arraySize-1).FindPropertyRelative("uniqueParameters").GetArrayElementAtIndex(0).FindPropertyRelative("maxValue").floatValue = max;
        }
    }
}