using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConvertExtensions
{
    public static int ToCharArray(this uint value, char[] buffer, int bufferIndex = 0)
    {
        if (value == 0)
        {
            buffer[bufferIndex] = '0';
            return 1;
        }

        int len = 1;
        for (uint rem = value / 10; rem > 0; rem /= 10)
            len++;

        for (int i = len - 1; i >= 0; i--)
        {
            buffer[bufferIndex + i] = (char)('0' + (value % 10));
            value /= 10;
        }

        return len;
    }
}
