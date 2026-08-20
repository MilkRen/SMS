namespace DB.Core.DTO
{
    /// <summary>
    /// Контаркт для MenuItemEntity
    /// </summary>
    public record MenuItemDTO(
        string Article,
        string Name,
        double Price,
        bool IsWeighted,
        string FullPath,
        List<string> Barcodes,
        int? Id = null);
}
