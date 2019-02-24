
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static T RandomSelection<T>(this List<T> list, System.Random random)
    {
        return list[random.Next(list.Count)];
    }    
    
    public static T RandomSelection<T>(this List<T> list)
    {
        return list[Random.Range(0,list.Count)];
    }
}