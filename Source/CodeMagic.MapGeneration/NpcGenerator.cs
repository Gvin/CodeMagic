using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Objects.Creatures.Implementations;
using CodeMagic.Implementations.Objects.Creatures.NonPlayable;

namespace CodeMagic.MapGeneration
{
    internal class NpcGenerator
    {
        private const int GoblinsCount = 20;
        private const int MinDistanceFromPlayer = 6;

        public void GenerateNpc(IAreaMap map, Point playerPosition)
        {
            PlaceCreatures(GoblinsCount, map, playerPosition);
        }

        private void PlaceCreatures(int count, IAreaMap map, Point playerPosition)
        {
            var maxIterations = count * count;
            var iteration = 0;
            var placed = 0;
            while (placed < count && iteration < maxIterations)
            {
                iteration++;

                var x = RandomHelper.GetRandomValue(0, 30);
                var y = RandomHelper.GetRandomValue(0, 30);
                var position = new Point(x, y);

                if (!CheckDistanceFromPlayer(playerPosition, position))
                    continue;

                var cell = map.TryGetCell(position);
                if (cell == null)
                    continue;

                if (cell.BlocksMovement)
                    continue;

                var npc = CreateNpc();
                map.AddObject(position, npc);
                placed++;
            }
        }

        private bool CheckDistanceFromPlayer(Point playerPosition, Point targetPosition)
        {
            return Point.GetDistance(playerPosition, targetPosition) > MinDistanceFromPlayer;
        }

        private NonPlayableCreatureObject CreateNpc()
        {
            return CreateGoblin();
        }

        private NonPlayableCreatureObject CreateGoblin()
        {
            return new GoblinImpl(new GoblinCreatureObjectConfiguration
            {
                Name = "Goblin",
                Health = 20,
                MaxHealth = 20,
                MinDamage = 2,
                MaxDamage = 5,
                HitChance = 60,
                VisibilityRange = 3
            });
        }
    }
}