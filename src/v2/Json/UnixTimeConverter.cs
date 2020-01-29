// Copyright (c) 2020 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace TrakHound.Api.v2.Json
{
    public class UnixTimeConverter : DateTimeConverterBase
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dateTime = (DateTime)value;

            if (dateTime > DateTime.MinValue)
            {
                writer.WriteRawValue(Math.Round(((DateTime)value - UnixTimeExtensions.EpochTime).TotalMilliseconds, 0).ToString());
            }
            else
            {
                writer.WriteEnd();
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                if (reader.Value == null) { return null; }
                return UnixTimeExtensions.EpochTime.AddMilliseconds((long)reader.Value);
            }
            catch (Exception ex) { }

            return null;
        }
    }
}
