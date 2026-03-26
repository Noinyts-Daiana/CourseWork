namespace CourseWork.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("animal_characteristics")]
public class AnimalCharacteristic
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [Column("animal_id")]
    public int AnimalId { get; set; }
    
    [Required]
    [Column("characteristic_id")]
    public int CharacteristicId { get; set; }
}