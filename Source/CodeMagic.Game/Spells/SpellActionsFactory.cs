using CodeMagic.Core.Spells;
using CodeMagic.Game.Objects;
using CodeMagic.Game.Spells.SpellActions;

namespace CodeMagic.Game.Spells
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
                case PushSpellAction.ActionType:
                    return new PushSpellAction(actionData);
                case CompressSpellAction.ActionType:
                    return new CompressSpellAction(actionData);
                case DecompressSpellAction.ActionType:
                    return new DecompressSpellAction(actionData);
                case CreateWaterSpellAction.ActionType:
                    return new CreateWaterSpellAction(actionData);
                case LongCastSpellAction.ActionType:
                    return new LongCastSpellAction(actionData, spell);
                case TransformWaterSpellAction.ActionType:
                    return new TransformWaterSpellAction(actionData);
                case ShockSpellAction.ActionType:
                    return new ShockSpellAction(actionData);
                case EmitLightSpellAction.ActionType:
                    return new EmitLightSpellAction(actionData, spell);
            }

            throw new SpellException($"Action type \"{actionData.type}\" is not supported.");
        }
    }
}