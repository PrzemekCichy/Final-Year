using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json; //For manipulating JSON Data
using System.IO;

namespace FinalProject
{
    public class Car
    {
        private string imgPath;
        private string brand;

        private List<string> information;
        private int mileage;
        private string model;
        private double price;
        private int year;

        public Car()
        {
            Brand = "";
            AddDate = new DateTime(DateTime.Now.Ticks, DateTimeKind.Utc);
            EditDate = new DateTime(DateTime.Now.Ticks, DateTimeKind.Utc);
            information = new List<string>();
        }

        public Car(Car duplicateCar)
        {
            this.BodyType = duplicateCar.BodyType;
            this.Information = duplicateCar.Information;
            this.Mileage = duplicateCar.Mileage;
            this.Model = duplicateCar.Model;
            this.Price = duplicateCar.Price;
            this.Year = duplicateCar.Year;
            this.Gearbox = duplicateCar.Gearbox;
            this.Brand = duplicateCar.Brand;
            this.ImgPath = duplicateCar.imgPath;
            AddDate = new DateTime(DateTime.Now.Ticks, DateTimeKind.Utc);
            EditDate = new DateTime(DateTime.Now.Ticks, DateTimeKind.Utc);
        }

        [JsonProperty("Brand")]
        public string Brand {
            get
            {
                return brand;
            }
            set {
                EditDate = new DateTime(DateTime.Now.Ticks, DateTimeKind.Utc);
                this.brand = value;
            }
        }

        [JsonProperty("Model")]
        public string Model {
            get
            {
                return model;
            }
            set
            {
                EditDate = new DateTime(DateTime.Now.Ticks, DateTimeKind.Utc);
                this.model = value;
            }
        }

        [JsonProperty("Year")]
        public int Year
        {
            get
            {
                return year;
            }
            set
            {
                EditDate = new DateTime(DateTime.Now.Ticks, DateTimeKind.Utc);
                this.year = value;
            }
        }

        [JsonProperty("Price")]
        public double Price
        {
            get
            {
                return price;
            }
            set
            {
                EditDate = new DateTime(DateTime.Now.Ticks, DateTimeKind.Utc);
                this.price = value;
            }
        }

        [JsonProperty("Mileage")]
        public int Mileage
        {
            get
            {
                return mileage;
            }
            set
            {
                EditDate = new DateTime(DateTime.Now.Ticks, DateTimeKind.Utc);
                this.mileage = value;
            }
        }

        [JsonProperty("BodyType")]
        public string BodyType
        {
            get;
            set;
        }

        [JsonProperty("Gearbox")]
        public string Gearbox { get; set; }

        [JsonProperty("Information")]
        public List<string> Information
        {
            get
            {
                return information;
            }
            set
            {
                this.information = value;
            }
        }

        [JsonProperty("ImgPath")]
        public string ImgPath
        {
            get
            {
                try
                {
                    return Path.GetFullPath("image\\" + imgPath);
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                EditDate = new DateTime(DateTime.Now.Ticks, DateTimeKind.Utc);
                imgPath = Path.GetFileName(value);                
            }
        }

        [JsonProperty("AddDate")]
        public DateTime AddDate
        {
            get; set;
        }

        [JsonProperty("EditDate")]
        public DateTime EditDate { get; set; }
    }
}
