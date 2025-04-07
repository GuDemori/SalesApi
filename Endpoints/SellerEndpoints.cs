using Microsoft.EntityFrameworkCore;
using SalesApi.Data;
using SalesApi.Models;

namespace SalesApi.Endpoints
{
    public static class SellerEndpoints
    {
        public static void MapSellerEndpoints(this WebApplication app)
        {
            app.MapPost("/sellers", async (Seller seller, AppDbContext db) =>
            {
                if (string.IsNullOrWhiteSpace(seller.Name) || string.IsNullOrWhiteSpace(seller.Email))
                    return Results.BadRequest("Nome e e-mail são obrigatórios.");

                if (!IsValidEmail(seller.Email))
                    return Results.BadRequest("E-mail inválido.");

                seller.IsActive = true;
                seller.CreatedAt = DateTime.UtcNow;
                seller.UpdatedAt = DateTime.UtcNow;

                db.Sellers.Add(seller);
                await db.SaveChangesAsync();
                return Results.Created($"/sellers/{seller.Id}", seller);
            });

            app.MapGet("/sellers", async (AppDbContext db) =>
                await db.Sellers.Where(s => s.IsActive).ToListAsync());

            app.MapGet("/sellers/{id}", async (Guid id, AppDbContext db) =>
            {
                var seller = await db.Sellers.FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
                return seller is null ? Results.NotFound("Vendedor não encontrado.") : Results.Ok(seller);
            });

            app.MapPut("/sellers/{id}", async (Guid id, Seller input, AppDbContext db) =>
            {
                var seller = await db.Sellers.FindAsync(id);
                if (seller is null || !seller.IsActive)
                    return Results.NotFound("Vendedor não encontrado.");

                if (string.IsNullOrWhiteSpace(input.Name) || string.IsNullOrWhiteSpace(input.Email))
                    return Results.BadRequest("Nome e e-mail são obrigatórios.");

                if (!IsValidEmail(input.Email))
                    return Results.BadRequest("E-mail inválido.");

                seller.Name = input.Name;
                seller.Email = input.Email;
                seller.UpdatedAt = DateTime.UtcNow;

                await db.SaveChangesAsync();
                return Results.Ok(seller);
            });

            app.MapDelete("/sellers/{id}", async (Guid id, AppDbContext db) =>
            {
                var seller = await db.Sellers.FindAsync(id);
                if (seller is null || !seller.IsActive)
                    return Results.NotFound("Vendedor não encontrado.");

                seller.IsActive = false;
                seller.UpdatedAt = DateTime.UtcNow;
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            static bool IsValidEmail(string email)
            {
                try
                {
                    var addr = new System.Net.Mail.MailAddress(email);
                    return addr.Address == email;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
