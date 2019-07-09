using System;
using System.Collections.Generic;
using System.ComponentModel;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.PlayerData;

namespace CodeMagic.UI.Console.Drawing.JournalTextProviding
{
    public class JournalTextProviderFactory
    {
        private readonly Dictionary<Type, Func<IJournalMessage, string>> providers;

        public JournalTextProviderFactory()
        {
            providers = new Dictionary<Type, Func<IJournalMessage, string>>
            {
                {typeof(DealDamageMessage), GetDealDamageText},
                {typeof(DeathMessage), GetDeathText},
                {typeof(SpellOutOfManaMessage), GetSpellOutOfManaText},
                {typeof(SpellCastMessage), GetSpellCastTest},
                {typeof(NotEnoughManaMessage), GetNotEnoughManaText},
                {typeof(EnvironmentDamageMessage), GetEnvironmentDamageText},
                {typeof(SpellErrorMessage), GetSpellErrorText},
                {typeof(BurningDamageMessage), GetBurningDamageText}
            };
        }

        private Func<IJournalMessage, string> GetTextProvider(Type messageType)
        {
            if (providers.ContainsKey(messageType))
                return providers[messageType];

            throw new ArgumentException($"Unknown journal message type: {messageType.FullName}");
        }

        public string GetMessageText(IJournalMessage message)
        {
            var provider = GetTextProvider(message.GetType());
            return provider(message);
        }

        private string GetBurningDamageText(IJournalMessage message)
        {
            var damageMessage = (BurningDamageMessage) message;
            return $"{GetObjectName(damageMessage.Object)} got {damageMessage.Damage} fire damage from burning.";
        }

        private string GetDealDamageText(IJournalMessage message)
        {
            var damageMessage = (DealDamageMessage)message;
            return
                $"{GetObjectName(damageMessage.Source)} dealt {damageMessage.Damage} damage to {GetObjectName(damageMessage.Target)}.";
        }

        private string GetDeathText(IJournalMessage message)
        {
            var deathMessage = (DeathMessage)message;
            return $"{GetObjectName(deathMessage.Object)} died.";
        }

        private string GetSpellOutOfManaText(IJournalMessage message)
        {
            var outOfManaMessage = (SpellOutOfManaMessage) message;
            return $"Spell \"{outOfManaMessage.SpellName}\" is out of mana.";
        }

        private string GetSpellCastTest(IJournalMessage message)
        {
            var castMessage = (SpellCastMessage) message;
            return $"{GetObjectName(castMessage.Caster)} casted spell \"{castMessage.SpellName}\"";
        }

        private string GetNotEnoughManaText(IJournalMessage message)
        {
            return "You have not enough mana to cast this spell.";
        }

        private string GetEnvironmentDamageText(IJournalMessage message)
        {
            var envDamageMessage = (EnvironmentDamageMessage) message;
            return
                $"{GetObjectName(envDamageMessage.Object)} got {envDamageMessage.Damage} {GetElementName(envDamageMessage.Element)} damage from environment.";
        }

        private string GetSpellErrorText(IJournalMessage message)
        {
            var errorMessage = (SpellErrorMessage) message;
            return $"Error in spell \"{errorMessage.SpellName}\": {errorMessage.Message}";
        }

        private string GetElementName(Element? element)
        {
            if (!element.HasValue)
                return "physical";

            switch (element.Value)
            {
                case Element.Fire:
                    return "fire";
                case Element.Frost:
                    return "frost";
            }
            throw new InvalidEnumArgumentException($"Unknown element type: {element.Value}");
        }

        private string GetObjectName(IMapObject mapObject)
        {
            if (mapObject is IPlayer)
            {
                return "You";
            }

            return mapObject.Name;
        }
    }
}