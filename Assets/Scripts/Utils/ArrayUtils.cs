using System;
using System.Collections.Generic;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace Utils
{
    public class ArrayUtils
    {
        public static T GetRandomItem<T>(T[] arr) => arr[Random.Range(0, arr.Length)];
        public static T GetRandomItem<T>(List<T> arr) => arr[Random.Range(0, arr.Count)];
    }
}