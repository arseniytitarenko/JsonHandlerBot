namespace ClassLibrary
{
    /// <summary>
    /// Перечисление содержит уровни на которых может находиться пользователь.
    /// </summary>
    public enum Level
    {
        Start,
        Send,
        Choice,
        Sort,
        Filter,
        GetFirstFilter,
        GetSecondFilter,
        Get
    }

    /// <summary>
    /// Перечисление содержит типы файлов которые может отправить пользователь.
    /// </summary>
    public enum TypeFile
    {
        Csv,
        Json
    }

    /// <summary>
    /// Перечисление содержит поля по которым может происходить фильтрация.
    /// </summary>
    public enum TypeFilter
    {
        StationStart,
        StationEnd,
        StartEnd
    }
    
    /// <summary>
    /// Класс содержит информацию о пользователе.
    /// </summary>
    public class UserData
    {        
        /// <summary>
        /// Список объектов Aeroexpress, с которым будет работать программа.
        /// </summary>
        public List<Aeroexpress> Aeroexpresses { get; set; }     
        
        /// <summary>
        /// Уровень, на котором находится пользователь.
        /// </summary>
        public Level LevelType { get; set; }
        
        /// <summary>
        /// Тип файла, который пользователь отправит боту.
        /// </summary>
        public TypeFile FileType { get; set; }
        
        /// <summary>
        /// Поле по которому может происходить фильтрация.
        /// </summary>
        public TypeFilter FilterType { get; set; }
        
        /// <summary>
        /// Конструктор без параметров задаёт значения по усолчанию.
        /// </summary>
        public UserData()
        {
            LevelType = Level.Start;
            FileType = TypeFile.Csv;
            FilterType = TypeFilter.StationStart;
            Aeroexpresses = new List<Aeroexpress>();
        }
    }
}