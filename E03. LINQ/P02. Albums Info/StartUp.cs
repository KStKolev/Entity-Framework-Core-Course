namespace MusicHub
{
    using System;
    using System.Text;
    using Data;
    using Initializer;
    using MusicHub.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            Console.WriteLine(ExportAlbumsInfo(context, 9));
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
            throw new NotImplementedException();
        }
    }
}
