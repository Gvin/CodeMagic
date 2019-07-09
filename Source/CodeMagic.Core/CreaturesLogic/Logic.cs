using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.CreaturesLogic.Strategies;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Core.CreaturesLogic
{
    public class Logic
    {
        private readonly List<ChangeStrategyRule> rules;
        private ICreatureStrategy currentStrategy;

        public Logic()
        {
            rules = new List<ChangeStrategyRule>();
        }

        public void SetInitialStrategy(ICreatureStrategy strategy)
        {
            currentStrategy = strategy;
        }

        public void AddTransferRule(ICreatureStrategy source, ICreatureStrategy target, Func<IAreaMap, Point, bool> condition)
        {
            rules.Add(new ChangeStrategyRule(source, target, condition));
        }

        public void Update(INonPlayableCreatureObject creature, IAreaMap map, Point position, Journal journal)
        {
            var endTurn = false;
            while (!endTurn)
            {
                var nextStrategy = GetNextStrategy(map, position);
                if (nextStrategy != null)
                {
                    currentStrategy = nextStrategy;
                }

                endTurn = currentStrategy.Update(creature, map, position, journal);
            }
        }

        private ICreatureStrategy GetNextStrategy(IAreaMap map, Point position)
        {
            var matchingRule = rules.FirstOrDefault(rule => rule.Source == currentStrategy && rule.Condition(map, position));
            return matchingRule?.Target;
        }

        private class ChangeStrategyRule
        {
            public ChangeStrategyRule(ICreatureStrategy source, ICreatureStrategy target, Func<IAreaMap, Point, bool> condition)
            {
                Source = source;
                Target = target;
                Condition = condition;
            }

            public ICreatureStrategy Source { get; }

            public ICreatureStrategy Target { get; }

            public Func<IAreaMap, Point, bool> Condition { get; }
        }
    }
}