namespace CodeMagic.Core.Spells.SpellActions
{
    public class SpellActionsFactory
    {
        public ISpellAction GetSpellAction(dynamic actionData, CodeSpell spell)
        {
            if (string.IsNullOrEmpty(actionData.type))
                throw new SpellException("Action type cannot be empty.");

            switch (actionData.type)
            {
                case MoveSpellAction.ActionType:
                    return new MoveSpellAction(actionData, spell);
                case BuildWallSpellAction.ActionType:
                    return new BuildWallSpellAction(actionData);
                case HeatAreaSpellAction.ActionType:
                    return new HeatAreaSpellAction(actionData);
                case CoolAreaSpellAction.ActionType:
                    return new CoolAreaSpellAction(actionData);
            }

            throw new SpellException($"Action type \"{actionData.type}\" is not supported.");
        }
    }
}