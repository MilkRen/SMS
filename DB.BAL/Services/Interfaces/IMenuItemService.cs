using DB.Core.DTO;

namespace DB.BAL.Services.Interfaces
{
    /// <summary>
    /// Сервис для работы с БД MenuItem
    /// </summary>
    public interface IMenuItemService
    {
        /// <summary>
        /// Создать много блюд
        /// </summary>
        Task<int> CreateManyMenuAsync(List<MenuItemDTO> menuItems);

        /// <summary>
        /// Получить весь список блюд
        /// </summary>
        Task<List<MenuItemDTO>> GetMenuAsync();
    }
}