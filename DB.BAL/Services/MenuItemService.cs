using DB.BAL.Services.Interfaces;
using DB.Core.DTO;
using DB.Core.Enums;
using DB.DAL.Repositories.Interfaces;
using Serilog;
using Sms.Test;

namespace DB.BAL.Services
{
    /// <summary>
    /// Сервис для работы с БД MenuItem
    /// </summary>
    public class MenuItemService : IMenuItemService
    {
        #region Fields

        private readonly IMenuItemRepository _menuItemRepository;

        #endregion

        #region Ctor

        public MenuItemService(IMenuItemRepository menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Создать много блюд
        /// </summary>
        public async Task<int> CreateManyMenuAsync(List<MenuItemDTO> menuItems)
        {
            try
            {
                return await _menuItemRepository.CreateManyAsync(menuItems);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, $"Ошибка в БД - {nameof(MenuItemService)} - {nameof(CreateManyMenuAsync)}");
                return (int)ErrorDb.Fatal;
            }
        }

        /// <summary>
        /// Получить весь список блюд
        /// </summary>
        public async Task<List<MenuItemDTO>> GetMenuAsync()
        {
            try
            {
                return await _menuItemRepository.GetAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, $"Ошибка в БД - {nameof(MenuItemService)} - {nameof(GetMenuAsync)}");
                return [];
            }
        }

        #endregion
    }
}
