using System;

[Serializable]
public class BrTargetSelection
{
    public enum MethodEnum
    {
        Closest,
        All,
        Random,
        Weakest
    }

    public MethodEnum Method;
    
    
}