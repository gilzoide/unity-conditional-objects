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
        #region Instance

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

        #endregion

        [Serializable]
        internal class EmbeddedPresets : IComparable<EmbeddedPresets>
        {
            public GUID AssetGuid;
            public List<Preset> PresetList;

            public int CompareTo(EmbeddedPresets other)
            {
                return AssetGuid.CompareTo(other.AssetGuid);
            }
        }

        [SerializeField] private List<EmbeddedPresets> _embeddedPresets = new List<EmbeddedPresets>();

        public Preset CreatePreset(GameObject ownerObject, Object target)
        {
            string assetPath = ownerObject.GetAssetPath();
            return CreatePreset(assetPath, target);
        }

        public void HandleAssetDelete(string assetPath)
        {
            RemoveEmbeddedPresets(assetPath);
        }

        public void DeleteOrphanPresets(string assetPath, HashSet<Preset> referencedPresets)
        {
            GetEmbeddedPresets(assetPath)?.PresetList.RemoveAll(p =>
            {
                bool shouldRemove = !referencedPresets.Contains(p);
                if (shouldRemove)
                {
                    DestroyImmediate(p, true);
                }
                return shouldRemove;
            });
        }

        private Preset CreatePreset(string assetPath, Object target)
        {
            var guid = new GUID(AssetDatabase.AssetPathToGUID(assetPath));
            var preset = new Preset(target)
            {
                name = $"{guid}-{GlobalObjectId.GetGlobalObjectIdSlow(target).targetObjectId}",
                hideFlags = HideFlags.HideInHierarchy,
            };
            preset.ExcludeAllProperties();
            AssetDatabase.AddObjectToAsset(preset, this);
            GetOrCreateEmbeddedPresets(assetPath).PresetList.Add(preset);
            return preset;
        }

        private EmbeddedPresets GetEmbeddedPresets(string assetPath)
        {
            var guid = new GUID(AssetDatabase.AssetPathToGUID(assetPath));
            int index = _embeddedPresets.BinarySearch(new EmbeddedPresets { AssetGuid = guid });
            if (index >= 0)
            {
                return _embeddedPresets[index];
            }
            else
            {
                return null;
            }
        }

        private EmbeddedPresets GetOrCreateEmbeddedPresets(string assetPath)
        {
            var guid = new GUID(AssetDatabase.AssetPathToGUID(assetPath));
            int index = _embeddedPresets.BinarySearch(new EmbeddedPresets { AssetGuid = guid });
            if (index >= 0)
            {
                return _embeddedPresets[index];
            }
            else
            {
                var embeddedPreset = new EmbeddedPresets { AssetGuid = guid, PresetList = new List<Preset>() };
                _embeddedPresets.Insert(~index, embeddedPreset);
                return embeddedPreset;
            }
        }

        private void RemoveEmbeddedPresets(string assetPath)
        {
            var guid = new GUID(AssetDatabase.AssetPathToGUID(assetPath));
            int index = _embeddedPresets.BinarySearch(new EmbeddedPresets { AssetGuid = guid });
            if (index >= 0)
            {
                _embeddedPresets.RemoveAt(index);
            }
        }
    }
}
#endif