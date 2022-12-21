using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class HelperFunctions
{


    public static int RandomWeighted(List<float> weights)
    {
        float weightTotal = weights.Sum();
        int result = 0;
        float total = 0;
        float randVal = Random.Range(0f, weightTotal);
        for (result = 0; result < weights.Count; result++)
        {
            total += weights[result];
            if (total >= randVal) break;
        }
        return result;
    }
}
