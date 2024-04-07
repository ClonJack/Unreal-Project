using System;
using CrashKonijn.Goap.Interfaces;

namespace UnrealTeam.SB.GamePlay.AI.Brains
{
    public class GoalPredicate
    {
        public IGoalBase Goal { get; set; }
        public Func<bool> Predicate { get; set; }
        public bool EndAction { get; set; } = false;
    }
}