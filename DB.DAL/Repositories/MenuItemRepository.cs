using DB.Core.DTO;
using DB.DAL.Entities;
using DB.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DB.DAL.Repositories
{
    /// <summary>
    /// CRUD операции для MenuItem
    /// </summary>
    public class MenuItemRepository : IMenuItemRepository
    {
        #region Fields

        private readonly ApplicationDbContext _applicationDbContext;

        #endregion

        #region Ctor

        public MenuItemRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Создать много блюд
        /// </summary>
        public async Task<int> CreateManyAsync(List<MenuItemDTO> menuItems)
        {
            foreach (var menu in menuItems)
            {
                var menuItemEntity = new MenuItemEntity
                {
                    Article = menu.Article,
                    Name = menu.Name,
                    Price = menu.Price,
                    IsWeighted = menu.IsWeighted,
                    FullPath = menu.FullPath,
                    Barcodes = menu.Barcodes.ToList()
                };

                _applicationDbContext.MenuItem.Add(menuItemEntity);
            }

            return await _applicationDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Получить весь список блюд
        /// </summary>
        public async Task<List<MenuItemDTO>> GetAsync()
        {
            var menuItemsEntities = await _applicationDbContext.MenuItem
                 .AsNoTracking()
                 .ToListAsync();

            return menuItemsEntities.ConvertAll(x => new MenuItemDTO(x.Article, x.Name, x.Price, x.IsWeighted, x.FullPath, x.Barcodes, x.Id));
        }

        #endregion
    }
}
