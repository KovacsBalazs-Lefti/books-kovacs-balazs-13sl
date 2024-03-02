using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace books_kovacs_balazs_13sl
{
    internal class Statisztika
    {
        private List<Book> books;

        public Statisztika()
        {
            books = new List<Book>();
        }

        public void LoadAndListBooks()
        {

            string connectionString = "Server=localhost;Database=books;Uid=root;Pwd=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT id, title, author, page_count, publish_year FROM books";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long id = reader.GetInt64("id");
                            string title = reader.GetString("title");
                            string author = reader.GetString("author");
                            int page_count = reader.GetInt32("page_count");
                            int publish_year = reader.GetInt32("publish_year");

                            Book book = new Book(id, title, author, page_count, publish_year);
                            books.Add(book);

                            // Kiírás azonnal a betöltés során
                            Console.WriteLine($"ID: {book.Id}");
                            Console.WriteLine($"Title: {book.Title}");
                            Console.WriteLine($"Author: {book.Author}");
                            Console.WriteLine($"Page Count: {book.Page_Count}");
                            Console.WriteLine($"Publish Year: {book.Publish_year}");
                            Console.WriteLine();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Hiba a feldolgozás során: " + ex.Message);
                }
            }
        }
        public void AddNewBook(long id, string title, string author, int pageCount, int publishYear)
        {
            string connectionString = "Server=localhost;Database=books;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Ellenőrzés, hogy az adott ID már létezik-e
                    string checkQuery = "SELECT COUNT(*) FROM books WHERE id = @id";
                    using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@id", id);

                        int existingCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (existingCount > 0)
                        {
                            Console.WriteLine("Hiba: Az adott ID már létezik.");
                            return;
                        }
                    }

                    // Ha az ID nem létezik, új könyvet adunk hozzá
                    string query = "INSERT INTO books (id, title, author, page_count, publish_year) VALUES (@id, @title, @author, @page_count, @publish_year)";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@title", title);
                        command.Parameters.AddWithValue("@author", author);
                        command.Parameters.AddWithValue("@page_count", pageCount);
                        command.Parameters.AddWithValue("@publish_year", publishYear);

                        command.ExecuteNonQuery();
                    }
                }

                Console.WriteLine("Az új könyv sikeresen hozzáadva az adatbázishoz.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hiba a könyv hozzáadása során: " + ex.Message);
            }
        }
        public void DeleteBook()
        {
            // Bekérjük a felhasználótól az id-t, amit törölni szeretne
            Console.WriteLine("Kérem, adja meg a törlendő könyv azonosítóját:");
            long idToDelete;
            if (!long.TryParse(Console.ReadLine(), out idToDelete))
            {
                Console.WriteLine("Érvénytelen azonosító formátum.");
                return;
            }

            string connectionString = "Server=localhost;Database=books;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Ellenőrizzük, hogy az adott ID létezik-e
                    string checkQuery = "SELECT COUNT(*) FROM books WHERE id = @id";
                    using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@id", idToDelete);

                        int existingCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (existingCount == 0)
                        {
                            Console.WriteLine("Hiba: Az adott ID nem létezik.");
                            return;
                        }
                    }

                    // Ha az ID létezik, töröljük a könyvet
                    string deleteQuery = "DELETE FROM books WHERE id = @idToDelete";
                    using (MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@idToDelete", idToDelete);
                        deleteCommand.ExecuteNonQuery();
                    }
                }

                Console.WriteLine("A könyv sikeresen törölve lett az adatbázisból.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hiba a könyv törlése során: " + ex.Message);
            }
        }
        public void UpdateBook()
        {
            // Bekérjük a felhasználótól az id-t, amely alapján módosítani szeretné a könyvet
            Console.WriteLine("Kérem, adja meg a módosítani kívánt könyv azonosítóját:");
            long idToUpdate;
            if (!long.TryParse(Console.ReadLine(), out idToUpdate))
            {
                Console.WriteLine("Érvénytelen azonosító formátum.");
                return;
            }

            string connectionString = "Server=localhost;Database=books;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Ellenőrizzük, hogy az adott ID létezik-e
                    string checkQuery = "SELECT COUNT(*) FROM books WHERE id = @id";
                    using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@id", idToUpdate);

                        int existingCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (existingCount == 0)
                        {
                            Console.WriteLine("Hiba: Az adott ID nem létezik.");
                            return;
                        }
                    }

                    // Bekérjük az új adatokat a könyvhöz
                    Console.WriteLine("Kérem, adja meg az új adatokat:");
                    Console.Write("Cím: ");
                    string newTitle = Console.ReadLine();

                    Console.Write("Szerző: ");
                    string newAuthor = Console.ReadLine();

                    Console.Write("Oldalszám: ");
                    int newPageCount = int.Parse(Console.ReadLine());

                    Console.Write("Kiadás éve: ");
                    int newPublishYear = int.Parse(Console.ReadLine());

                    // Az új adatokat tartalmazó parancs összeállítása
                    string updateQuery = "UPDATE books SET title = @title, author = @author, page_count = @page_count, publish_year = @publish_year WHERE id = @id";
                    using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@id", idToUpdate);
                        updateCommand.Parameters.AddWithValue("@title", newTitle);
                        updateCommand.Parameters.AddWithValue("@author", newAuthor);
                        updateCommand.Parameters.AddWithValue("@page_count", newPageCount);
                        updateCommand.Parameters.AddWithValue("@publish_year", newPublishYear);

                        // Módosítás engedélyezése
                        Console.WriteLine("Biztosan szeretné módosítani a könyv adatait? (igen/nem)");
                        string confirmation = Console.ReadLine();

                        if (confirmation.ToLower() == "igen")
                        {
                            updateCommand.ExecuteNonQuery();
                            Console.WriteLine("Köszönjük, hogy módosította az adatokat.");
                        }
                        else if (confirmation.ToLower() == "nem")
                        {
                            Console.WriteLine("A módosítás megszakítva.");
                        }
                        else
                        {
                            Console.WriteLine("Érvénytelen válasz. A módosítás megszakítva.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hiba a könyv módosítása során: " + ex.Message);
            }
        }
        public int longestbooks()
        {
            int count = 0;

            foreach (var book in books)
            {
                if (book.Page_Count > 500)
                {
                    count++;
                }
            }

            Console.WriteLine($"Az 500 oldalnál hosszabb könyvek száma: {count}");
            return count;
        }
        public void oldestbooks()
        {
            foreach (var book in books)
            {
                if (book.Publish_year < 1950)
                {
                    Console.WriteLine("Van 1950-nél régebbi könyv.");
                    return;
                }
            }

            Console.WriteLine("Nincs 1950-nél régebbi könyv.");
        }
        public void longestbook()
        {
            int maxPageCount = 0;
            Book longestBook = null;

            foreach (var book in books)
            {
                if (book.Page_Count > maxPageCount)
                {
                    maxPageCount = book.Page_Count;
                    longestBook = book;
                }
            }

            if (longestBook != null)
            {
                Console.WriteLine($"ID: {longestBook.Id}");
                Console.WriteLine($"Title: {longestBook.Title}");
                Console.WriteLine($"Author: {longestBook.Author}");
                Console.WriteLine($"Page Count: {longestBook.Page_Count}");
                Console.WriteLine($"Publish Year: {longestBook.Publish_year}");
            }
            else
            {
                Console.WriteLine("Nincs könyv a listában.");
            }
        }
        public void MostActiveWriter()
        {
            // Létrehozunk egy szótárat, hogy követni tudjuk, melyik szerző hány könyvet írt
            Dictionary<string, int> authorBookCount = new Dictionary<string, int>();

            // Végigmegyünk a könyveken és összeszámoljuk, hogy melyik szerző hány könyvet írt
            foreach (Book book in books)
            {
                if (!authorBookCount.ContainsKey(book.Author))
                {
                    authorBookCount[book.Author] = 1; // Ha még nem szerepelt a szerző, akkor hozzáadjuk az adatbázishoz
                }
                else
                {
                    authorBookCount[book.Author]++; // Ha már szerepelt a szerző, akkor növeljük a könyvek számát
                }
            }

            // Megkeressük a legtöbb könyvet író szerző nevét és számát
            string mostActiveWriter = "";
            int maxBookCount = 0;

            foreach (var kvp in authorBookCount)
            {
                if (kvp.Value > maxBookCount)
                {
                    maxBookCount = kvp.Value;
                    mostActiveWriter = kvp.Key;
                }
            }

            // Kiírjuk a legtöbb könyvet író szerző nevét
            if (!string.IsNullOrEmpty(mostActiveWriter))
            {
                Console.WriteLine($"A legtöbb könyvet író szerző: {mostActiveWriter}");
            }
            else
            {
                Console.WriteLine("Nincs adat a legtöbb könyvet író szerzőről.");
            }
        }
        public void BookAddress()
        {
            // Bekérjük a felhasználótól a könyv címét
            Console.Write("Kérek egy könyv címet: ");
            string searchedTitle = Console.ReadLine();

            bool found = false;

            // Végigmegyünk a könyveken
            foreach (Book book in books)
            {
                // Ha megtaláltuk a keresett című könyvet
                if (book.Title.Equals(searchedTitle, StringComparison.OrdinalIgnoreCase))
                {
                    // Kiírjuk a könyv szerzőjét
                    Console.WriteLine($"A megadott könyv szerzője: {book.Author}");
                    found = true;
                    break;
                }
            }

            // Ha nem találtunk ilyen című könyvet
            if (!found)
            {
                Console.WriteLine("Nincs ilyen könyv");
            }
        }
    }
}
