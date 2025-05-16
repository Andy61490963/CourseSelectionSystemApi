namespace CourseSelectionSystem.Models;

public class TeachersModel
{
    /// <summary>
    /// 使用者唯一識別碼
    /// </summary>
    public Guid id_ { get; set; }

    /// <summary>
    /// 使用者姓名
    /// </summary>
    public string name_ { get; set; }

    /// <summary>
    /// 使用者電子郵件
    /// </summary>
    public string email_ { get; set; }

    /// <summary>
    /// 使用者密碼
    /// </summary>
    public string password_ { get; set; }

    /// <summary>
    /// 資料建立時間
    /// </summary>
    public DateTime create_time_ { get; set; }

    /// <summary>
    /// 最後一次上課時間
    /// </summary>
    public DateTime last_course_time_ { get; set; }

    /// <summary>
    /// 是否已刪除
    /// </summary>
    public bool delete_ { get; set; }
}