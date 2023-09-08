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
            public GUID AssetGuid;
            public Preset Preset;
        }

        [SerializeField] private List<EmbeddedPreset> _embeddedPresets = new List<EmbeddedPreset>();

        public Preset CreatePreset(GameObject ownerObject, Object target)
        {
            string assetPath = ownerObject.GetAssetPath();
            return CreatePreset(AssetDatabase.AssetPathToGUID(assetPath), target);
        }

        public Preset CreatePreset(string assetGuid, Object target)
        {
            if (string.IsNullOrEmpty(AssetDatabase.GUIDToAssetPath(assetGuid)))
            {
                throw new ArgumentOutOfRangeException(nameof(assetGuid), "GUID is not know in AssetDatabase");
            }

            var preset = new Preset(target)
            {
                name = $"{assetGuid}-{GlobalObjectId.GetGlobalObjectIdSlow(target).targetObjectId}",
            };
            preset.ExcludeAllProperties();
            _embeddedPresets.Add(new EmbeddedPreset
            {
                AssetGuid = new GUID(assetGuid),
                Preset = preset,
            });
            AssetDatabase.AddObjectToAsset(preset, this);
            AssetDatabase.SaveAssetIfDirty(this);
            return preset;
        }

        public void HandleAssetDelete(string assetPath)
        {
            var guid = new GUID(AssetDatabase.AssetPathToGUID(assetPath));
            _embeddedPresets.RemoveAll(p =>
            {
                bool shouldRemove = p.AssetGuid == guid;
                if (shouldRemove)
                {
                    DestroyImmediate(p.Preset, true);
                }
                return shouldRemove;
            });
        }

        public void DeleteOrphanPresets(string assetPath, HashSet<Preset> referencedPresets)
        {
            var guid = new GUID(AssetDatabase.AssetPathToGUID(assetPath));
            _embeddedPresets.RemoveAll(p =>
            {
                bool shouldRemove = p.AssetGuid == guid && !referencedPresets.Contains(p.Preset);
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