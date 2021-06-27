
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace BehinRahkar.Catalog.Domain.Models
{
    public class Product
    {
        
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; } 
        public decimal Price{ get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public bool IsConfirmed
        {
            get { return _isConfirmed; }
            private set { _isConfirmed = value; }
        }

        //----Data Concurrency-----------------
        [Timestamp]
        public byte[] RowVersion { get; set; }
        //-------------------------------------


        private bool _isConfirmed;
        public Product()
        {
        }

        public Product(string code, string name, string photo, decimal price)
        {
            Code = code;
            Name = name;
            Price = price;
            Photo = string.IsNullOrEmpty(photo) ? null : $"{Guid.NewGuid()}{Path.GetExtension(photo)}";
            _isConfirmed = Price <= 999;
        }


        public void ChangeEditableProperties(string name, string photo, decimal? price)
        {
            if (!string.IsNullOrEmpty(name))
                Name = name;
            if (!string.IsNullOrEmpty(photo))
                Photo = $"{Guid.NewGuid()}{Path.GetExtension(photo)}";
            if (price.HasValue)
            {
                Price = price.Value;
                _isConfirmed = Price <= 999;
            }
        }

        public void Confirm()
        {
            _isConfirmed = true;
        }
    }
}
