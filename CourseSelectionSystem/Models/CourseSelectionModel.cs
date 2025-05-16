namespace CourseSelectionSystem.Models;

public class CourseSelectionModel
{
    /// <summary>
    /// 課程的唯一識別碼
    /// </summary>
    public Guid id_ { get; set; }

    /// <summary>
    /// 學生唯一識別碼
    /// </summary>
    public Guid student_id_ { get; set; }
    
    /// <summary>
    /// 課程唯一識別碼
    /// </summary>
    public Guid course_id_ { get; set; }
    
    /// <summary>
    /// 選擇時間
    /// </summary>
    public DateTime select_time_ { get; set; }
}