using System.Text.Json.Serialization;

namespace ClassLibrary
{
    /// <summary>
    /// Класс содержит свойства, конструкторы и методы для представления и работы с объектами Course.
    /// </summary>
    public class Aeroexpress
    {
        /// <summary>
        /// Id аэроэкспресса.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id {  get; set; }
        
        /// <summary>
        /// Станция отправления аэроэкспресса.
        /// </summary>
        [JsonPropertyName("stationStart")]
        public string? StationStart {  get; set; }
        
        /// <summary>
        /// Станция прибытия аэроэкспресса.
        /// </summary>
        [JsonPropertyName("stationEnd")]
        public string? StationEnd { get; set;}
        
        /// <summary>
        /// Направление аэроэкспресса.
        /// </summary>
        [JsonPropertyName("line")]
        public string? Line { get; set; }
        
        /// <summary>
        /// Время отправления аэроэкспресса.
        /// </summary>
        [JsonPropertyName("timeStart")]
        public string? TimeStart { get; set; }
        
        /// <summary>
        /// Время прибытия аэроэкспресса.
        /// </summary>
        [JsonPropertyName("timeEnd")]
        public string? TimeEnd { get; set; }
        
        /// <summary>
        /// GlobalId аэроэкспресса.
        /// </summary>
        [JsonPropertyName("globalId")]
        public uint GlobalId { get; set; }

        /// <summary>
        /// Конструктор, создающий объект со всеми данными о курсе.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stationStart"></param>
        /// <param name="stationEnd"></param>
        /// <param name="line"></param>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        /// <param name="globalId"></param>
        public Aeroexpress(int id, string stationStart, string stationEnd, string line, string? timeStart, string? timeEnd, uint globalId)
        {
            Id = id;
            StationStart = stationStart;
            StationEnd = stationEnd;
            Line = line;
            TimeStart = timeStart;
            TimeEnd = timeEnd;
            GlobalId = globalId;
        }
        
        /// <summary>
        /// Конструктор без параметров.
        /// </summary>
        public Aeroexpress() { }
        
        /// <summary>
        /// Метод возвращает данные об объекте в виде строки формата CSV.
        /// </summary>
        /// <returns></returns>
        public string ToCsv()
        {
            return $"\"{Id}\";\"{StationStart}\";\"{Line}\";\"{TimeStart}\";" +
                $"\"{StationEnd}\";\"{TimeEnd}\";\"{GlobalId}\";";
        }
    }
}