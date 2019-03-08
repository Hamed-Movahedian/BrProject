using System;

[Serializable]
public class BrFleeTargertSelection
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