using CourseSelectionSystem.Models;

namespace CourseSelectionSystem.Services;

public interface ITeacherService
{
    Task<Guid?> CreateTeacherAsync(TeachersModel teacher);
    Task<IEnumerable<TeachersModel>> GetAllTeachersAsync(int limit);
    Task<IEnumerable<CourseModel>> GetCoursesByTeacherAsync(Guid teacherId, int limit);
    Task<bool> UpdateTeacherAsync(Guid teacherId, TeachersModel updatedTeacher);
    Task<bool> DeleteTeacherAsync(Guid teacherId);
    Task<TeachersModel?> GetTeacherByIdAsync(Guid teacherId);
}