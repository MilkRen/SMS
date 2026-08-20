using DB.Core.DTO;

namespace DB.DAL.Repositories.Interfaces
{
    /// <summary>
    /// CRUD операции для MenuItemEntity
    /// </summary>
    public interface IMenuItemRepository
    {
        /// <summary>
        /// Создать много блюд
        /// </summary>
        Task<int> CreateManyAsync(List<MenuItemDTO> menuItems);

        /// <summary>
        /// Получить весь список блюд
        /// </summary>
        Task<List<MenuItemDTO>> GetAsync();
    }
}