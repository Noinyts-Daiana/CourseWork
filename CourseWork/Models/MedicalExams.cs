using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CourseWork.Models;

[Table("medical_exams")]
public class MedicalExams
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [Column("animal_id")]
    public int AnimalId { get; set; }
    
    [Required]
    [Column("exam_date")]
    public DateTime ExamDate { get; set; }
    
    [Required]
    [Column("temperature", TypeName = "timestamp with time zone")]
    public decimal Temperature { get; set; }
    
    [Required]
    [Column("weight")]
    public decimal Weight { get; set; }
    
    [Column("notes")]
    public string? Notes { get; set; }
    
}