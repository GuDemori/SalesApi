using Microsoft.EntityFrameworkCore;
using SalesApi.Data;
using SalesApi.Models;

namespace SalesApi.Endpoints
{
    public static class TransactionEndpoints
    {
        public static void MapTransactionEndpoints(this WebApplication app)
        {
            app.MapPost("/transactions", async (Transaction transaction, AppDbContext db) =>
            {
                var seller = await db.Sellers.FirstOrDefaultAsync(s => s.Id == transaction.SellerId && s.IsActive);

                if (seller is null)
                    return Results.BadRequest("Vendedor inválido ou inativo.");

                if (transaction.TotalAmount <= 0)
                    return Results.BadRequest("O valor da transação deve ser maior que zero.");

                transaction.TransactionDate = DateTime.UtcNow;

                db.Transactions.Add(transaction);
                await db.SaveChangesAsync();
                return Results.Created($"/transactions/{transaction.Id}", transaction);
            });

            app.MapGet("/transactions", async (AppDbContext db) =>
                await db.Transactions
                        .Include(t => t.Seller)
                        .Where(t => t.Seller != null && t.Seller.IsActive)
                        .ToListAsync());
        }
    }
}
