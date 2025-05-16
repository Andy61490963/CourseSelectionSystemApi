using CourseSelectionSystem.Models;

namespace CourseSelectionSystem.Services;

public interface ICourseService
{
    Task<IEnumerable<CourseModel>> GetAllCoursesAsync(int limit);
    Task<IEnumerable<CourseWithTeacherViewModel>> GetAllCoursesWithTeachersAsync(int limit);
    Task<Guid> CreateCourseAsync(CourseModel course);
    Task<bool> UpdateCourseAsync(Guid id, CourseModel course);
    Task<bool> DeleteCourseAsync(Guid id);
    Task<IEnumerable<StudentsModel>> GetStudentsByCourseAsync(Guid courseId);
}