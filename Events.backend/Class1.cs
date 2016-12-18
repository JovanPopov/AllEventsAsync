using System.Collections.Generic;

public class Venue
{
    public string street { get; set; }
    public string city { get; set; }
    public string state { get; set; }
    public string country { get; set; }
    public double latitude { get; set; }
    public double longitude { get; set; }
    public string full_address { get; set; }
}

public class Datum
{
    public string event_id { get; set; }
    public string eventname { get; set; }
    public string owner_id { get; set; }
    public string thumb_url { get; set; }
    public string thumb_url_large { get; set; }
    public int start_time { get; set; }
    public string start_time_display { get; set; }
    public int end_time { get; set; }
    public string end_time_display { get; set; }
    public string location { get; set; }
    public Venue venue { get; set; }
    public string label { get; set; }
    public int featured { get; set; }
    public string event_url { get; set; }
    public string share_url { get; set; }
    public string banner_url { get; set; }
    public double score { get; set; }
    public List<object> categories { get; set; }
    public List<object> tags { get; set; }
}

public class Request
{
    public string page { get; set; }
    public int rows { get; set; }
}

public class RootObject
{
    public string message { get; set; }
    public int error { get; set; }
    public List<Datum> data { get; set; }
    public Request request { get; set; }
    public int count { get; set; }
}