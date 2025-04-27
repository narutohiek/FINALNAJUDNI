using System.ComponentModel.DataAnnotations;

namespace SocialWelfarre.Models
{
    public class Consultation
    {
        public int Id { get; set; }
  
        public string First_Name { get; set; }
     
        public string Middle_Name { get; set; }
   
        public string Last_Name { get; set; }
        public string FullName => $"{First_Name} {Middle_Name} {Last_Name}";

        public string Brgy_Cert { get; set; }
     
        public string Brgy_Cert_Path { get; set; }

        public string Proof_SoloParent { get; set; }
    
        public string Proof_SoloParent_Path { get; set; }
   
        public string Birth_Cert { get; set; }

        public string Birth_Cert_Path { get; set; }
 
        public string Valid_ID { get; set; }

        public string Valid_ID_Path { get; set; }

        public string x1_Pic { get; set; }

        public string x1_Pic_Path { get; set; }
        public DateOnly Consultation_Date { get; set; }
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

