namespace DB.DAL.Entities
{
    /// <summary>
    /// Сущность одного блюда в заказе
    /// </summary>
    public class OrderItemEnity
    {
        public int Id { get; set; }

        public int IdMenuItem { get; set; }

        public MenuItemEntity MenuItem { get; set; } = null!;

        public double Quantity { get; set; }

        public int IdOrder { get; set; }

        public OrderEntity Order { get; set; } = null!;
    }
}
