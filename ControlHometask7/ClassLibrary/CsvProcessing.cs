using System.Text;
using System.Text.RegularExpressions;

namespace ClassLibrary
{
    
    
    /// <summary>
    /// Класс используется для чтения и записи коллекции объектов Aeroexpress в формате CSV. 
    /// </summary>
    public class CsvProcessing : IProcessable
    {
        /// <summary>
        /// Первая строка файла CSV.
        /// </summary>
        private readonly string _firstString = "\"ID\";\"StationStart\";\"Line\";\"TimeStart\";" +
            "\"StationEnd\";\"TimeEnd\";\"global_id\";";
        
        /// <summary>
        /// Вторая строка файла CSV.
        /// </summary>
        private readonly string _secondString = "\"Локальный идентификатор\";\"Станция отправления\";\"Направление Аэроэкспресс\";" +
                                               "\"Время отправления со станции\";\"Конечная станция направления Аэроэкспресс\";" +
                                               "\"Время прибытия на конечную станцию направления Аэроэкспресс\";\"global_id\";";
        
        /// <summary>
        /// Метод записывает в MemoryStream CSV представление списка объектов Aeroexpress.
        /// Если список пуст, метод выбрасывает ArgumentException.
        /// </summary>
        /// <param name="aeroexpresses"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public MemoryStream Write(List<Aeroexpress> aeroexpresses)
        {
            if (aeroexpresses is null || aeroexpresses.Count == 0)
            {
                throw new ArgumentException("Выборка пуста");
            }
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.WriteLine(_firstString);
            writer.WriteLine(_secondString);
            foreach (var aero in aeroexpresses)
            {
                writer.WriteLine(aero.ToCsv());
            }
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        
        /// <summary>
        /// Метод получает из потока список объектов Aeroexpress.
        /// Если формат файла не соответсвует варианту, метод выбрасывает ArgumentException.
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public List<Aeroexpress> Read(MemoryStream ms)
        {
            List<Aeroexpress> aeroexpresses = new();
            string content = Encoding.UTF8.GetString(ms.ToArray());
            string[] lines = Regex.Split(content, "\r\n|\r|\n");
            if (lines.Length <= 2 || lines[0] != _firstString || lines[1] != _secondString) 
                throw new ArgumentException("У данного файла неверный формат");
            for (int i = 2; i < lines.Length - 1; i++)
            {
                string[] elements = lines[i].Split(';');
                bool first = int.TryParse(elements[0].Trim('\"'), out int id);
                bool second = uint.TryParse(elements[6].Trim('\"'), out uint globalId);
                if (elements.Length != 8 || !first || !second)
                    throw new ArgumentException("У данного файла неверный формат");
                string stationStart = elements[1].Trim('\"');
                string line = elements[2].Trim('\"');
                string timeStart = elements[3].Trim('\"');
                string stationEnd = elements[4].Trim('\"');
                string timeEnd = elements[5].Trim('\"');
                
                Aeroexpress aeroexpress = new(id, stationStart, stationEnd, line, timeStart, timeEnd, globalId);
                aeroexpresses.Add(aeroexpress);
            }
            return aeroexpresses;
        }
        
        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public CsvProcessing() {}
    }
}