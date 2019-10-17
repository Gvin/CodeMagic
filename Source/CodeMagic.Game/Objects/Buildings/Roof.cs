using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;
using CodeMagic.Game.JournalMessages;

namespace CodeMagic.Game.Objects.Buildings
{
    public class Roof : IRoofObject, IDynamicObject
    {
        public const string BuildingKey = "roof";

        private const int CollapseDamage = 5;
        private const int MaxSupportDistance = 3;

        public string Name => "Roof";
        public bool BlocksMovement => false;
        public bool BlocksProjectiles => false;
        public bool IsVisible => false;
        public bool BlocksVisibility => false;
        public bool BlocksAttack => false;
        public bool BlocksEnvironment => false;
        public ZIndex ZIndex => ZIndex.Roof;

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(this, other);
        }

        public ObjectSize Size => ObjectSize.Huge;

        public void Update(IAreaMap map, IJournal journal, Point position)
        {
            var supportArea = map.GetMapPart(position, MaxSupportDistance);
            var hasSupport = supportArea.Any(row => row.Any(cell => cell != null && cell.Objects.OfType<IRoofSupport>().Any()));
            if (hasSupport)
                return;

            journal.Write(new RoofCollapsedMessage());
            var objectsUnderRoof = map.GetCell(position).Objects.OfType<IDestroyableObject>();
            foreach (var destroyable in objectsUnderRoof)
            {
                journal.Write(new EnvironmentDamageMessage(destroyable, CollapseDamage, Element.Blunt));
                destroyable.Damage(journal, CollapseDamage, Element.Blunt);
            }
            map.RemoveObject(position, this);
        }

        public bool Updated { get; set; }
        public UpdateOrder UpdateOrder => UpdateOrder.Late;
    }
}