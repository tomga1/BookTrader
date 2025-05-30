using BookTrader.DTOs;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.SqlServer.Query.Internal.SqlServerOpenJsonExpression;

namespace BookTrader.Services
{
    public class GoogleBookService
    {
        private readonly HttpClient _httpClient;

        public GoogleBookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<InsertLibroDTO> BuscarLibroPorISBN(string isbn)
        {
            var response = await _httpClient.GetAsync($"https://www.googleapis.com/books/v1/volumes?q=isbn:{isbn}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine(json);
                var data = JsonDocument.Parse(json);
                // Verificar si existe "items" y que no esté vacío
                if (data.RootElement.TryGetProperty("items", out var items) && items.GetArrayLength() > 0)
                {
                    var item = items[0];
                    var volumeInfo = item.GetProperty("volumeInfo"); // Corregido el nombre aquí
                    return new InsertLibroDTO
                    {
                        Nombre = volumeInfo.GetProperty("title").GetString(),
                        Autor = volumeInfo.TryGetProperty("authors", out var authors) && authors.GetArrayLength() > 0
                            ? authors[0].GetString()
                            : "",
                        
                        cantPaginas = volumeInfo.TryGetProperty("pageCount", out var pages) 
    ? pages.GetInt32()
    : 0,
                        Editorial = volumeInfo.TryGetProperty("publisher", out var pub) ? pub.GetString() : "",
                        MasInfo = volumeInfo.TryGetProperty("description", out var desc) ? desc.GetString() : "",
                        ImagenUrl = volumeInfo.TryGetProperty("imageLinks", out var img) && img.TryGetProperty("thumbnail", out var thumb)
                            ? thumb.GetString()

                            : ""

                    };
                    
                }
            }

            return null;
        }

    }
}
