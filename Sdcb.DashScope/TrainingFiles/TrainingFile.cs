using System.IO;
using System.Text;

namespace Sdcb.DashScope.TrainingFiles;

/// <summary>
/// Represents a training file with a name, optional description, and data stream.
/// </summary>
public class TrainingFile
{
    /// <summary>
    /// The file name of the training file.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The optional description of the training file.
    /// </summary>
    /// <value>Optional. The description of the training file.</value>
    public string? Description { get; init; }

    /// <summary>
    /// The data stream of the training file.
    /// </summary>
    public required Stream Stream { get; init; }

    /// <summary>
    /// Creates a <see cref="TrainingFile"/> object from a file on the local file system.
    /// </summary>
    /// <param name="fileName">The name of the file to create the <see cref="TrainingFile"/> from.</param>
    /// <param name="description">Optional. The description of the training file.</param>
    /// <returns>A new instance of <see cref="TrainingFile"/> with the specified name and stream.</returns>
    public static TrainingFile FromFile(string fileName, string? description = null)
    {
        return new TrainingFile
        {
            Name = Path.GetFileName(fileName),
            Description = description,
            Stream = new MemoryStream(File.ReadAllBytes(fileName)),
        };
    }

    /// <summary>
    /// Creates a <see cref="TrainingFile"/> object from a text content.
    /// </summary>
    /// <param name="fileName">The name for the created training file.</param>
    /// <param name="textContent">The text content to be used in the training file's data stream.</param>
    /// <param name="description">Optional. The description of the training file.</param>
    /// <returns>A new instance of <see cref="TrainingFile"/> with the specified name, text content as a stream, and optional description.</returns>
    public static TrainingFile FromText(string fileName, string textContent, string? description = null)
    {
        return new TrainingFile
        {
            Name = fileName,
            Description = description,
            Stream = new MemoryStream(Encoding.UTF8.GetBytes(textContent)),
        };
    }

    /// <summary>
    /// Creates a <see cref="TrainingFile"/> object from a byte array content.
    /// </summary>
    /// <param name="fileName">The name for the created training file.</param>
    /// <param name="bytesContent">The byte array to be used in the training file's data stream.</param>
    /// <param name="description">Optional. The description of the training file.</param>
    /// <returns>A new instance of <see cref="TrainingFile"/> with the specified name, byte array content as a stream, and optional description.</returns>
    public static TrainingFile FromBytes(string fileName, byte[] bytesContent, string? description = null)
    {
        return new TrainingFile
        {
            Name = fileName,
            Description = description,
            Stream = new MemoryStream(bytesContent),
        };
    }
}