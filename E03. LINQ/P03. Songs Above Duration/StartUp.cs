namespace MusicHub
{
    using System;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using MusicHub.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            /*DbInitializer.ResetDatabase(context);*/
            Console.WriteLine(ExportSongsAboveDuration(context, 180));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Albums
                .Where(a => a.ProducerId == producerId)
                .Select(a => new
                {
                    AlbumName = a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy"),
                    ProducerName = a.Producer.Name,
                    Songs = a.Songs.Select(s => new 
                    {
                        SongName = s.Name,
                        SongPrice = s.Price,
                        WriterName = s.Writer.Name
                    }).OrderByDescending(s => s.SongName)
                    .ThenBy(s => s.WriterName)
                    .ToList(),
                    TotalAlbumPrice = a.Price
                })
                .ToList();

            StringBuilder result =  new StringBuilder();
            foreach (var album in albums.OrderByDescending(a => a.TotalAlbumPrice)) 
            {
                int counter = 1;
                result.AppendLine($"-AlbumName: {album.AlbumName}");
                result.AppendLine($"-ReleaseDate: {album.ReleaseDate}");
                result.AppendLine($"-ProducerName: {album.ProducerName}");
                result.AppendLine($"-Songs:");
                foreach (var song in album.Songs)
                {
                    result.AppendLine($"---#{counter++}");
                    result.AppendLine($"---SongName: {song.SongName}");
                    result.AppendLine($"---Price: {song.SongPrice:F2}");
                    result.AppendLine($"---Writer: {song.WriterName}");
                }
                result.AppendLine($"-AlbumPrice: {album.TotalAlbumPrice:F2}");
            }
            return result.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            StringBuilder result = new();
            var songs = context.Songs
                .Include(s => s.SongPerformers)
                    .ThenInclude(sp => sp.Performer)
                .Include(s => s.Writer)
                .Include(s => s.Album)
                    .ThenInclude(a => a.Producer)
                .ToList()
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new 
                {
                    SongName = s.Name,
                    PerformersFullName = s.SongPerformers
                    .Select(sp => sp.Performer.FirstName + " " + sp.Performer.LastName)
                    .OrderBy(p => p),
                    WriterName = s.Writer.Name,
                    AlbumProducer = s.Album.Producer.Name,
                    Duration = s.Duration.ToString("c")
                })
                .OrderBy(s => s.SongName)
                .ThenBy(s => s.WriterName)
                .ToList();

            int counter = 1;
            foreach (var song in songs)
            {
                result.AppendLine($"-Song #{counter++}");
                result.AppendLine($"---SongName: {song.SongName}");
                result.AppendLine($"---Writer: {song.WriterName}");
                if (song.PerformersFullName.Any())
                {
                    result.AppendLine(string.Join(Environment.NewLine, song.PerformersFullName.Select(p => $"---Performer: {p}")));
                }
                result.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                result.AppendLine($"---Duration: {song.Duration}");
            }

            return result.ToString().TrimEnd();
        }
    }
}
