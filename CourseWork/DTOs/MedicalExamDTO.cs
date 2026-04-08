using System.ComponentModel.DataAnnotations;
using CourseWork.Models;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.DTOs;

public class MedicalExamDTO
{
    public int Id { get; set; }
    
    public int AnimalId{get;set;}
    
    public string? AnimalName{get;set;}
    
    public DateTime ExamDate{get;set;}
    
    public decimal Temperature{get;set;}

    public decimal Weight{get;set;}
    
    public string? Notes{get;set;}
}