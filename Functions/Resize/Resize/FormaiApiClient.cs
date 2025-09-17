using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resize
{
    public class FormaiApiClient(HttpClient httpClient)
    {

        // Marca a foto como redimensionada
        public async Task MarkResizedAsync(long photoId)
        {
            var response = await httpClient.PostAsync($"{photoId}/mark-resized", null);
            response.EnsureSuccessStatusCode();
        }
    }
}
