namespace TimeInABottle.Core.Contracts.Services;
/// <summary>
/// Defines the contract for a buffer service.
/// </summary>
public interface IBufferService
{
    /// <summary>
    /// Gets the size of the buffer.
    /// </summary>
    public int BufferSize { get; }

    /// <summary>
    /// Loads BufferSize with data.
    /// </summary>
    public void LoadBuffer();
}
