using System;
using System.Linq;
using Characters;
using Characters.Enums;
using UnityEditor;
using UnityEngine;

namespace Network.Editor
{
    [CustomEditor(typeof(PlayerPropertiesProgressionModelSO))]
    public class PlayerPropertiesProgressionModelEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var models = serializedObject.FindProperty("models");

            if (GUILayout.Button("Normalize chances"))
            {
                float chancesSum = ((PlayerPropertiesProgressionModelSO)target).Models.Sum(i => i.Chance);
                
                if (chancesSum == 0f)
                {
                    for (int i = 0; i < models.arraySize; i++)
                    {
                        var chance = models.GetArrayElementAtIndex(i).FindPropertyRelative("chance");
                        chance.floatValue = PlayerPropertiesProgressionModelSO.MaxChance / models.arraySize;
                    }
                }
                else
                {
                    for (int i = 0; i < models.arraySize; i++)
                    {
                        var chance = models.GetArrayElementAtIndex(i).FindPropertyRelative("chance");
                        chance.floatValue /= chancesSum;
                        chance.floatValue *= PlayerPropertiesProgressionModelSO.MaxChance;
                    }
                }
            }
            
            if (GUILayout.Button("Reset models list"))
            {
                models.ClearArray();
                var values = Enum.GetValues(typeof(ECharacterStat));
                models.arraySize = values.Length;
                foreach (int i in values)
                {
                    var a = models.GetArrayElementAtIndex(i);
                    a.FindPropertyRelative("parameterName").enumValueIndex = i;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}