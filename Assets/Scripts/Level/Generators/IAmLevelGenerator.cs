using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKOU.SimAI.Levels
{
    /// <summary>
    /// Basic inteface for all level generators.
    /// </summary>
    public interface IAmLevelGenerator
    {
        public int minLevelWidth { get; }
        public int minLevelHeight { get; }

        /// <summary>
        /// Does a generation pass on the level using this generator.
        /// </summary>
        /// <param name="width">Width of the level</param>
        /// <param name="height">Height of the level</param>
        /// <returns><see cref="Level"/> base or NULL if not all entry conditions are fulfilled. </returns>
        public void GenerateLevel(Level level);

        /// <summary>
        /// Checks if the level is valid for the given generator.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool IsLevelValid(Level level);
    }
}
