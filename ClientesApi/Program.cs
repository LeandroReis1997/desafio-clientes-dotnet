using System.Collections.Concurrent;
using System.Net.Mail;

namespace ClientesApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        var clientes = new ConcurrentDictionary<string, Cliente>(StringComparer.OrdinalIgnoreCase);

        app.MapGet("/clientes", () =>
        {
            var lista = clientes.Values.OrderBy(c => c.Nome).ToList();
            return Results.Ok(lista);
        });

        app.MapPost("/clientes", (CreateClienteRequest request) =>
        {
            var erros = Validar(request);
            if (erros.Count > 0)
                return Results.BadRequest(new { message = "Falha de validação", errors = erros });

            var cliente = new Cliente(
                Id: Guid.NewGuid(),
                Nome: request.Nome.Trim(),
                Email: request.Email.Trim()
            );

            if (!clientes.TryAdd(cliente.Email, cliente))
                return Results.Conflict(new { message = "Email já cadastrado.", field = "email" });

            return Results.Created($"/clientes/{cliente.Id}", cliente);
        });

        app.Run();
    }

    static Dictionary<string, string[]> Validar(CreateClienteRequest request)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(request.Nome))
            errors["nome"] = new[] { "Nome é obrigatório." };

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            errors["email"] = new[] { "Email é obrigatório." };
        }
        else
        {
            var email = request.Email.Trim();
            if (!EmailValido(email))
                errors["email"] = new[] { "Email em formato inválido." };
        }

        return errors;
    }

    static bool EmailValido(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return addr.Address.Equals(email, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }
}

public record CreateClienteRequest(string Nome, string Email);
public record Cliente(Guid Id, string Nome, string Email);
