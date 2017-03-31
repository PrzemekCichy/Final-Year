using System;


//Used for reading json. Do not change anything in this class
public class Car
{
    public Car() { }

    [JsonProperty("Brand")]
    public string Brand { get; set; }

    [JsonProperty("Model")]
    public string Model { get; set; }

    [JsonProperty("Year")]
    public string Year { get; set; }

    [JsonProperty("Price")]
    public string Price { get; set; }

    [JsonProperty("Mileage")]
    public string Mileage { get; set; }

    [JsonProperty("BodyType")]
    public string BodyType { get; set; }

    [JsonProperty("Gearbox")]
    public string Gearbox { get; set; }

    [JsonProperty("Information")]
    public List<string> Information { get; set; }

    [JsonProperty("ImgPath")]
    public string ImgPath { get; set; }

    [JsonProperty("AddDate")]
    public string AddDate { get; set; }

    [JsonProperty("EditDate")]
    public string EditDate { get; set; }
}