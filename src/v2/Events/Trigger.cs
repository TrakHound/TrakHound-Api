// Copyright (c) 2017 TrakHound Inc, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using TrakHound.Api.v2.Data;

namespace TrakHound.Api.v2.Events
{
    public class Trigger : IEvaluator
    {
        [XmlAttribute("filter")]
        public string Filter { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        [XmlAttribute("modifier")]
        public TriggerModifier Modifier { get; set; }


        public bool Evaluate(List<SampleInfo> samples)
        {
            if (!samples.IsNullOrEmpty())
            {
                // Find the Sample that matches the Trigger Filter
                var sample = samples.Find(o => CheckFilter(o));
                if (sample != null)
                {
                    // Check value
                    if (Value != null && (sample.CDATA != null || sample.Condition != null))
                    {
                        string x = Value;
                        string y = sample.CDATA != null ? sample.CDATA : sample.Condition;

                        return CheckValue(x, y);
                    }
                }
                // If not found then ignore
                else return true;
            }

            return false;
        }

        public bool CheckFilter(SampleInfo sample)
        {
            if (sample.DataItem != null)
            {
                var paths = Filter.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (paths != null)
                {
                    bool match = false;

                    for (int i = paths.Length - 1; i >= 0; i--)
                    {
                        var path = paths[i];

                        // Check DataItem
                        if (i == paths.Length - 1)
                        {
                            match = path == sample.Id || path == NormalizeType(sample.DataItem.Type);
                        }
                        // Check Component
                        else
                        {
                            match = sample.DataItem.Parents.Exists(o => o.Id == path || o.Type == path);
                        }

                        if (!match) return false;
                    }

                    return match;
                }
            }

            return false;
        }

        private bool CheckValue(string x, string y)
        {
            long x1;
            long y1;

            switch (Modifier)
            {
                case TriggerModifier.CONTAINS:

                    return Regex.IsMatch(x, "^(?=.*" + y + ").+$", RegexOptions.IgnoreCase);

                case TriggerModifier.CONTAINS_MATCH_CASE:

                    return Regex.IsMatch(x, "^(?=.*" + y + ").+$");

                case TriggerModifier.CONTAINS_WHOLE_WORD:

                    return Regex.IsMatch(x, y + "\\b", RegexOptions.IgnoreCase);

                case TriggerModifier.CONTAINS_WHOLE_WORD_MATCH_CASE:

                    return Regex.IsMatch(x, y + "\\b");

                case TriggerModifier.GREATER_THAN:

                    if (long.TryParse(x, out x1) && long.TryParse(y, out y1))
                    {
                        return x1 > y1;
                    }
                    break;

                case TriggerModifier.LESS_THAN:

                    if (long.TryParse(x, out x1) && long.TryParse(y, out y1))
                    {
                        return x1 < y1;
                    }
                    break;

                case TriggerModifier.NOT:

                    return x.ToLower() != y.ToLower();

                default:

                    return x.ToLower() == y.ToLower();
            }

            return false;
        }

        private static string NormalizeType(string s)
        {
            string debug = s;

            if (!string.IsNullOrEmpty(s))
            {
                if (s.ToUpper() != s)
                {
                    // Split string by Uppercase characters
                    var parts = Regex.Split(s, @"(?<!^)(?=[A-Z])");
                    s = string.Join("_", parts);
                    s = s.ToUpper();
                }

                // Return to Pascal Case
                s = s.Replace("_", " ");
                s = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());

                s = s.Replace(" ", "");
            }

            return s;
        }

        private static bool HasWildcard(string filter)
        {
            return filter.Length > 0 && filter[filter.Length - 1] == '*';
        }
    }
}
