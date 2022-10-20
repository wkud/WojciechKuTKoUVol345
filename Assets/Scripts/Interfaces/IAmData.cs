using UnityEngine;

namespace TKOU.SimAI
{
    /// <summary>
    /// All datas should use this interface.
    /// </summary>
    public interface IAmData
    {
        /// <summary>
        /// Sprite representing this data for UI.
        /// </summary>
        Sprite DataIcon { get; }

        /// <summary>
        /// Name representing this data.
        /// </summary>
        string DataName { get; }

        /// <summary>
        /// The prefab for this data, if any
        /// </summary>
        public IAmEntity EntityPrefab { get; }
    }
}