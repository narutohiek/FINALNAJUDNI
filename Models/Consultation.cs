using System.ComponentModel.DataAnnotations;

namespace SocialWelfarre.Models
{
    public class Consultation
    {
        public int Id { get; set; }
        [Required]
        public string First_Name { get; set; }
        [Required]
        public string Middle_Name { get; set; }
        [Required]
        public string Last_Name { get; set; }
        public string FullName => $"{First_Name} {Middle_Name} {Last_Name}";
        [Required]
        public string Brgy_Cert { get; set; }
        [Required]
        public string Brgy_Cert_Path { get; set; }
        [Required]
        public string Proof_SoloParent { get; set; }
        [Required]
        public string Proof_SoloParent_Path { get; set; }
        [Required]
        public string Birth_Cert { get; set; }
        [Required]
        public string Birth_Cert_Path { get; set; }
        [Required]
        public string Valid_ID { get; set; }
        [Required]
        public string Valid_ID_Path { get; set; }
        [Required]
        public string x1_Pic { get; set; }
        [Required]
        public string x1_Pic_Path { get; set; }
        public DateOnly Consulatation_Date { get; set; }
        public TimeOnly Consultation_Time { get; set; }
        public ActiveStatus2? Consultation_status { get; set; }
    }
    public enum ActiveStatus2
    {
        [Display(Name = "Pending")]
        Pending,
        [Display(Name = "Approved")]
        Approved,
        [Display(Name = "Denied")]
        Denied
    }
}

