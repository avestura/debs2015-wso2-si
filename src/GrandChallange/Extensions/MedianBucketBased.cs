using System;
using System.Collections.Generic;
using System.Text;

namespace GrandChallange.Extensions
{
    class MedianBucketBased
    {
        const int multiplexer = 1;
        const int size = 3016 * multiplexer;
        int[] mediationArray = new int[size];
        int totalElements = 0;

        float lastReturnedMedian = 0;

        public float GetMedian()
        {

            if (totalElements % 2 == 0)
            {
                int firstMedianIndex = ((totalElements) / 2);
                int secondMedianIndex = ((totalElements) / 2) + 1;

                int firstMedianValue = 0;
                int secondMedianValue = 0;
                bool flag = true;

                int count = 0;
                int loopCount = 0;
                foreach (int occurrenceCount in mediationArray)
                {
                    count = count + occurrenceCount;

                    if (firstMedianIndex <= count && flag)
                    {
                        firstMedianValue = loopCount;
                        flag = false;
                        loopCount++;
                        continue;
                    }
                    if (secondMedianIndex <= count)
                    {
                        secondMedianValue = loopCount;
                        break;
                    }
                    loopCount++;
                }
                lastReturnedMedian = (firstMedianValue + secondMedianValue) / 2f;


            }
            else
            {
                int medianIndex = ((totalElements - 1) / 2) + 1;
                int count = 0;
                int medianValue = 0;
                int loopCount = 0;
                foreach (int medianCount in mediationArray)
                {
                    count = count + medianCount;
                    if (medianIndex <= count)
                    {
                        medianValue = loopCount;
                        break;
                    }
                    loopCount++;
                }
                lastReturnedMedian = medianValue / multiplexer;
            }

            return lastReturnedMedian;
        }


    }
}
