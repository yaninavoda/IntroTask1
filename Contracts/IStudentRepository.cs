﻿using IntroTask.Entities;

namespace Contracts;

public interface IStudentRepository
{
    IEnumerable<Student> GetAllStudents(bool trackChanges);
}
