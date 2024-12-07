using Microsoft.Extensions.ObjectPool;

var builder = WebApplication.CreateBuilder(args);

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

var rnd = new Random();


//my pizza arrays 

string[] pizzas = {
            "Margherita",
            "Pepperoni",
            "BBQ Chicken",
            "Hawaiian",
            "Veggie",
            "Meat Lovers",
            "Four Cheese",
            "Buffalo Chicken",
            "Mushroom & Spinach",
            "Mediterranean"
        };

// Array of 10 different pizza toppings
string[] toppings = {
            "Extra Cheese",
            "Olives",
            "Jalapeños",
            "Mushrooms",
            "Bacon",
            "Pineapple",
            "Onions",
            "Green Peppers",
            "Sausage",
            "Fresh Basil"
        };

int pizzaArraySize = pizzas.Length;
int toppingsArraySize = toppings.Length;
//for all possible pizza 10x10 combination we created a list 
List<string[]> allPizzaCombination = new List<string[]> { };

//we use two nested loop to create 10x10 pizza types 

for (int i = 0; i < pizzaArraySize; i++)
{
    for (int j = 0; j < toppingsArraySize; j++)
    {
        allPizzaCombination.Add(new string[] { pizzas[i], toppings[j] });
    }
}






app.MapGet("/", () =>
{
    string randomPizza = pizzas[rnd.Next(0, pizzaArraySize)];
    string randomTopping = toppings[rnd.Next(0, toppingsArraySize)];

    //now i create my random piza json object

    var result = new
    {
        Pizza = randomPizza,
        Topping = randomTopping

    };

    // send to front end but since i will send as json i say how i will handle
    return Results.Json(result);

}


);

app.MapGet("/allpizzas", () =>
{

    //here is good part of asp.net core we directly send our list and this will shown as jsom

    return Results.Json(allPizzaCombination);



});

app.Run();
