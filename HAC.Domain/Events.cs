using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using HAC.Domain.Tools;

namespace HAC.Domain
{
    public class Events
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public int Day { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public DateTime Date { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; }
        public  string Description { get; set; }
        public string URL { get; set; }
        //public string show_news { get; set; }
        //public  string news1 { get; set; }
        //public  string news2 { get; set; }
        //public  int resultid { get; set; }
        //public byte[] photo { get; set; }
        //public  string ImgS { get; set; }
        //public string ImgB { get; set; }
        public string Name { get; set; }
        public string Lname { get; set; }
        //public int mid { get; set; }
        //public  string show_results { get; set; }
        public DateTime udate { get; set; }
        public string EventDate
        {
            get
            {
                return Date.ToString("D",
                CultureInfo.CreateSpecificCulture("en-US"));
            }
            //String.Format("{0} {1}", TextTools.CreateDateSuffix(Date), Month); }
        }
    }
}
