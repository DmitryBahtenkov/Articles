using System;
using System.Collections.Generic;
using Builder.Examples;
using Builder.Game;
using Builder.Http;

namespace Builder
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var http = new HttpClient();
            http.SendGet("example.com", query: "variable=12");
            http.SendJsonPost("example.com", data:new Data(){Property = "Data"});
        }
    }

    class Data
    {
        public string Property { get; set; }
    }
}