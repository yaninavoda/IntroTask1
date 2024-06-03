using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class TeacherCourseService : ITeacherCourseService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public TeacherCourseService(
            IRepositoryManager repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task AppointTeacherForCourse(int teacherId, int courseId, bool trackChanges)
        {
            var teacher = await _repository.Teacher.GetTeacherByIdAsync(teacherId, trackChanges)
            ?? throw new TeacherNotFoundException(teacherId);

            var course = await _repository.Course.GetCourseByIdAsync(courseId, trackChanges)
            ?? throw new CourseNotFoundException(courseId);

            course.TeacherId = teacherId;
        }
    }
}
