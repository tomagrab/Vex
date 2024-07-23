namespace Vex.Models
{
    public class TicketModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Branch { get; set; }
        public string Company { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string AssignedTo { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
    }
}