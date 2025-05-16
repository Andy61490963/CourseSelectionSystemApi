namespace CourseSelectionSystem.Models;

public class StudentsModel
{
    /// <summary>
    /// 使用者唯一識別碼（GUID）
    /// </summary>
    public Guid id_ { get; set; }

    /// <summary>
    /// 使用者姓名。
    /// </summary>
    public string name_ { get; set; }

    /// <summary>
    /// 使用者電子郵件（帳號）
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
}