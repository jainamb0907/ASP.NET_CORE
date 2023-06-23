using System;
using AutoMapper;
using Practical18.Data;
using Practical18.Model;

namespace Practical18.Repository
{
    public class StudentRepository : IstudentRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public StudentRepository(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IEnumerable<Student> AddStudent(StudentViewModel request)
        {
            if (request != null)
            {
                var student = _mapper.Map<Student>(request);
                _context.Students.Add(student);
                _context.SaveChanges();
            }
            return GetAllStudents();
        }

        public Student DeleteStudent(int? id)
        {
            var student = _context.Students.FirstOrDefault(x => x.StudentId == id && x.IsDelete == 0);
            if (student != null)
            {
                student.IsDelete = 1;
                _context.SaveChanges();
            }
            return student!;
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return _context.Students.Where(x => x.IsDelete == 0).ToList();
        }

        public Student GetSingleStudent(int? id)
        {
            var student = _context.Students.FirstOrDefault(x => x.StudentId == id && x.IsDelete == 0);
            return student!;
        }

        public Student UpdateStudent(int? id, StudentViewModel student)
        {
            var studentUpdate = _context.Students.FirstOrDefault(x => x.StudentId == id && x.IsDelete == 0);
            if (studentUpdate != null)
            {
                studentUpdate.StudentName = student.StudentName;
                studentUpdate.Email = student.Email;
                studentUpdate.PhoneNumber = student.PhoneNumber;
                studentUpdate.Dob = student.Dob;
                _context.SaveChanges();
            }
            return studentUpdate!;
        }
    }
}
