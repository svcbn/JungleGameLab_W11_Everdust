using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InversWeightedRandom
{
    int minInclusive;
    int maxExclusive;
    int[] count;
    float[] weights;

    public InversWeightedRandom(int _minInclusive, int _maxExclusive){
        minInclusive = _minInclusive;
        maxExclusive = _maxExclusive;
        count = new int[maxExclusive - minInclusive];
        Array.Fill(count, 0);
        weights = new float [maxExclusive - minInclusive];
        Array.Fill(weights, 1.0f);
    }

    // start 포함, end 미포함
    public int GetRandomInt(){
        float randomNum = UnityEngine.Random.Range(0, weights.Sum());
        float currentNum = 0;

        for(int i = 0 ; i < count.Length; ++i){
            if(currentNum <= randomNum && randomNum < currentNum + weights[i]){
                count[i] += 1;
                weights[i] = 1.0f / count[i];
                return i;
            }
            currentNum += weights[i];
        }

        Debug.Log("Some Error Occur");
        return -100;

    }

    // start 포함, end 미포함
    public int[] GetCount(){
        return count;
    }

}
