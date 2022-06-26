using ContosoPizza.Models;
using ContosoPizza.Data;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Services;

public class PizzaService
{
    private readonly PizzaContext context;

    public PizzaService(PizzaContext context) => this.context = context;

    public IEnumerable<Pizza> GetAll()
    {
        return this.context.Pizzas
            .AsNoTracking()
            .ToList();
    }

    public Pizza? GetById(int id)
    {
        return this.context.Pizzas
            .Include(p => p.Toppings)
            .Include(p => p.Sauce)
            .AsNoTracking()
            .SingleOrDefault(p => p.Id == id);
    }

    public Pizza Create(Pizza pizza)
    {
        this.context.Pizzas.Add(pizza);
        this.context.SaveChanges();

        return pizza;
    }

    public void AddTopping(int pizzaId, int toppingId)
    {
        var pizzaToUpdate = this.context.Pizzas.Find(pizzaId);
        var toppingToAdd = this.context.Toppings.Find(toppingId);

        if (pizzaToUpdate is null || toppingToAdd is null)
        {
            throw new InvalidOperationException("Pizza or topping does not exist");
        }

        if(pizzaToUpdate.Toppings is null)
        {
            pizzaToUpdate.Toppings = new List<Topping>();
        }

        pizzaToUpdate.Toppings.Add(toppingToAdd);

        this.context.SaveChanges();
    }

    public void UpdateSauce(int pizzaId, int sauceId)
    {
        var pizzaToUpdate = this.context.Pizzas.Find(pizzaId);
        var sauceToUpdate = this.context.Sauces.Find(sauceId);

        if (pizzaToUpdate is null || sauceToUpdate is null)
        {
            throw new InvalidOperationException("Pizza or sauce does not exist");
        }

        pizzaToUpdate.Sauce = sauceToUpdate;

        this.context.SaveChanges();
    }

    public void DeleteById(int id)
    {
        var pizzaToDelete = this.context.Pizzas.Find(id);
        if (pizzaToDelete is not null)
        {
            this.context.Pizzas.Remove(pizzaToDelete);
            this.context.SaveChanges();
        }        
    }
}