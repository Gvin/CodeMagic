using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMagic.Core.Common;

namespace CodeMagic.Core.Game.Locations
{
    public class GameWorld
    {
        private readonly List<ILocation> storedLocations;

        public GameWorld(ILocation startingLocation)
        {
            storedLocations = new List<ILocation>();
            CurrentLocation = startingLocation;
        }

        public ILocation CurrentLocation { get; private set; }

        public void TravelToLocation(IGameCore game, ILocation newLocation, Direction enterDirection)
        {
            game.RemovePlayerFromMap();

            if (CurrentLocation.KeepOnLeave)
            {
                storedLocations.Add(CurrentLocation);
            }

            CurrentLocation.ProcessPlayerLeave(game);

            if (storedLocations.Contains(newLocation))
            {
                storedLocations.Remove(newLocation);
            }

            CurrentLocation = newLocation;
            CurrentLocation.ProcessPlayerEnter(game);
            var locationEnterDirection = DirectionHelper.InvertDirection(enterDirection);
            game.UpdatePlayerPosition(newLocation.GetEnterPoint(locationEnterDirection));
        }

        public void TravelToLocation(IGameCore game, string locationId, Direction enterDirection)
        {
            var location = storedLocations.FirstOrDefault(loc => string.Equals(loc.Id, locationId));
            if (location == null)
                throw new KeyNotFoundException($"Location with id \"{locationId}\" not found in stored locations.");

            TravelToLocation(game, location, enterDirection);
        }

        public void AddLocation(ILocation location)
        {
            if (storedLocations.Contains(location))
                throw new ArgumentException("Such location already exists in the world.");
            if (!location.KeepOnLeave)
                throw new ArgumentException("Added location should be marked as KeepOnLeave.");

            storedLocations.Add(location);
        }

        public Task UpdateStoredLocations(DateTime gameTime)
        {
            var updateTasks = storedLocations.Select(location => location.BackgroundUpdate(gameTime, CurrentLocation.TurnCycle)).ToArray();
            foreach (var updateTask in updateTasks.Where(task => !task.IsCompleted))
            {
                updateTask.Start();
            }
            return Task.WhenAll(updateTasks);
        }
    }
}