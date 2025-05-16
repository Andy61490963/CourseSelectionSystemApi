namespace CourseSelectionSystem.Models;

public class CourseModel
{
    /// <summary>
    /// 課程的唯一識別碼
    /// </summary>
    public Guid id_ { get; set; }

    /// <summary>
    /// 課程名稱
    /// </summary>
    public string name_ { get; set; }

    /// <summary>
    /// 課程描述，說明課程內容或目標
    /// </summary>
    public string description_ { get; set; }

    /// <summary>
    /// 課程開始時間
    /// </summary>
    public DateTime start_time_ { get; set; }

    /// <summary>
    /// 課程結束時間
    /// </summary>
    public DateTime end_time_ { get; set; }

    /// <summary>
    /// 授課教師的識別碼
    /// </summary>
    public Guid teacher_id_ { get; set; }

    /// <summary>
    /// 資料建立時間，用於記錄這筆課程資料是何時建立的
    /// </summary>
    public DateTime create_time_ { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public bool delete_ { get; set; }
}