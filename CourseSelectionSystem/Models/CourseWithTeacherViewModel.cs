namespace CourseSelectionSystem.Models;

public class CourseWithTeacherViewModel
{
    public Guid id_ { get; set; }
    public string name_ { get; set; }
    public string description_ { get; set; }
    public DateTime start_time_ { get; set; }
    public DateTime end_time_ { get; set; }
    public Guid teacher_id_ { get; set; }
    public DateTime create_time_ { get; set; }

    public TeacherBriefViewModel teacher { get; set; } = new();
}

public class TeacherBriefViewModel
{
    public Guid id_ { get; set; }
    public string name_ { get; set; }
    public string email_ { get; set; }
}