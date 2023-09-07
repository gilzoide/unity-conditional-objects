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
        private GlobalObjectId _serializedObjectId;
        private Preset _preset;
        private UnityEditor.Editor _presetEditor;

        void OnEnable()
        {
            _gameObjectProperty = serializedObject.FindProperty($"{nameof(PropertyModifier.Target)}.{nameof(GameObjectOrComponent.GameObject)}");
            _componentIndexProperty = serializedObject.FindProperty($"{nameof(PropertyModifier.Target)}.{nameof(GameObjectOrComponent.ComponentIndex)}");
            _serializedObjectId = GlobalObjectId.GetGlobalObjectIdSlow(target);
            _preset = EmbeddedPresetHolder.Instance.GetOrCreatePreset(_serializedObjectId, GetTarget());
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
            if (newTarget != previousTarget)
            {
                Undo.RecordObject(EmbeddedPresetHolder.Instance, "");
                _preset = EmbeddedPresetHolder.Instance.GetOrCreatePreset(_serializedObjectId, newTarget);
            }

            if (_preset != null)
            {
                if (_presetEditor == null || _presetEditor.target != _preset)
                {
                    DestroyImmediate(_presetEditor);
                    _presetEditor = CreateEditor(_preset);
                }
                _presetEditor.OnInspectorGUI();
            }
        }

        private Object GetTarget()
        {
            return ((GameObject) _gameObjectProperty.objectReferenceValue).ComponentAtIndexOrSelf(_componentIndexProperty.intValue);
        }
    }
}
