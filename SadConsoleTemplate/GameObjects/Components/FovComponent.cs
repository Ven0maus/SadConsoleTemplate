using GoRogue;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using SadConsoleTemplate.GameObjects.Entities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SadConsoleTemplate.GameObjects.Components
{
    /// <summary>
    /// The field of view component
    /// </summary>
    public class FovComponent
    {
        private readonly Actor _actor;
        private readonly Distance _distanceCalculationMethod;
        private FOV _fieldOfView;

        public FovComponent(Actor actor, Distance distanceCalculationMethod)
        {
            _actor = actor;
            _distanceCalculationMethod = distanceCalculationMethod;
        }

        /// <summary>
        /// Initializes the Field Of View map, that this component should be based on
        /// </summary>
        /// <param name="fieldOfViewMap"></param>
        public void Initialize([NotNull] ArrayMap<bool> fieldOfViewMap)
        {
            _fieldOfView = new FOV(fieldOfViewMap);
        }

        /// <summary>
        /// Resets the Field Of View map that this component should be based on
        /// </summary>
        public void Reset()
        {
            _fieldOfView = null;
        }

        /// <summary>
        /// Calculates the FieldOfView component
        /// </summary>
        public void Calculate()
        {
            if (_fieldOfView == null) return;
            _fieldOfView.Calculate(_actor.Position, _actor.FieldOfViewRadius, _distanceCalculationMethod);
        }

        /// <summary>
        /// Returns true if the position is in the field of view of this actor
        /// If the FieldOfView component is not yet initialized, it will return false
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool PositionInFieldOfView(Point position)
        {
            if (_fieldOfView == null) return false;
            return _fieldOfView.BooleanFOV[position];
        }

        /// <summary>
        /// Returns all positions that are in the field of view of this actor
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Point> PositionsInFieldOfView()
        {
            if (_fieldOfView == null) yield break;
            for (int y = 0; y < _fieldOfView.BooleanFOV.Height; y++)
            {
                for (int x = 0; x < _fieldOfView.BooleanFOV.Width; x++)
                {
                    if (_fieldOfView.BooleanFOV[x, y])
                        yield return new Point(x, y);
                }
            }
        }
    }
}
