using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.RightsManagement;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SVPP_CS_WPF_Lab2_task2_Product_Card
{
    [Serializable]
    public enum Categories {Телефоны, Техника, Мебель, Одежда, Посуда,  Обувь, Украшения, Другое}

    [Serializable]
    public class Product
    {
        public const string PriceCurrency = "BYN";

        private int articleId = 0;
        private string name = "No name";
        private string description = "No description";
        private string manufacturer = "No manufacturer";
        private Categories category = Categories.Другое;
        private bool bu = false;
        private double price = 0;
        private DateTime adoptionDate = DateTime.MinValue;
        private string photoPath = string.Empty;

        
        public int ArticleId { get => articleId; set => articleId = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public string Manufacturer { get => manufacturer; set => manufacturer = value; }
        public Categories Category { get => category; set => category = value; }
        public bool BU { get => bu; set => bu = value; }
        public double Price { get => price; set => price = value; }
        public DateTime AdoptionDate { get => adoptionDate; set => adoptionDate = value; }
        public string PhotoPath { get => photoPath; set => photoPath = value; }

        public Product() {}

        public Product(int articleId, string name, string description, string manufacturer, 
            Categories category, bool bU, double price,
            DateTime adoptionDate, string photoPath) :this()
        {
            ArticleId = articleId;
            Name = name;
            Description = description;
            Manufacturer = manufacturer;
            Category = category;
            BU = bU;
            Price = price;
            AdoptionDate = adoptionDate;
            PhotoPath = photoPath;
        }

        public override string ToString()
        {
            string st = $"ArticleID: {ArticleId}\nName: {Name}\n Category: {Category.ToString()}\n"+
                $"Manufacturer: {Manufacturer}\b/u: {BU}\nPrice: {price} {PriceCurrency}\n" +
                $"Adoption date: {AdoptionDate.ToShortDateString()}\nPhoto path: {PhotoPath}";
            return st;
        }

        /// <summary>
        /// Сериализует объект в файл json
        /// </summary>
        public string inJson()
        {
            string path = $"{Name}_{Manufacturer}_{AdoptionDate.ToShortDateString()}.json";
            

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never;           

            using (System.IO.Stream fs = new FileStream(path, FileMode.Create))
            {
                JsonSerializer.Serialize(fs, this, options);                
            }
            return path ;
        }

        /// <summary>
        /// Десериализует в файл json в объект Product
        /// </summary>
        public static Product CreateFromJSON(string pathJson)
        {
            FileStream fs = File.OpenRead(pathJson);
            Product? product = JsonSerializer.Deserialize<Product>(fs);

            if (product is null) throw new JsonException();
            else { return product; }
        }
    }
}
