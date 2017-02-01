// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;

namespace TrakHound.Api.v2.Data
{
    public class DataItemInfo : DataItemDefinition
    {
        public List<ComponentDefinition> Parents { get; set; }

        public DataItemInfo(DataItemDefinition definition, List<ComponentDefinition> components)
        {
            this.Id = definition.Id;
            this.Type = definition.Type;
            this.SubType = definition.SubType;
            this.Parents = DataItemInfo.GetParents(definition.ParentId, components);
        }

        private static List<ComponentDefinition> GetParents(string parentId, List<ComponentDefinition> components)
        {
            List<ComponentDefinition> componentDefinitionList = new List<ComponentDefinition>();
            if (!parentId.IsNullOrEmpty<char>())
            {
                ComponentDefinition componentDefinition = components.Find((Predicate<ComponentDefinition>)(o => o.Id == parentId));
                if (componentDefinition != null)
                {
                    componentDefinitionList.Add(componentDefinition);
                    componentDefinitionList.AddRange((IEnumerable<ComponentDefinition>)DataItemInfo.GetParents(componentDefinition.ParentId, components));
                }
            }
            return componentDefinitionList;
        }

        public static List<DataItemInfo> CreateList(List<DataItemDefinition> dataItems, List<ComponentDefinition> components)
        {
            List<DataItemInfo> dataItemInfoList = new List<DataItemInfo>();
            if (!dataItems.IsNullOrEmpty<DataItemDefinition>() && !components.IsNullOrEmpty<ComponentDefinition>())
            {
                foreach (DataItemDefinition dataItem in dataItems)
                    dataItemInfoList.Add(new DataItemInfo(dataItem, components));
            }
            return dataItemInfoList;
        }
    }
}
