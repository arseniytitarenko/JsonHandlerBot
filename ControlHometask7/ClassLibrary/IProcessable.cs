using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    /// <summary>
    /// Интерфейс для чтения и записи коллекции объектов Aeroexpress.
    /// </summary>
    public interface IProcessable
    {
        /// <summary>
        /// Метод принимает на вход коллекцию объектов типа Aeroexpress и возвращает объект типа Stream.
        /// </summary>
        /// <param name="aeroexpresses"></param>
        /// <returns></returns>
        public MemoryStream Write(List<Aeroexpress> aeroexpresses);

        /// <summary>
        /// Метод принимает на вход Stream и возвращает коллекцию объектов типа Aeroexpress.
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public List<Aeroexpress> Read(MemoryStream ms);
    }
}
