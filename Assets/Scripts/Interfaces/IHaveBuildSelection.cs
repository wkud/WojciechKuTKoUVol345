using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TKOU.SimAI
{
    /// <summary>
    /// An object that have a building data selected for building.
    /// </summary>
    public interface IHaveBuildSelection
    {
        IAmData BuildSelection { get; set; }
    }
}
