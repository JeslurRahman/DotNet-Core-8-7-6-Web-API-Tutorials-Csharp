﻿namespace CollegeApp.Data.Repository
{
    //Here we have added all the database or high level operations here

    public interface IStudentRepository
    {
        Task<List<Student>> GetAllAsync();

        Task<Student> GetByIdAsync(int id, bool useNoTracking = false);

        Task<Student> GetByNameAsync(string name);

        Task<int> CreateAsync(Student student);

        Task<int> UpdateAsync(Student student);

        //int UpdateParcialAsync(Student student);

        Task<bool> DeleteAsync(Student student);
    }
}