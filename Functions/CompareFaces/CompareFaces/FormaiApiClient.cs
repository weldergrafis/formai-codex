using Neurotec.Biometrics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static DetectFaces.Services.NeurotecService;

namespace DetectFaces;

public class FormaiApiClient(HttpClient httpClient)
{

    // Marca a foto como redimensionada
    public async Task MarkFacesDetectedAsync(long photoId)
    {
        var response = await httpClient.PostAsync($"{photoId}/mark-faces-detected", null);
        response.EnsureSuccessStatusCode();
    }

    // DTO do retorno
    public sealed class FaceTemplateDto
    {
        public long Id { get; set; }
        public byte[] Template { get; set; } = System.Array.Empty<byte>();
    }

    // DTO do request
    public sealed class FacesTemplatesRequest
    {
        public long? Start{ get; set; }
        public long? End{ get; set; }
    }

    // Comentário: consome o endpoint POST /api/CompareFaces/get-faces-templates
    public async Task<List<FaceTemplateDto>?> GetFacesTemplatesAsync(FacesTemplatesRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("CompareFaces/get-faces-templates", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<FaceTemplateDto>>();
    }

}
