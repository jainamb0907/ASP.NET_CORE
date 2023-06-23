using AutoMapper;
using Practical18.Model;

namespace Practical18.Mapper
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<Student, StudentViewModel>().ReverseMap();
            CreateMap<StudentViewModel, Student>().ReverseMap();
        }
    }
}
