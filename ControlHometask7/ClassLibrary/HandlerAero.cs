namespace ClassLibrary
{
    /// <summary>
    /// Класс производит фильтрацию и сортировку списка объектов типа Aeroexpress.
    /// </summary>
    public static class HandlerAero
    {
        /// <summary>
        /// Метод производит сортировку aeroexpresses в порядке возрастания.
        /// Если flag, сортировка происходит по полю TimeStart, иначе по TimeEnd.
        /// </summary>
        /// <param name="aeroexpresses"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static List<Aeroexpress> Sort(List<Aeroexpress> aeroexpresses, bool flag)
        {
            return flag ? aeroexpresses.OrderBy(a => a.TimeStart).ToList() : aeroexpresses.OrderBy(a => a.TimeEnd).ToList();
        }
        
        /// <summary>
        /// Метод производит фильтрацию списка Aeroexpress по полю StationStart или StationEnd.
        /// Если выборка пуста, метод выбрасывает ArgumentException.
        /// </summary>
        /// <param name="aeroexpresses"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="strStart"></param>
        /// <param name="strEnd"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static List<Aeroexpress> Filter(List<Aeroexpress> aeroexpresses, bool start, bool end, string strStart, string strEnd)
        {
            List<Aeroexpress> aero = new();
            if (start) {aero = aeroexpresses.Where(a => a.StationStart == strStart).ToList(); }
            if (end) {aero = aeroexpresses.Where(a => a.StationEnd == strEnd).ToList(); }
            if (aero.Count == 0)
            {
                throw new ArgumentException("Выборка пуста, введите другое значение");
            }
            return aero;
        }
    }
}