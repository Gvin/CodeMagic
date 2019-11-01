using CodeMagic.Core.CreaturesLogic;
using CodeMagic.Game.CreaturesLogic.MovementStrategies;
using CodeMagic.Game.CreaturesLogic.Strategies;
using CodeMagic.Game.CreaturesLogic.TargetStrategies;

namespace CodeMagic.Game.CreaturesLogic.LogicConfigurators
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