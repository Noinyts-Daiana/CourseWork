namespace CourseWork.DTOs;

public class VaccinationDto
{
    public int Id { get; set; }
    public string? VaccineName { get; set; }
    public DateTime DateAdministered { get; set; }
    public DateTime NextDueDate { get; set; }
    public int AnimalId { get; set; }
    public string? AnimalName { get; set; }
}