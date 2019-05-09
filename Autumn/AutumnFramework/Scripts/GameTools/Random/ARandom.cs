﻿//=====================================================
// - FileName:      ARandom.cs
// - Author:       Autumn
// - CreateTime:    2019/05/09 16:39:21
// - Email:         543761701@qq.com
// - Description:   
// -  (C) Copyright 2019, webeye,Inc.
// -  All Rights Reserved.
//======================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Autumn
{
    public class ARandom
    {
        public static int seed = 31;
        public static readonly float precisionFactor = 1.0f / 65535.0f;

        static ARandom()
        {
            seed = (int)DateTime.Now.Ticks;
        }

        // return [min,max)
        public static int Range(int min, int max)
        {
            seed = 214013 * seed + 2531011;

            if (max > min)
            {
                return ((seed ^ seed >> 15) % (max - min)) + min;
            }
            else
            {
                return min;
            }
        }

        public static long Range(long min, long max)
        {
            seed = 214013 * seed + 2531011;

            if (max > min)
            {
                return ((seed ^ seed >> 15) % (max - min)) + min;
            }
            else
            {
                return min;
            }
        }

        // return [min,max]
        public static float Range(float min, float max)
        {
            seed = 214013 * seed + 2531011;

            if (max > min)
            {
                return min + (((UInt32)seed) >> 16) * precisionFactor * (max - min);
            }
            else
            {
                return min;
            }
        }

        // return [min,max) -- 不会改变当前种子(传入的种子不一定是素数)
        public static int Range(int min, int max, int newSeed)
        {
            int originalSeed = seed;
            seed = newSeed;
            int result = Range(min, max);
            seed = originalSeed;
            return result;
        }
        // return [min,max) -- 不会改变当前种子(传入的种子不一定是素数)
        public static float Range(float min, float max, int newSeed)
        {
            int originalSeed = seed;
            seed = newSeed;
            float result = Range(min, max);
            seed = originalSeed;
            return result;
        }

        public static int CalSeed(int inputSeed)
        {
            return 214013 * inputSeed + 2531011;
        }
    }
}
