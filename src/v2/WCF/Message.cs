// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.ServiceModel;

namespace TrakHound.Api.v2.WCF
{
    public class Message
    {
        public Message() { }

        public Message(string id, string text)
        {
            Id = id;
            Text = text;
        }

        public string Id { get; set; }

        public string Text { get; set; }
    }

    public interface IMessageCallback
    {
        [OperationContract]
        void OnCallback(Message data);
    }

    [ServiceContract(CallbackContract = typeof(IMessageCallback))]
    public interface IMessage
    {
        [OperationContract]
        object SendData(Message data);
    }
}
