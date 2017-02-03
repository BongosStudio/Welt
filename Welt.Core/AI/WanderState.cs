using System;
using Welt.API.AI;
using Welt.API.Entities;
using System.Threading.Tasks;
using Welt.API;

namespace Welt.Core.AI
{
    public class WanderState : IMobState
    {
        /// <summary>
        /// Chance that mob will decide to move during an update when idle.
        /// Chance is equal to 1 / IdleChance.
        /// </summary>
        public int IdleChance { get; set; }
        /// <summary>
        /// The maximum distance the mob will move in an iteration.
        /// </summary>
        /// <value>The distance.</value>
        public int Distance { get; set; }
        public AStarPathFinder PathFinder { get; set; }

        public WanderState()
        {
            IdleChance = 10;
            Distance = 25;
            PathFinder = new AStarPathFinder();
        }

        public void Update(IMobEntity entity, IEntityManager manager)
        {
            var cast = entity as IEntity;
            if (entity.CurrentPath != null)
                entity.AdvancePath(manager.TimeSinceLastUpdate);
            else
            {
                if (FastMath.NextRandom(IdleChance) == 0)
                {
                    var target = new Vector3I(
                        (int)(cast.Position.X + (FastMath.NextRandom(Distance) - Distance / 2)),
                        0,
                        (int)(cast.Position.Z + (FastMath.NextRandom(Distance) - Distance / 2))
                    );
                    var adjusted = entity.World.FindBlockPosition(target, out var chunk, generate: false);
                    target.Y = chunk.GetHeight((byte)adjusted.X, (byte)adjusted.Z) + (uint)1;
                    Task.Factory.StartNew(() =>
                    {
                            entity.CurrentPath = PathFinder.FindPath(entity.World, entity.BoundingBox,
                                cast.Position, target);
                    });
                }
            }
        }
    }
}