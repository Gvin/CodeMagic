﻿using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Configuration.Buildings;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.Buildings
{
    public class BuildingSite : IMapObject, IUsableObject, IWorldImageProvider
    {
        private const string WorldImageName = "Building_Site";

        private readonly IMapObject building;
        private readonly Dictionary<Type, int> remainingCost;
        private readonly Dictionary<Type, string> resourcesNameMapping;
        private int remainingBuildTime;
        private readonly IBuildingConfiguration buildingConfiguration;

        public BuildingSite(IBuildingConfiguration buildingConfiguration)
        {
            building = CreateBuilding(buildingConfiguration.Type);
            this.buildingConfiguration = buildingConfiguration;
            remainingCost = buildingConfiguration.Cost.ToDictionary(cost => cost.Type, cost => cost.Count);
            resourcesNameMapping = buildingConfiguration.Cost.ToDictionary(cost => cost.Type, cost => cost.Name);
            remainingBuildTime = buildingConfiguration.BuildTime;
        }

        private static IMapObject CreateBuilding(Type buildingType)
        {
            if (!typeof(IMapObject).IsAssignableFrom(buildingType))
                throw new ApplicationException($"Invalid building type: {buildingType.FullName}");

            var constructor = buildingType.GetConstructor(new Type[0]);
            if (constructor == null)
                throw new ApplicationException($"Unable to find constructor without arguments for building type: {buildingType.FullName}");

            return constructor.Invoke(new object[0]) as IMapObject;
        }

        public string Name => $"{building.Name} Building Site";

        public bool BlocksMovement => false;

        public bool BlocksProjectiles => false;

        public bool BlocksAttack => false;

        public bool IsVisible => true;

        public bool BlocksVisibility => false;

        public bool BlocksEnvironment => false;

        public ZIndex ZIndex => ZIndex.AreaDecoration;

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(this, other);
        }

        public ObjectSize Size => ObjectSize.Huge;

        public void Use(IGameCore game, Point position)
        {
            if (remainingCost.Count != 0)
            {
                var availableMaterial = GetAvailableMaterialType(game.Player.Inventory);
                if (availableMaterial == null)
                {
                    game.Journal.Write(new ResourcesRequiredToBuildMessage(
                        remainingCost.ToDictionary(cost => resourcesNameMapping[cost.Key], cost => cost.Value),
                        remainingBuildTime));
                    return;
                }

                game.Player.Inventory.RemoveItem(availableMaterial);

                remainingCost[availableMaterial]--;
                if (remainingCost[availableMaterial] == 0)
                {
                    remainingCost.Remove(availableMaterial);
                }

                game.Journal.Write(new ResourcesRequiredToBuildMessage(
                    remainingCost.ToDictionary(cost => resourcesNameMapping[cost.Key], cost => cost.Value),
                    remainingBuildTime));
                return;
            }

            remainingBuildTime--;
            game.Journal.Write(new ResourcesRequiredToBuildMessage(
                null,
                remainingBuildTime));

            if (remainingBuildTime <= 0)
            {
                game.Map.RemoveObject(position, this);
                game.Map.AddObject(position, building);
                ApplyBuildingUnlocks(game);
                game.Journal.Write(new BuildingCompleteMessage(buildingConfiguration));
            }
        }

        private void ApplyBuildingUnlocks(IGameCore game)
        {
            foreach (var unlockId in buildingConfiguration.Unlocks)
            {
                var buildingConfig =
                    ConfigurationManager.Current.Buildings.Buildings.FirstOrDefault(b => string.Equals(b.Id, unlockId));
                if (buildingConfig == null)
                    throw new ApplicationException($"Building with id \"{unlockId}\" not found.");

                if (game.Player.UnlockBuilding(buildingConfig))
                {
                    game.Journal.Write(new BuildingUnlockedMessage(buildingConfig));
                }
            }
        }

        private Type GetAvailableMaterialType(Inventory playerInventory)
        {
            foreach (var materialType in remainingCost.Keys.ToArray())
            {
                if (playerInventory.GetItemsCount(materialType) > 0)
                    return materialType;
            }

            return null;
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(WorldImageName);
        }
    }
}