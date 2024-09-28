using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "Inventory Item")]
    public class InventoryItedmData : ScriptableObject
    {
        public string Id;
        public string DisplayName;
        public int maxSizeStack;
        public Sprite Icon;
    }
}