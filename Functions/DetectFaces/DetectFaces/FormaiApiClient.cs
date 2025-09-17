using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetectFaces;

public class FormaiApiClient(HttpClient httpClient)
{

    // Marca a foto como redimensionada
    public async Task MarkFacesDetectedAsync(long photoId)
    {
        var response = await httpClient.PostAsync($"{photoId}/mark-faces-detected", null);
        response.EnsureSuccessStatusCode();
    }
}
