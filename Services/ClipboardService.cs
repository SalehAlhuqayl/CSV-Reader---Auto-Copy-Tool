using System.Windows;

namespace CsvReader.Services
{
    /// <summary>
    /// Service for handling clipboard operations
    /// </summary>
    public class ClipboardService
    {
        /// <summary>
        /// Copy text to Windows clipboard
        /// </summary>
        /// <param name="text">Text to copy</param>
        public void CopyToClipboard(string text)
        {
            try
            {
                Clipboard.SetText(text);
            }
            catch (System.Runtime.InteropServices.ExternalException ex)
            {
                // Clipboard access can fail if another process is using it
                // Log error or retry (for now, we'll just swallow the exception)
                System.Diagnostics.Debug.WriteLine($"Clipboard access failed: {ex.Message}");
            }
        }
    }
}
