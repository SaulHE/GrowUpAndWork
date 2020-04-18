﻿namespace GrowUpAndWorkLib.Interfaces
{
    public interface IAction
    {
        Ref Context { get; }
        object Value { get; }
        void Do();
        void Undo();
    }
}
