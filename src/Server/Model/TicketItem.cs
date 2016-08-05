using System;

namespace Server.Model
{
    public class TicketItem
    {
        public string Prefix { get; set; }          // строковый префикс элемента (П или Э)
        public uint NumberElement { get; set; }     // номер в очереди на момент добавления
        public ushort CountElement { get; set; }    // кол-во клиентов в очереди на момент добавления
        public DateTime AddedTime{ get; set; }      // дата добавления
        public int? Сashbox { get; set; }           // номер кассира
        public byte CountTryHandling { get; set; }  // количество попыток обработки этого билета кассиром

        public override string ToString()
        {
            return $" Дата поступления в обработку: {AddedTime}       Номер билет: {NumberElement}{Prefix}     Номер кассира:   {  Сashbox?.ToString() ?? "неизвестный кассир" } ";
        }
    }
}