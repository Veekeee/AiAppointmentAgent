namespace AiAppointmentAgent.Models
{
    public class ClinicRule
    {
        public int Id { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public bool IsSundayClosed { get; set; }
    }
}
