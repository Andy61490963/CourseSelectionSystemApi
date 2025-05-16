using CourseSelectionSystem.Models;

namespace CourseSelectionSystem.Services;

public interface ILoginService
{
    Task<StudentsModel?> ValidateStudentLoginAsync(string email, string password);
    Task<TeachersModel?> ValidateTeacherLoginAsync(string email, string password);

}