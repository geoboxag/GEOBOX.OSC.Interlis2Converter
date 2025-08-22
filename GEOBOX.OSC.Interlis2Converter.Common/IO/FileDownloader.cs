namespace GEOBOX.OSC.Interlis2Converter.Common.IO
{
    /// <summary>
    /// Downloads file from url
    /// </summary>
    internal class FileDownloader : IDisposable
    {
        private HttpClient httpClient;
        internal event EventHandler<double> DownloadProgressChanged;

        /// <summary>
        /// File Donwloader
        /// </summary>
        internal FileDownloader()
        {
            httpClient = new HttpClient();
        }

        /// <summary>
        /// Downloads a file from the specified URL or uses the default Swiss topo URL
        /// </summary>
        /// <param name="outputPath">Path where the file should be saved</param>
        /// <param name="url">URL to download from (defaults to Swiss topo data)</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>FileInfo object of the downloaded file</returns>
        internal async Task<FileInfo> DownloadFileAsync(
            string outputPath,
            string url,
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var response = await httpClient.GetAsync(
                    url,
                    HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken);

                response.EnsureSuccessStatusCode();

                var totalBytes = response.Content.Headers.ContentLength ?? -1L;
                var buffer = new byte[8192];
                var bytesRead = 0L;

                using var contentStream = await response.Content.ReadAsStreamAsync();

                using var fileStream = new FileStream(outputPath , FileMode.Create, FileAccess.Write, FileShare.None);

                while (true)
                {
                    var read = await contentStream.ReadAsync(buffer, cancellationToken);
                    if (read == 0)
                        break;

                    await fileStream.WriteAsync(buffer.AsMemory(0, read), cancellationToken);

                    bytesRead += read;

                    if (totalBytes != -1L)
                    {
                        var progress = (double)bytesRead / totalBytes * 100;
                        DownloadProgressChanged?.Invoke(this, progress);
                    }
                }

                return new FileInfo(outputPath);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to download file from {url}: {ex.Message}", ex);
            }
            catch (IOException ex)
            {
                throw new Exception($"Failed to save file to {outputPath}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets the filename from the URL or generates a timestamp-based name
        /// </summary>
        /// <param name="url">URL to extract filename from</param>
        /// <returns>Suggested filename</returns>
        internal string GetSuggestedFilename(string url)
        {
            try
            {
                var uri = new Uri(url);
                var filename = Path.GetFileName(uri.LocalPath);
                return string.IsNullOrEmpty(filename)
                    ? $"download_{DateTime.Now:yyyyMMddHHmmss}.zip"
                    : filename;
            }
            catch
            {
                return $"download_{DateTime.Now:yyyyMMddHHmmss}.zip";
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects)
                    httpClient.Dispose();
                }

                // free unmanaged resources (unmanaged objects) and override finalizer
                // set large fields to null
                disposedValue = true;
            }
        }

        // // override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~FileDownloader()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}