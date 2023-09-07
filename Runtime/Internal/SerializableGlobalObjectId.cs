#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Gilzoide.ConditionalObjects
{
    [Serializable]
    public struct SerializableGlobalObjectId : ISerializationCallbackReceiver, IEquatable<SerializableGlobalObjectId>, IEquatable<GlobalObjectId>, ISingleFieldProperty
    {
        [SerializeField, ReadOnlyProperty] private string _globalObjectIdText;
        [NonSerialized] public GlobalObjectId GlobalObjectId;

        public SerializableGlobalObjectId(GlobalObjectId globalObjectId)
        {
            GlobalObjectId = globalObjectId;
            _globalObjectIdText = globalObjectId.ToString();
        }

        public void OnAfterDeserialize()
        {
            GlobalObjectId.TryParse(_globalObjectIdText, out GlobalObjectId);
        }

        public void OnBeforeSerialize()
        {
            _globalObjectIdText = GlobalObjectId.ToString();
        }

        public bool Equals(GlobalObjectId other)
        {
            return GlobalObjectId.Equals(other);
        }

        public bool Equals(SerializableGlobalObjectId other)
        {
            return GlobalObjectId.Equals(other.GlobalObjectId);
        }

        public static implicit operator SerializableGlobalObjectId(GlobalObjectId globalObjectId)
        {
            return new SerializableGlobalObjectId(globalObjectId);
        }

        public static implicit operator GlobalObjectId(SerializableGlobalObjectId serializedGlobalObjectId)
        {
            return serializedGlobalObjectId.GlobalObjectId;
        }
    }
}
#endif