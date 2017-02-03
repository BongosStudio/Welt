using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Welt.API;
using Welt.API.Forge;

namespace Welt.Core.AI
{
    public class AStarPathFinder
    {
        private readonly Vector3I[] m_Neighbors =
        {
            Vector3I.Forward,
            Vector3I.Right,
            Vector3I.Backward,
            Vector3I.Left
        };

        private readonly Vector3I[][] m_DiagonalNeighbors =
        {
            new[] { Vector3I.Forward, Vector3I.Right },
            new[] { Vector3I.Forward, Vector3I.Left },
            new[] { Vector3I.Backward, Vector3I.Right },
            new[] { Vector3I.Backward, Vector3I.Left },
        };

        private PathResult TracePath(Vector3I start, Vector3I goal, Dictionary<Vector3I, Vector3I> parents)
        {
            var list = new List<Vector3I>();
            var current = goal;
            while (current != start)
            {
                current = parents[current];
                list.Insert(0, current);
            }
            list.Add(goal);
            return new PathResult { Waypoints = list };
        }

        private bool CanOccupyVoxel(IWorld world, BoundingBox box, Vector3I voxel)
        {
            var id = world.GetBlock(voxel).Id;
            // TODO: Make this more sophisticated
            return id == 0;
        }

        private IEnumerable<Vector3I> GetNeighbors(IWorld world, BoundingBox subject, Vector3I current)
        {
            for (int i = 0; i < m_Neighbors.Length; i++)
            {
                var next = m_Neighbors[i] + current;
                if (CanOccupyVoxel(world, subject, next))
                    yield return next;
            }
            for (int i = 0; i < m_DiagonalNeighbors.Length; i++)
            {
                var pair = m_DiagonalNeighbors[i];
                var next = pair[0] + pair[1] + current;

                if (CanOccupyVoxel(world, subject, next)
                    && CanOccupyVoxel(world, subject, pair[0] + current)
                    && CanOccupyVoxel(world, subject, pair[1] + current))
                    yield return next;
            }
        }

        public PathResult FindPath(IWorld world, BoundingBox subject, Vector3I start, Vector3I goal)
        {
            // Thanks to www.redblobgames.com/pathfinding/a-star/implementation.html
            var parents = new Dictionary<Vector3I, Vector3I>();
            var costs = new Dictionary<Vector3I, double>();
            var openset = new PriorityQueue<Vector3I>();
            var closedset = new HashSet<Vector3I>();

            openset.Enqueue(start, 0);
            parents[start] = start;
            costs[start] = start.DistanceTo(goal);

            while (openset.Count > 0)
            {
                var current = openset.Dequeue();
                if (current == goal)
                    return TracePath(start, goal, parents);

                closedset.Add(current);

                foreach (var next in GetNeighbors(world, subject, current))
                {
                    if (closedset.Contains(next))
                        continue;
                    var cost = (int)(costs[current] + current.DistanceTo(next));
                    if (!costs.ContainsKey(next) || cost < costs[next])
                    {
                        costs[next] = cost;
                        var priority = cost + next.DistanceTo(goal);
                        openset.Enqueue(next, priority);
                        parents[next] = current;
                    }
                }
            }

            return null;
        }
    }
}