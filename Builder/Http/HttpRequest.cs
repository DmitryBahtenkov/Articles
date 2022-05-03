using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace Builder.Http
{
    public class HttpRequest
    {
        // метод запроса
        public HttpMethod Method { get; set; }
        // url запроса
        public string Url { get; set; }
        // данные из формы
        public Dictionary<string, string> FormData { get; set; } = new();
        // данные application/json
        public string JsonData { get; set; }
        // заголовки запроса
        public Dictionary<string, string> Headers { get; set; } = new();
        // строковое представление параметров запроса 
        public string Query { get; set; }
    }

    public abstract class HttpRequestBuilder
    {
        protected readonly HttpRequest HttpRequest;

        public HttpRequestBuilder()
        {
            HttpRequest = new HttpRequest();
        }

        public abstract void SetMethod();
        public abstract void SetUrl(string url);
        // метод для установки заголовка авторизации
        public abstract void SetAuth(string auth);
        public abstract void SetForm(Dictionary<string, string> data);
        public abstract void SetJson(object data);
        public abstract void SetQuery(string query);
        public abstract void AddHeader(string key, string value);
        public abstract void SetHeaders(Dictionary<string, string> headers);

        public HttpRequest Build()
        {
            return HttpRequest;
        }
    }

    public class GetHttpRequestBuilder : HttpRequestBuilder
    {
        public override void SetMethod()
        {
            HttpRequest.Method = HttpMethod.Get;
        }

        public override void SetUrl(string url)
        {
            HttpRequest.Url = url;
        }

        public override void SetAuth(string auth)
        {
            HttpRequest.Headers["Authorization"] = auth;
        }

        public override void SetForm(Dictionary<string, string> data)
        {
            // кидаем исключение, так как у get-запросов не может быть тела
            throw new Exception("У get-запроса не может быть тела");
        }

        public override void SetJson(object data)
        {
            // кидаем исключение, так как у get-запросов не может быть тела
            throw new Exception("У get-запроса не может быть тела");
        }

        public override void SetQuery(string query)
        {
            HttpRequest.Query = query;
        }

        public override void AddHeader(string key, string value)
        {
            HttpRequest.Headers[key] = value;
        }

        public override void SetHeaders(Dictionary<string, string> headers)
        {
            HttpRequest.Headers = headers;
        }
    }
    
    public class PostHttpRequestBuilder : HttpRequestBuilder
    {
        public override void SetMethod()
        {
            HttpRequest.Method = HttpMethod.Post;
        }

        public override void SetUrl(string url)
        {
            HttpRequest.Url = url;
        }

        public override void SetAuth(string auth)
        {
            HttpRequest.Headers["Authorization"] = auth;
        }

        public override void SetForm(Dictionary<string, string> data)
        {
            HttpRequest.FormData = data;
        }

        public override void SetJson(object data)
        {
            HttpRequest.JsonData = JsonSerializer.Serialize(data);
        }

        public override void SetQuery(string query)
        {
            HttpRequest.Query = query;
        }

        public override void AddHeader(string key, string value)
        {
            HttpRequest.Headers[key] = value;
        }
        
        public override void SetHeaders(Dictionary<string, string> headers)
        {
            HttpRequest.Headers = headers;
        }
    }

    public class HttpClient
    {
        public void SendGet(string url, Dictionary<string, string> headers = null, string query = null)
        {
            var builder = new GetHttpRequestBuilder();
            builder.SetMethod();
            builder.SetUrl(url);
            
            // если строка query не пуста
            if (!string.IsNullOrEmpty(query))
            {
                builder.SetQuery(query);
            }

            // если заголовки переданы
            if (headers is not null)
            {
                builder.SetHeaders(headers);
            }

            var request = builder.Build();
            
            // имитируем отправку http-запроса
            Console.WriteLine($"Запрос отправлен. Метод: {request.Method}. Url: {request.Url + "?" + request.Query}");
        }
        
        public void SendJsonPost(string url, Dictionary<string, string> headers = null, object data = null)
        {
            var builder = new PostHttpRequestBuilder();
            builder.SetMethod();
            builder.SetUrl(url);
            
            // если data не пуста
            if (data is not null)
            {
                builder.SetJson(data);
            }
            
            // если заголовки переданы
            if (headers is not null)
            {
                builder.SetHeaders(headers);
            }

            var request = builder.Build();
            
            // имитируем отправку http-запроса
            Console.WriteLine($"Запрос отправлен. Метод: {request.Method}. Url: {request.Url}. Data: {request.JsonData}");
        }
    }
}