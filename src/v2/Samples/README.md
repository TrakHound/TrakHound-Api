# TrakHound Samples Api

Used to get raw sample data for device. Similar to the sample data directly from MTConnect.
Data can be sent or returned in JSON or BSON.
Piggyback to MTConnect in that it stores data permanently and allows user accounts and grouping.
Timestamps are sent and retrieved as Epoch milliseconds to reduce size of transfer

## Read
Samples are read using an HTTP GET request. If no parameters (other than authentication) are given, the
most current samples are returned (similar to MTConnect Current).

[GET] http://api.trakhound.com/users/batman/samples/[json or bson]

Parameters
--------------
- access_token
- from
- to
- at
- count


[POST] http:/api.trakhound.com/users/batman/samples/[json or bson]

Parameters
--------------
- access_token
- sample data (JSON). See below:

[
    {
     "device_id":"123457"
     "samples":
     [
         {
          "id":"cn3", (MTConnect DataItemId)
          "value_1":"FAULT",
          "value_2":"Coolant Low",
          "timestamp":"1482046182994" (Epoch Time in milliseconds)
         }
     ]
    }
]
