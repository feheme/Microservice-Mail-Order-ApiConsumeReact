namespace Inveon.Services.EmailAPI
{
    public class MessageSender
    {
        public int CartHeaderId { get; set; }
        public string UserId { get; set; }
        public double OrderTotal { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public double DiscountTotal { get; set; }
        public string CouponCode { get; set; }
        public DateTime PickupDateTime { get; set; }


    }
}
