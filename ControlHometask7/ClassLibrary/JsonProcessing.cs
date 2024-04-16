using System.Text.Json;
using System.Text.Unicode;

namespace ClassLibrary
{
    /// <summary>
    /// Класс используется для чтения и записи коллекции объектов Aeroexpress в формате JSON.
    /// </summary>
    public class JsonProcessing : IProcessable
    {
        /// <summary>
        /// Метод записывает в MemoryStream JSON представление списка объектов Aeroexpress.
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
            var options = new JsonWriterOptions
            {
                Indented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All)
            };
            var stream = new MemoryStream();
            var writer = new Utf8JsonWriter(stream, options);
            JsonSerializer.Serialize(writer, aeroexpresses);
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
            List<Aeroexpress>? aeroexpresses =  JsonSerializer.Deserialize<List<Aeroexpress>>(ms.ToArray());
            if (aeroexpresses is null || aeroexpresses.Count == 0)
            {
                throw new ArgumentException("У данного файла неверный формат");
            }
            return aeroexpresses;
        }
        
        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public JsonProcessing() {}
    }
}