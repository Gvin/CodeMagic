using CodeMagic.Core.CreaturesLogic.MovementStrategies;
using CodeMagic.Core.CreaturesLogic.Strategies;
using CodeMagic.Core.CreaturesLogic.TargetStrategies;

namespace CodeMagic.Core.CreaturesLogic.LogicConfigurators
{
    public class SimpleWonderingChaserLogicConfigurator : ILogicConfigurator
    {
        public void Configure(Logic logic)
        {
            var patrol = new FreeWonderStrategy();
            var attack = new ChaseTargetStrategy(new PlayerTargetStrategy(), new SimpleCreatureMovementStrategy());

            logic.SetInitialStrategy(patrol);

            logic.AddTransferRule(patrol, attack, LogicHelper.GetIfPlayerVisible);
            logic.AddTransferRule(attack, patrol, (creature, map, position) => !LogicHelper.GetIfPlayerVisible(creature, map, position));
        }
    }
}