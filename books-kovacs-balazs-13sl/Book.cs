using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace books_kovacs_balazs_13sl
{
    internal class Book
    {
        public Book(long id, string title, string author, int page_Count, int publish_year)
        {
            Id = id;
            Title = title;
            Author = author;
            Page_Count = page_Count;
            Publish_year = publish_year;
           
        }


        public long Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
       
        public int Page_Count {  get; set; }
        public int Publish_year {  get; set; }
       


    }
}
