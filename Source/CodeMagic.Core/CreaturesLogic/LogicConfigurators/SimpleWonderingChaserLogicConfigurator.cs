using CodeMagic.Core.CreaturesLogic.MovementStrategies;
using CodeMagic.Core.CreaturesLogic.Strategies;

namespace CodeMagic.Core.CreaturesLogic.LogicConfigurators
{
    public class SimpleWonderingChaserLogicConfigurator : ILogicConfigurator
    {
        public void Configure(Logic logic)
        {
            var patrol = new FreeWonderStrategy();
            var attack = new ChasePlayerStrategy(new SimpleCreatureMovementStrategy());

            logic.SetInitialStrategy(patrol);

            logic.AddTransferRule(patrol, attack, LogicHelper.GetIfPlayerVisible);
            logic.AddTransferRule(attack, patrol, (creature, map, position) => !LogicHelper.GetIfPlayerVisible(creature, map, position));
        }
    }
}