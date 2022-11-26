using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gilzoide.ConditionalObjects
{
    [Serializable]
    public class GameObjectOrComponent
    {
        public GameObject GameObject;
        public int ComponentIndex = -1;

        public Object Object => GameObject.ComponentAtIndexOrSelf(ComponentIndex);
    }
}
