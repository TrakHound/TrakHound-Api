// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using TrakHound.Api.v2.Data;

namespace TrakHound.Api.v2
{
    /// <summary>
    /// Filter DataItem by path 
    /// </summary>
    public class DataFilter
    {
        public string FilterString { get; set; }

        public DataItemDefinition DataItem { get; set; }

        public List<ComponentDefinition> Components { get; set; }


        public DataFilter(string filter, DataItemDefinition dataItem, List<ComponentDefinition> components)
        {
            FilterString = filter;
            DataItem = dataItem;
            Components = components;
        }

        public bool IsMatch()
        {
            if (!string.IsNullOrEmpty(FilterString))
            {
                string deviceId = DataItem.DeviceId;

                var paths = FilterString.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (paths != null)
                {
                    string componentId = DataItem.ParentId;
                    if (!string.IsNullOrEmpty(FilterString))
                    {
                        bool match = false;

                        for (var i = paths.Length - 1; i >= 0; i--)
                        {
                            match = false;
                            var path = paths[i];

                            // If Last Node in Path
                            if (i == paths.Length - 1)
                            {
                                if (HasWildcard(FilterString)) match = true;
                                else
                                {
                                    match = NormalizeType(DataItem.Type) == NormalizeType(path);
                                    if (!match) match = DataItem.Id == path;
                                }
                            }
                            else
                            {
                                var component = Components.Find(o => o.Id == componentId);

                                // Find if Direct Parent or if Descendant of path
                                while (component != null)
                                {
                                    match = component.Type == path;
                                    if (match) break;

                                    componentId = component.ParentId;
                                    component = Components.Find(o => o.Id == componentId);
                                }
                            }

                            if (!match) break;
                        }

                        return match;
                    }
                }
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

                return s;
            }

            return s;
        }

        private static bool HasWildcard(string filter)
        {
            return filter.Length > 0 && filter[filter.Length - 1] == '*';
        }
    }
}
