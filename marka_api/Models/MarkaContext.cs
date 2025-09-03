namespace marka_api.Models
{
    public class MarkaContext
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public DateTime created_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public Guid? deleted_by { get; set; }
        public bool is_deleted { get; set; }
        public Guid customer_id { get; set; }

    }
}
