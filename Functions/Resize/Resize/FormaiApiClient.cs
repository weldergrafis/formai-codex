using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resize
{
    public class FormaiApiClient
    {
        private readonly HttpClient _httpClient;

        public FormaiApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7228/api/photos/");
        }

        // Marca a foto como redimensionada
        public async Task MarkResizedAsync(long photoId)
        {
            var response = await _httpClient.PostAsync($"{photoId}/mark-resized", null);
            response.EnsureSuccessStatusCode();
        }
    }
}
