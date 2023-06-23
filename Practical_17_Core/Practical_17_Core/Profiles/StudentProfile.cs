using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Practical_17_Core.Models;
using Practical_17_Core.Repository;


namespace Practical_17_Core.Profiles;

public class StudentProfile : Profile
{
    public StudentProfile()
    {
        CreateMap<Student, Student>();
    
    }
}
