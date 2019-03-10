using System;

[Serializable]
public class BrTargetSelection
{
    public enum MethodEnum
    {
        Closest,
        Random,
        Weakest
    }

    public MethodEnum Method;
    
    
}