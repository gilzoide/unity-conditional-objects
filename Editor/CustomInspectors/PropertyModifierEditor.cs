using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

namespace Gilzoide.ConditionalObjects.Editor
{
    [CustomEditor(typeof(PropertyModifier))]
    public class PropertyModifierEditor : AImportTimeObjectModifierEditor
    {
        private SerializedProperty _gameObjectProperty;
        private SerializedProperty _componentIndexProperty;
        private SerializedProperty _presetProperty;
        private UnityEditor.Editor _presetEditor;

        void OnEnable()
        {
            _gameObjectProperty = serializedObject.FindProperty($"{nameof(PropertyModifier.Target)}.{nameof(GameObjectOrComponent.GameObject)}");
            _componentIndexProperty = serializedObject.FindProperty($"{nameof(PropertyModifier.Target)}.{nameof(GameObjectOrComponent.ComponentIndex)}");
            _presetProperty = serializedObject.FindProperty(nameof(PropertyModifier._preset));
        }

        void OnDisable()
        {
            _gameObjectProperty.Dispose();
            _componentIndexProperty.Dispose();
            DestroyImmediate(_presetEditor);
        }

        public override void OnInspectorGUI()
        {
            Object previousTarget = GetTarget();
            DrawDefaultInspector();
            Object newTarget = GetTarget();
            Preset preset = (Preset) _presetProperty.objectReferenceValue;
            if (newTarget != previousTarget || preset == null || !preset.CanBeAppliedTo(newTarget))
            {
                if (newTarget == null)
                {
                    preset = null;
                    _presetProperty.objectReferenceValue = null;
                }
                else
                {
                    preset = EmbeddedPresetHolder.Instance.CreatePreset(((Component) target).gameObject, newTarget);
                    _presetProperty.objectReferenceValue = preset;
                }
            }

            if (preset != null)
            {
                if (_presetEditor == null || _presetEditor.target != preset)
                {
                    DestroyImmediate(_presetEditor);
                    _presetEditor = CreateEditor(preset);
                }
                EditorGUI.BeginChangeCheck();
                _presetEditor.OnInspectorGUI();
                if (EditorGUI.EndChangeCheck())
                {
                    AssetDatabase.SaveAssetIfDirty(EmbeddedPresetHolder.Instance);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private Object GetTarget()
        {
            return ((GameObject) _gameObjectProperty.objectReferenceValue).ComponentAtIndexOrSelf(_componentIndexProperty.intValue);
        }
    }
}
