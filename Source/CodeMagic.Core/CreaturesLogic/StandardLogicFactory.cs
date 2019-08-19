using System;
using System.Collections.Generic;
using CodeMagic.Core.CreaturesLogic.LogicConfigurators;

namespace CodeMagic.Core.CreaturesLogic
{
    public static class StandardLogicFactory
    {
        private static readonly Dictionary<string, ILogicConfigurator> Configurators = new Dictionary<string, ILogicConfigurator>
        {
            {"SimplePatrollingChaser", new SimplePatrollingChaserLogicConfigurator()},
            {"SimpleWonderingChaser", new SimpleWonderingChaserLogicConfigurator()}
        };

        public static ILogicConfigurator GetConfigurator(string logicName)
        {
            if (Configurators.ContainsKey(logicName))
                return Configurators[logicName];

            throw new ArgumentException($"Logic name not found: {logicName}");
        }
    }
}