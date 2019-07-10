using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Statuses
{
    public class OnFireObjectStatus : IObjectStatus
    {
        private const int FireDamageMin = 2;
        private const int FireDamageMax = 10;
        private const byte SelfExtinguishChance = 15;
        private const int CellTemperatureIncrease = 10;

        public const string StatusType = "on_fire";

        public bool Update(IDestroyableObject owner, AreaMapCell cell, Journal journal)
        {
            if (RandomHelper.CheckChance(SelfExtinguishChance))
            {
                return false;
            }
            var damage = RandomHelper.GetRandomValue(FireDamageMin, FireDamageMax);
            journal.Write(new BurningDamageMessage(owner, damage));
            owner.Damage(damage, Element.Fire);
            cell.Environment.Temperature += CellTemperatureIncrease;
            return true;
        }

        public string Type => StatusType;
    }
}