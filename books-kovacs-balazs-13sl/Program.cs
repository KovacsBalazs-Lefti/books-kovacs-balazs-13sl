using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books_kovacs_balazs_13sl
{
    class Program
    {
        static void Main(string[] args)
        {
            Statisztika statisztika = new Statisztika();
            statisztika.LoadAndListBooks();

            // Törlés, új könyv felvitele vagy könyv módosítása
            Console.WriteLine("Válasszon egy műveletet:");
            Console.WriteLine("1. Törlés");
            Console.WriteLine("2. Új könyv felvitele");
            Console.WriteLine("3. Könyv módosítása");
            Console.WriteLine();
            int longBooksCount = statisztika.longestbooks();
            statisztika.oldestbooks();
            statisztika.longestbook();
            statisztika.MostActiveWriter();
            statisztika.BookAddress();


            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    statisztika.DeleteBook();
                    break;
                case "2":
                    Console.WriteLine("Új könyv felvitele");
                    // Bekérjük az új könyv adatait a felhasználótól
                    Console.Write("Cím: ");
                    string title = Console.ReadLine();

                    Console.Write("Szerző: ");
                    string author = Console.ReadLine();

                    Console.Write("Oldalszám: ");
                    int pageCount = int.Parse(Console.ReadLine());

                    Console.Write("Kiadás éve: ");
                    int publishYear = int.Parse(Console.ReadLine());

                    // Hívjuk meg az AddNewBook metódust az új könyv adataival
                    statisztika.AddNewBook(0, title, author, pageCount, publishYear); // Az id-t 0-nak állítjuk be, mivel ezt majd az adatbázis generálja
                    break;
                case "3":
                    Console.WriteLine("Könyv módosítása");
                    statisztika.UpdateBook();
                    break;
            }
        }
    }
}
