using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.CreaturesLogic.Strategies;
using CodeMagic.Core.Game;
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

        public void AddTransferRule(ICreatureStrategy source, ICreatureStrategy target, Func<INonPlayableCreatureObject, IAreaMap, Point, bool> condition)
        {
            rules.Add(new ChangeStrategyRule(source, target, condition));
        }

        public void Update(INonPlayableCreatureObject creature, IGameCore game, Point position)
        {
            var endTurn = false;
            while (!endTurn)
            {
                var nextStrategy = GetNextStrategy(creature, game.Map, position);
                if (nextStrategy != null)
                {
                    currentStrategy = nextStrategy;
                }

                endTurn = currentStrategy.Update(creature, game, position);
            }
        }

        private ICreatureStrategy GetNextStrategy(INonPlayableCreatureObject creature, IAreaMap map, Point position)
        {
            var matchingRule = rules.FirstOrDefault(rule => rule.Source == currentStrategy && rule.Condition(creature, map, position));
            return matchingRule?.Target;
        }

        private class ChangeStrategyRule
        {
            public ChangeStrategyRule(ICreatureStrategy source, ICreatureStrategy target, Func<INonPlayableCreatureObject, IAreaMap, Point, bool> condition)
            {
                Source = source;
                Target = target;
                Condition = condition;
            }

            public ICreatureStrategy Source { get; }

            public ICreatureStrategy Target { get; }

            public Func<INonPlayableCreatureObject, IAreaMap, Point, bool> Condition { get; }
        }
    }
}