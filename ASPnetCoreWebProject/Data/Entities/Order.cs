using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaWebsite.Data.Entities
{
    public class Order
    {
        [Key]

        public int Id { get; set; }
        public string UserId { get; set; }
        public List<CartItem> CartItems { get; set; }
        public int CartId { get; set; }

        public Cart Cart { get; set; }
        
        public virtual string DelivererId { get; set; }

        public PaymentType PaymentType { get; set; }

        public string CustomerEmail { get; set; }

        public string CustomerFirstName { get; set; }

        public string CustomerLastName { get; set; }

        public DateTime OrderTime { get; set; }

        public string DeliveryArea { get; set; }

        public string DeliveryAddress { get; set; }

        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        [Required]
        public Status Status { get; set; }

        public ReceptionMethod ReceptionMethod { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? TimeAccepted { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? TimeCompleted { get; set; }
        public UserData UserData { get; set; }
    }

    public enum Status
    {
        Ordered, //When front end or user makes an order.
        Preparing, //When cook is cooking items in an order.
        Ready, //When cook is done cooking items and its ready for pickup/delivery
        Pending, //When either currently delivering or currently waiting for pickup
        Completed //When order has been delivered or picked up.
    }

    public enum PaymentType
    {
        Cash,
        Credit
    }

    public enum ReceptionMethod
    {
        Pickup,
        Delivery
    }
}
