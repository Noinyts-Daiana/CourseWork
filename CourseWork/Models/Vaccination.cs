using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseWork.Models;

[Table("vaccinations")]
public class Vaccination
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [Column("animal_id")]
    public int AnimalId { get; set; }
    
    [Required]
    [Column("vaccine_name")]
    public string? VaccineName { get; set; }
    
    [Required]
    [Column("date_administered", TypeName = "timestamp with time zone")]
    public DateTime DateAdministered { get; set; }
    
    [Required]
    [Column("next_due_date", TypeName = "timestamp with time zone")]
    public DateTime NextDueDate { get; set; }
    
    [ForeignKey("AnimalId")]
    public Animal? Animal { get; set; }
    
}