using System;
using System.Collections.Generic;
using System.Linq;
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

        public void AddTransferRule(ICreatureStrategy source, ICreatureStrategy target, Func<INonPlayableCreatureObject, Point, bool> condition)
        {
            rules.Add(new ChangeStrategyRule(source, target, condition));
        }

        public void Update(INonPlayableCreatureObject creature, Point position)
        {
            var endTurn = false;
            while (!endTurn)
            {
                var nextStrategy = GetNextStrategy(creature, position);
                if (nextStrategy != null)
                {
                    currentStrategy = nextStrategy;
                }

                endTurn = currentStrategy.Update(creature, position);
            }
        }

        private ICreatureStrategy GetNextStrategy(INonPlayableCreatureObject creature, Point position)
        {
            var matchingRule = rules.FirstOrDefault(rule => rule.Source == currentStrategy && rule.Condition(creature, position));
            return matchingRule?.Target;
        }

        private class ChangeStrategyRule
        {
            public ChangeStrategyRule(ICreatureStrategy source, ICreatureStrategy target, Func<INonPlayableCreatureObject, Point, bool> condition)
            {
                Source = source;
                Target = target;
                Condition = condition;
            }

            public ICreatureStrategy Source { get; }

            public ICreatureStrategy Target { get; }

            public Func<INonPlayableCreatureObject, Point, bool> Condition { get; }
        }
    }
}