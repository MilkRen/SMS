namespace DB.DAL.Entities
{
    /// <summary>
    /// Сущность заказа с блюдами
    /// </summary>
    public class OrderEntity
    {
        public int Id { get; set; }

        public List<OrderItemEnity> OrderItems { get; set; } = new();
    }
}
