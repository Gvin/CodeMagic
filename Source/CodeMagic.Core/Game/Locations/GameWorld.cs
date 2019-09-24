using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeMagic.Core.Common;

namespace CodeMagic.Core.Game.Locations
{
    public class GameWorld : IDisposable
    {
        private const int MaxRemainingUpdatesForTravel = 3;

        public event EventHandler TravelStarted;
        public event EventHandler TravelFinished;

        private readonly List<StoredLocation> storedLocations;
        private readonly Task backgroundUpdateTask;
        private readonly CancellationTokenSource backgroundUpdateCancellationToken;

        public GameWorld(ILocation startingLocation)
        {
            storedLocations = new List<StoredLocation>();
            CurrentLocation = startingLocation;

            backgroundUpdateCancellationToken = new CancellationTokenSource();
            backgroundUpdateTask = new Task(
                () => PerformBackgroundUpdate(backgroundUpdateCancellationToken.Token), 
                backgroundUpdateCancellationToken.Token, 
                TaskCreationOptions.LongRunning);
            backgroundUpdateTask.Start();
        }

        private void PerformBackgroundUpdate(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var updated = false;

                lock (storedLocations)
                {
                    foreach (var storedLocation in storedLocations)
                    {
                        updated = updated || storedLocation.PerformSingleBackgroundUpdate();
                    }
                }

                if (!updated)
                {
                    Task.Delay(500).Wait();
                }
            }
        }

        public ILocation CurrentLocation { get; private set; }

        public void TravelToLocation(IGameCore game, ILocation newLocation, Direction enterDirection, Point position = null)
        {
            if (string.Equals(newLocation.Id, CurrentLocation.Id))
            {
                if (position == null)
                    throw new ArgumentException("Unable to move through current location without position.");

                game.RemovePlayerFromMap();
                game.UpdatePlayerPosition(position);
                return;
            }

            lock (storedLocations)
            {
                var oldStoredLocation =
                    storedLocations.FirstOrDefault(loc => string.Equals(loc.Location.Id, newLocation.Id));

                if (oldStoredLocation != null && oldStoredLocation.RemainingUpdatesCount > MaxRemainingUpdatesForTravel)
                {
                    TravelStarted?.Invoke(this, EventArgs.Empty);
                }

                game.RemovePlayerFromMap();

                if (CurrentLocation.KeepOnLeave)
                {
                    storedLocations.Add(new StoredLocation(CurrentLocation));
                }

                CurrentLocation.ProcessPlayerLeave(game);

                
                if (oldStoredLocation != null)
                {
                    oldStoredLocation.PerformBackgroundUpdates();
                    storedLocations.Remove(oldStoredLocation);
                }

                CurrentLocation = newLocation;
                CurrentLocation.ProcessPlayerEnter(game);
                if (position == null)
                {
                    var locationEnterDirection = DirectionHelper.InvertDirection(enterDirection);
                    game.UpdatePlayerPosition(newLocation.GetEnterPoint(locationEnterDirection));
                }
                else
                {
                    game.UpdatePlayerPosition(position);
                }

                TravelFinished?.Invoke(this, EventArgs.Empty);
            }
        }

        public void TravelToLocation(IGameCore game, string locationId, Direction enterDirection, Point position = null)
        {
            if (string.Equals(CurrentLocation.Id, locationId))
            {
                TravelToLocation(game, CurrentLocation, enterDirection, position);
                return;
            }

            var storedLocation = GetStoredLocation(locationId);
            TravelToLocation(game, storedLocation.Location, enterDirection, position);
        }

        private StoredLocation GetStoredLocation(string id)
        {
            lock (storedLocations)
            {
                return storedLocations.FirstOrDefault(loc => string.Equals(loc.Location.Id, id));
            }
        }

        public void AddLocation(ILocation location)
        {
            lock (storedLocations)
            {
                if (storedLocations.Any(loc => string.Equals(loc.Location.Id, location.Id)))
                    throw new ArgumentException("Such location already exists in the world.");
                if (!location.KeepOnLeave)
                    throw new ArgumentException("Added location should be marked as KeepOnLeave.");

                storedLocations.Add(new StoredLocation(location));
            }
        }

        public void UpdateStoredLocations(GameTimeManager gameTimeManager)
        {
            lock (storedLocations)
            {
                var localTimeManager = gameTimeManager.Clone();

                for (int counter = 0; counter < CurrentLocation.TurnCycle; counter++)
                {
                    localTimeManager.RegisterTurn(1);
                    foreach (var storedLocation in storedLocations)
                    {
                        storedLocation.AddBackgroundUpdate(localTimeManager.CurrentTime);
                    }
                }
            }
        }

        private class StoredLocation
        {
            public StoredLocation(ILocation location)
            {
                Location = location;
                backgroundUpdates = new List<BackgroundUpdate>();
            }

            public ILocation Location { get; }

            private readonly List<BackgroundUpdate> backgroundUpdates;

            public void AddBackgroundUpdate(DateTime gameTime)
            {
                backgroundUpdates.Add(new BackgroundUpdate(gameTime));
            }

            public bool PerformSingleBackgroundUpdate()
            {
                var backgroundUpdate = backgroundUpdates.FirstOrDefault();
                if (backgroundUpdate == null)
                    return false;

                Location.BackgroundUpdate(backgroundUpdate.GameTime);
                backgroundUpdates.Remove(backgroundUpdate);

                return true;
            }

            public void PerformBackgroundUpdates()
            {
                foreach (var backgroundUpdate in backgroundUpdates.ToArray())
                {
                    Location.BackgroundUpdate(backgroundUpdate.GameTime);
                    backgroundUpdates.Remove(backgroundUpdate);
                }
            }

            public int RemainingUpdatesCount => backgroundUpdates.Count;

            private class BackgroundUpdate
            {
                public BackgroundUpdate(DateTime gameTime)
                {
                    GameTime = gameTime;
                }

                public DateTime GameTime { get; }
            }
        }

        public void Dispose()
        {
            backgroundUpdateCancellationToken.Cancel();
            backgroundUpdateTask.Wait();
            backgroundUpdateTask.Dispose();
        }
    }
}