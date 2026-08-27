using FunBooksAndVideos.Domain.Entities;

namespace FunBooksAndVideos.Domain.Services
{
    public interface IShippingSlipService
    {
        ShippingSlipResult CreateForPhysicalProduct(PurchaseOrder order, Product product);
    }
}
