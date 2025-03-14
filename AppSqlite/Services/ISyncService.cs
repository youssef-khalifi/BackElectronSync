using AppSqlite.Entities;
using ProductsManage.Dtos;

namespace AppSqlite.Services;

public interface ISyncService
{
    Task<SyncResult> SyncProductsFromSqlite(List<Product> products);
}