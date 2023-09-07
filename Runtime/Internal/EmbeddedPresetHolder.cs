#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gilzoide.ConditionalObjects
{
    public class EmbeddedPresetHolder : ScriptableObject
    {
        public static readonly string InstanceAssetPath = $"Assets/ConditionalObjects/Editor/{nameof(EmbeddedPresetHolder)}.asset";

        public static EmbeddedPresetHolder Instance => _instance ? _instance : (_instance = GetOrCreateInstance());
        private static EmbeddedPresetHolder _instance;

        static EmbeddedPresetHolder GetOrCreateInstance()
        {
            string assetFolder = Path.GetDirectoryName(InstanceAssetPath);
            if (!Directory.Exists(assetFolder))
            {
                Directory.CreateDirectory(assetFolder);
            }

            EmbeddedPresetHolder instance = AssetDatabase.LoadAssetAtPath<EmbeddedPresetHolder>(InstanceAssetPath);
            if (!instance)
            {
                instance = CreateInstance<EmbeddedPresetHolder>();
                AssetDatabase.CreateAsset(instance, InstanceAssetPath);
            }
            return instance;
        }

        [Serializable]
        public class EmbeddedPreset
        {
            public SerializableGlobalObjectId ObjectId;
            public Preset Preset;
        }

        [SerializeField] private List<EmbeddedPreset> _embeddedPresets = new List<EmbeddedPreset>();

        public Preset GetPreset(Object owner, Object target)
        {
            GlobalObjectId objectId = GlobalObjectId.GetGlobalObjectIdSlow(owner);
            if (objectId.assetGUID.Empty())
            {
                return null;
            }

            if (_embeddedPresets.Find(p => objectId.Equals(p.ObjectId)) is EmbeddedPreset embeddedPreset
                && embeddedPreset.Preset != null
                && embeddedPreset.Preset.CanBeAppliedTo(target))
            {
                return embeddedPreset.Preset;
            }
            else
            {
                return null;
            }
        }

        public Preset GetOrCreatePreset(GlobalObjectId objectId, Object target)
        {
            if (objectId.assetGUID.Empty())
            {
                return null;
            }

            if (_embeddedPresets.Find(p => objectId.Equals(p.ObjectId)) is EmbeddedPreset embeddedPreset)
            {
                if (target == null)
                {
                    DestroyImmediate(embeddedPreset.Preset, true);
                    _embeddedPresets.RemoveAll(p => objectId.Equals(p.ObjectId));
                    return null;
                }
                else if (!embeddedPreset.Preset.CanBeAppliedTo(target))
                {
                    DestroyImmediate(embeddedPreset.Preset, true);
                }
            }
            else if (target == null)
            {
                return null;
            }
            else
            {
                embeddedPreset = new EmbeddedPreset
                {
                    ObjectId = objectId,
                };
                _embeddedPresets.Add(embeddedPreset);
            }

            if (embeddedPreset.Preset == null)
            {
                embeddedPreset.Preset = new Preset(target)
                {
                    name = objectId.ToString(),
                };
                embeddedPreset.Preset.ExcludeAllProperties();
                AssetDatabase.AddObjectToAsset(embeddedPreset.Preset, this);
                EditorUtility.SetDirty(this);
            }
            return embeddedPreset.Preset;
        }

        public void HandleAssetDelete(string assetPath)
        {
            DeletePresetsFromAsset(assetPath);
        }

        public void HandleAssetSave(string[] paths)
        {
            foreach (string assetPath in paths)
            {
                DeletePresetsFromAsset(assetPath);
            }
        }

        private void DeletePresetsFromAsset(string assetPath)
        {
            var guid = new GUID(AssetDatabase.AssetPathToGUID(assetPath));
            _embeddedPresets.RemoveAll(p =>
            {
                bool shouldRemove = p.ObjectId.GlobalObjectId.assetGUID == guid
                    && GlobalObjectId.GlobalObjectIdentifierToObjectSlow(p.ObjectId) == null;
                if (shouldRemove)
                {
                    DestroyImmediate(p.Preset, true);
                }
                return shouldRemove;
            });
        }
    }
}
#endif