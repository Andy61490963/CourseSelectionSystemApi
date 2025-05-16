using System.ComponentModel.DataAnnotations;

namespace CourseSelectionSystem.Enum;

public enum UserTypeFlag
{
    [Display( Name = "教師", Description = "教師" )]
    Teacher = 1,

    [Display( Name = "學生", Description = "學生" )]
    Student = 2,
}
