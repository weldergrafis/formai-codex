using Microsoft.Identity.Client;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resize.Helpers
{
    public static class ImageHelper
    {
        // Comentário: Redimensiona mantendo proporção, lado maior = targetMaxSide
        public static Image Resize(Image source, int targetMaxSide)
        {
            var (targetWidth, targetHeight) = GetTargetSize(source.Width, source.Height, targetMaxSide);

            return source.Clone(ctx =>
                {
                    ctx.Resize(new ResizeOptions
                    {
                        Size = new Size(targetWidth, targetHeight),
                        Mode = ResizeMode.Stretch,
                        Sampler = KnownResamplers.Lanczos3,
                        //Sampler = KnownResamplers.CatmullRom,
                        Compand = true,
                    });

                    //ctx.GaussianSharpen(0.2f);
                }
            );
        }

        // Comentário: redimensiona mantendo proporção e garante que o lado MAIOR = targetMaxSide
        static (int targetWidth, int targetHeight) GetTargetSize(int sourceWidth, int sourceHeight, int targetMaxSide)
        {
            if (sourceWidth >= sourceHeight)
            {
                var ratio = (double)targetMaxSide / sourceWidth;
                return (targetMaxSide, (int)Math.Round(sourceHeight * ratio));
            }
            else
            {
                var ratio = (double)targetMaxSide / sourceHeight;
                return ((int)Math.Round(sourceWidth * ratio), targetMaxSide);
            }
        }

        // Comentário: converte a Image para um Stream (JPEG)
        public static async Task<Stream> ToStreamAsync(Image image, int quality = 85)
        {
            var ms = new MemoryStream();
            var encoder = new JpegEncoder
            {
                Quality = quality,
                SkipMetadata = true,
            };

            await image.SaveAsJpegAsync(ms, encoder);
            ms.Position = 0; // volta para o início para leitura
            return ms;
        }
    }
}
