using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.UI.Images;
using Point = CodeMagic.Core.Game.Point;

namespace CodeMagic.Implementations.Objects.Buildings
{
    public class Furnace : WallBase, IUsableObject, IWorldImageProvider, IDynamicObject, ILightObject
    {
        private IFurnaceItem inputItem;
        private double currentProgress;

        public event EventHandler Used;

        public Furnace()
        {
            Temperature = int.MinValue;
        }

        private bool IsHot => inputItem != null && inputItem.MinTemperature < Temperature;

        public override string Name => "Furnace";

        public override bool CanConnectTo(IMapObject mapObject)
        {
            // TODO: Add connection to solid wall
            return false;
        }

        public int CurrentProgress => (int) Math.Floor(currentProgress);

        public int Temperature { get; private set; }

        public IFurnaceItem InputItem
        {
            get => inputItem;
            set
            {
                currentProgress = 0d;
                inputItem = value;
            }
        }

        public IItem OutputItem { get; set; }

        public void Use(IGameCore game, Point position)
        {
            Used?.Invoke(this, EventArgs.Empty);
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            var imageName = GetWorldImageName();
            return storage.GetImage(imageName);
        }

        private string GetWorldImageName()
        {
            if (HasConnectedTile(0, -1) && HasConnectedTile(0, 1))
                return IsHot ? "Building_Furnace_Right_Hot" : "Building_Furnace_Right_Cold";

            return IsHot ? "Building_Furnace_Down_Hot" : "Building_Furnace_Down_Cold";
        }

        public void Update(IAreaMap map, IJournal journal, Point position)
        {
            Temperature = GetBiggestTemperature(map, position);

            if (inputItem == null)
                return;

            if (OutputItem != null)
                return;

            if (Temperature > inputItem.MaxTemperature) // Item is burned
            {
                InputItem = null;
                return;
            }

            if (Temperature > inputItem.MinTemperature)
            {
                var diff = Temperature - inputItem.MinTemperature;
                var maxDiff = inputItem.MaxTemperature - inputItem.MinTemperature;
                var rate = 1 + diff / (double) maxDiff;
                currentProgress += rate;
            }

            if (Temperature < inputItem.MinTemperature)
            {
                currentProgress = Math.Max(0, currentProgress - 1d);
            }

            if (currentProgress >= inputItem.FurnaceProcessingTime)
            {
                OutputItem = inputItem.CreateFurnaceResult();
                InputItem = null;
            }
        }

        private int GetBiggestTemperature(IAreaMap map, Point position)
        {
            var temperatures = new[]
            {
                GetTemperature(map, position, Direction.East),
                GetTemperature(map, position, Direction.West),
                GetTemperature(map, position, Direction.North),
                GetTemperature(map, position, Direction.South)
            };
            return temperatures.Max();
        }

        private int GetTemperature(IAreaMap map, Point position, Direction direction)
        {
            var targetPos = Point.GetPointInDirection(position, direction);
            return map.TryGetCell(targetPos)?.Temperature ?? int.MinValue;
        }

        public bool Updated { get; set; }

        public UpdateOrder UpdateOrder => UpdateOrder.Early;

        public ILightSource[] LightSources
        {
            get
            {
                if (IsHot)
                {
                    return new ILightSource[] {new StaticLightSource(LightLevel.Dim1)};
                }

                return new ILightSource[0];
            }
        }
    }
}