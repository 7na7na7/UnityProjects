using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine.UI;
// using DG.Tweening;
// using Spine.Unity;
namespace GoGo.Extension
{

    public enum AnimLabel
    {
        Bounce,
        Shake,
        Rotate

    }

    public static class LabelExtention
    {
        public static string AnimLabelToString(this AnimLabel _type, string _label)
        {
            string _result = "";
            switch (_type)
            {
                case AnimLabel.Bounce:
                    _result = string.Format("<bounce><fade>{0}",_label);
                    break;
                    
                case AnimLabel.Shake:
                    _result = string.Format("<shake><fade>{0}", _label);
                    break;

                case AnimLabel.Rotate:
                    _result = string.Format("<rotate><fade>{0}", _label);
                    break;
            }
            return _result;
        }
    }


    public enum OBJType
    {
        Default,
        DamageLabel,
        AnimatorText,

    }

    public enum EFFType
    {
        Default,
        Smoke_L,
    }


    #region 타입 익스텐션
    public static class TypeExtention
    {

        public static int OBJTypeToIDX(this OBJType _type)
        {
            int idx = 0;
            switch (_type)
            {
                case OBJType.Default:
                    break;
                case OBJType.DamageLabel:
                    idx = 1;
                    break;
                case OBJType.AnimatorText:
                    idx = 2;
                    break;
            }
            return idx;

        }

        public static int EFFTypeToIDX(this EFFType _type)
        {
            int idx = 0;
            switch (_type)
            {
                case EFFType.Default:
                    break;
                case EFFType.Smoke_L:
                    idx = 1;
                    break;
            }
            return idx;
        }
    }
    #endregion

    #region Color Extension
    public static class ColorExtension
    {
        // Example: "#ff000099".ToColor() red with alpha ~50%
        // Example: "ffffffff".ToColor() white with alpha 100%
        // Example: "00ff00".ToColor() green with alpha 100%
        // Example: "0000ff00".ToColor() blue with alpha 0%
        public static Color ToColor(this string color)
        {
            if (color.StartsWith("#", StringComparison.InvariantCulture))
            {
                color = color.Substring(1); // strip #
            }

            if (color.Length == 6)
            {
                color += "FF"; // add alpha if missing
            }

            var hex = Convert.ToUInt32(color, 16);
            var r = ((hex & 0xff000000) >> 0x18) / 255f;
            var g = ((hex & 0xff0000) >> 0x10) / 255f;
            var b = ((hex & 0xff00) >> 8) / 255f;
            var a = ((hex & 0xff)) / 255f;

            return new Color(r, g, b, a);
        }
        public static T ChangeAlpha<T>(this T g, float newAlpha) where T : Graphic
        {
            var color = g.color;
            color.a = newAlpha;
            g.color = color;
            return g;
        }
        public static SpriteRenderer ChangeSpriteAlpha(this SpriteRenderer g, float newAlpha)
        {
            var color = g.color;
            color.a = newAlpha;
            g.color = color;
            return g;
        }
        

    }
    #endregion
    
    #region Lerp Extension

    public enum SmoothType
    {
        SmootherStep,
        SoSmootherStep,
        EaseOutWithSin,
        EaseInWithCos,
        Exponential

    }
    public static class LerpExtenstion
    {
        public static float Interpolation(this float t, SmoothType type)
        {
            switch (type)
            {
                case SmoothType.SmootherStep:
                    return t * t * (3 - 2 * t);
                case SmoothType.SoSmootherStep:
                    return t * t * t * (t * (6f * t - 15f) + 10f);
                case SmoothType.EaseOutWithSin:
                    return Mathf.Sin(t * Mathf.PI * 0.5f);
                case SmoothType.EaseInWithCos:
                    return 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
                case SmoothType.Exponential:
                    return t * t;
                default:
                    return t;
            }
        }
        public static float PalabolaMethod(this float t, float height)
        {
            return -4 * height * t * t + 4 * height * t;
        }
    }
    #endregion

    #region String Extenstion
    public static class StringExtension
    {
        /// <summary>
        /// 1_3_4_5  <- 이런 형태의 string 을 "_separator" 로 분리하여 ->  List<int> = {1,3,4,5}
        /// </summary>
        /// <param name="_phrase"></param>
        /// <param name="_separator"></param>
        /// <returns></returns>
        public static List<int> SplitToList(this string _phrase, string _separator)
        {
            if (!_phrase.Contains(_separator))
            {
                return new List<int>(new int[] { int.Parse(_phrase) });
            }

            string[] indexes = Regex.Split(_phrase, _separator);
            List<int> indexList = new List<int>();
            foreach (string key in indexes)
            {
                indexList.Add(int.Parse(key));
            }
            return indexList;
        }

        public static List<float> SplitToList_f(this string _phrase, string _separator)
        {
            string[] indexes = Regex.Split(_phrase, _separator);
            List<float> indexList = new List<float>();
            foreach (string key in indexes)
            {
                indexList.Add(float.Parse(key));
            }
            return indexList;
        }

        public static string JoinToString(this List<int> _infoList, string _separator)
        {
            string joinPhrase = string.Join(_separator, _infoList.Select(i => i.ToString()).ToArray());
            return joinPhrase;
        }

        public static string[] SplitByChars(this string _phrase, string[] _separator)
        {
            string[] result = _phrase.Split(_separator, System.StringSplitOptions.RemoveEmptyEntries);
            return result;
        }
        public static string DeleteString(this string _pharase, string _delete)
        {
            return _pharase.Replace(_delete, "");
        }
        public static string Replace(this string s, Dictionary<string, string> substitutions)
        {
            string pattern = "";
            int i = 0;
            foreach (string ch in substitutions.Keys)
            {
                if (i == 0)
                    pattern += "(" + Regex.Escape(ch) + ")";
                else
                    pattern += "|(" + Regex.Escape(ch) + ")";
                i++;
            }

            var r = new Regex(pattern);
            var parts = r.Split(s);

            string ret = "";
            foreach (string part in parts)
            {
                if (part.Length == 1 && substitutions.ContainsKey(part[0].ToString()))
                {
                    ret += substitutions[part[0].ToString()];
                }
                else
                {
                    ret += part;
                }
            }
            return ret;
        }
        public static object[] StringToArray(this string input, string separator, Type type)
        {
            string[] stringList = input.Split(separator.ToCharArray(),
                                              StringSplitOptions.RemoveEmptyEntries);
            object[] list = new object[stringList.Length];

            for (int i = 0; i < stringList.Length; i++)
            {
                list[i] = Convert.ChangeType(stringList[i], type);
            }

            return list;
        }
    }


    #endregion


}
