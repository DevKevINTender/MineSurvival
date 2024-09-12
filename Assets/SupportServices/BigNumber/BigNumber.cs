using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

[Serializable]
[InlineProperty]
public struct BigNumber
{

    public static BigNumber Zero = new BigNumber(0, 0);
    public static BigNumber One = new BigNumber(1, 0);
    [SerializeField][JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    private float _value;
    [SerializeField][JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    private int _index;
    private static int indexStep = 1000;
    //private static readonly char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    private static readonly char[] alphabet = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

    public float Value { get => _value; set => _value = value; }
    public int Index { get => _index; set => _index = value; }

    public BigNumber(float value, int index)
    {
        _value = value;
        _index = index;
    }

    public override string ToString()
    {
        return $"{_value} {GetAlphabeticSuffix(_index)}";
    }

    public static BigNumber operator +(BigNumber num1, BigNumber num2)
    {
        BigNumber result = new BigNumber();
        if (num1._index == num2._index)
        {
            result._value = num1._value + num2._value;
            result._index = num1._index;
        }
        else if(Math.Abs(num1._index - num2._index) == 1)
        {
            result._index = Math.Max(num1._index, num2._index);

            float mainValue = num1._index > num2._index ? num1._value : num2._value;
            float additionalValue = num1._index > num2._index ? num2._value : num1._value;

            result._value = mainValue + additionalValue / indexStep; 
        }
        else if (Math.Abs(num1._index - num2._index) > 1)
        {
            result._index = Math.Max(num1._index, num2._index);
            result._value = num1._index > num2._index ? num1._value : num2._value;
        }
        result = ControllIndex(result);
        //MonoBehaviour.print($"Число: {num1._value} Индекс: {num1._index} + Число: {num2._value} Индекс: {num2._index} = Число: {result._value} Индекс: {result._index}");
        //MonoBehaviour.print($"{num1} + {num2} = {result}");

        return result;
    }

    public static BigNumber operator -(BigNumber num1, BigNumber num2)
    {
        BigNumber result = new BigNumber();

        if (num1._index == num2._index)
        {
            result._value = num1._value - num2._value; ;
            result._index = num1._index; ;
        }
        else if (Math.Abs(num1._index - num2._index) == 1)
        {
            result._index = Math.Max(num1._index, num2._index);

            float mainValue = num1._index > num2._index ? num1._value : num2._value;
            float additionalValue = num1._index > num2._index ? num2._value : num1._value;

            result._value = mainValue - additionalValue / indexStep;
        }
        else if (Math.Abs(num1._index - num2._index) > 1)
        {
            result._index = Math.Max(num1._index, num2._index);
            result._value = num1._index > num2._index ? num1._value : num2._value;
        }

        result = ControllIndex(result);
        //MonoBehaviour.print($"Число: {num1._value} Индекс: {num1._index} - Число: {num2._value} Индекс: {num2._index} = Число: {result._value} Индекс: {result._index}");

        return result;
    }

    public static BigNumber operator *(BigNumber num1, BigNumber num2)
    {
        BigNumber result = new BigNumber();

        result._value = num1._value * num2._value;
        result._index = num1._index + num2._index;

        result = ControllIndex(result);
        //MonoBehaviour.print($"Число: {num1._value} Индекс: {num1._index} * Число: {num2._value} Индекс: {num2._index} = Число: {result._value} Индекс: {result._index}");

        return result;
    }

    public static BigNumber operator /(BigNumber num1, BigNumber num2)
    {
        BigNumber result = new BigNumber();

        result._value = num1._value / num2._value;
        result._index = num1._index - num2._index;

        result = ControllIndex(result);
        //MonoBehaviour.print($"Число: {num1._value} Индекс: {num1._index} / Число: {num2._value} Индекс: {num2._index} = Число: {result._value} Индекс: {result._index}");

        return result;
    }

    public static bool operator >=(BigNumber num1, BigNumber num2)
    {
        if (num1._index == num2._index)
        {
            return num1._value >= num2._value;
        }
        return num1._index >= num2._index;
    }

    public static bool operator <=(BigNumber num1, BigNumber num2)
    {
        if (num1._index == num2._index)
        {
            return num1._value <= num2._value;
        }
        return num1._index <= num2._index;
    }

    public static bool operator >(BigNumber num1, BigNumber num2)
    {
        if (num1._index == num2._index)
        {
            return num1._value > num2._value;
        }
        return num1._index > num2._index;
    }

    public static bool operator <(BigNumber num1, BigNumber num2)
    {
        if (num1._index == num2._index)
        {
            return num1._value < num2._value;
        }
        return num1._index < num2._index;
    }

    public static bool operator ==(BigNumber num1, BigNumber num2)
    {
        return num1._index == num2._index && num1._value == num2._value;
    }

    public static bool operator !=(BigNumber num1, BigNumber num2)
    {
        return num1._index != num2._index || num1._value != num2._value;
    }

    public static BigNumber Pow(BigNumber number, int exponent)
    {
        BigNumber result = number;

        if (exponent == 0) return BigNumber.One;

        if (exponent == 1) return result;

        for (int i = 0; i < exponent; i++)
        {
            result = result * number;
        }

        result = ControllIndex(result);
        //MonoBehaviour.print($"{ToString()} ^ {exponent} = {result.ToString()}");

        return result;
    }

    private static BigNumber ControllIndex(BigNumber num)
    {
        BigNumber result = num;
        if(num._index >= int.MaxValue)
        {
            Debug.Log("Limits");
        }
        if (result._value <= 0 || result._index < 0)
        {
            result = Zero;
        }

        else if (result._value < 1 && result._value > 0)
        {
            while (result._value < 1 && result._value > 0)
            {
                result._value = result._value * indexStep;
                result._index = result._index - 1;
            }
            result._value = (float)Math.Round(result._value, 2);;
        }

        else if (result._value >= indexStep)
        {
            while (result._value >= indexStep)
            {
                result._value = result._value / indexStep;
                result._index = result._index + 1;
            }
            result._value = (float)Math.Round(result._value, 2);
        }
        result._value = (float)Math.Round(result._value, 2);
        return result;
    }

    private static string GetAlphabeticSuffix(int index)
    {
        if (index == 1)
        {
            return "K";
        }
        else if (index == 2)
        {
            return "M";
        }
        else if (index >= 3)
        {
            index -= 3;
            string suffix = "";
            while (index >= 0)
            {
                int remainder = index % 26;
                suffix = alphabet[remainder] + suffix;
                index = index / 26 - 1;
            }
            return suffix;
        }

        return "";
    }

}


