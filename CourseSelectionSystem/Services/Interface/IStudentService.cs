using CourseSelectionSystem.Models;

namespace CourseSelectionSystem.Services;

public interface IStudentService
{
    Task<Guid?> CreateStudentAsync(StudentsModel student);
    Task<IEnumerable<StudentsModel>> GetAllStudentsAsync(int limit);
    Task<StudentsModel?> GetStudentByIdAsync(Guid studentId);
    Task<IEnumerable<CourseModel>> GetSelectedCoursesAsync(Guid studentId, int limit);
    Task<bool> UpdateCourseSelectionsAsync(Guid studentId, List<Guid> courseIds);
}