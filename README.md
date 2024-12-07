# ASP.NET Core and JavaScript Pizza Tutorial

This tutorial demonstrates how to create a simple **ASP.NET Core Web API** with a **JavaScript front end** that fetches and displays random pizza combinations and all possible pizza options.

## Backend: ASP.NET Core Empty Template

### Features

1. Generates a random pizza-topping combination.
2. Provides a list of all possible pizza-topping combinations (10x10 = 100 combinations).
3. Built using an **ASP.NET Core Empty Template**.

### Code Breakdown

1. **CORS Policy**: Configured to allow all origins, methods, and headers.
2. **Endpoints**:
   - `/` : Returns a random pizza combination.
   - `/allpizzas` : Returns all possible combinations of pizzas and toppings in JSON format.

### Backend Code

```csharp
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

// Pizza arrays
string[] pizzas = {
    "Margherita", "Pepperoni", "BBQ Chicken", "Hawaiian", "Veggie",
    "Meat Lovers", "Four Cheese", "Buffalo Chicken", "Mushroom & Spinach", "Mediterranean"
};

string[] toppings = {
    "Extra Cheese", "Olives", "Jalapeños", "Mushrooms", "Bacon",
    "Pineapple", "Onions", "Green Peppers", "Sausage", "Fresh Basil"
};

int pizzaArraySize = pizzas.Length;
int toppingsArraySize = toppings.Length;

// Create all possible combinations
List<string[]> allPizzaCombination = new List<string[]>();

for (int i = 0; i < pizzaArraySize; i++)
{
    for (int j = 0; j < toppingsArraySize; j++)
    {
        allPizzaCombination.Add(new string[] { pizzas[i], toppings[j] });
    }
}

// Endpoint for random pizza
app.MapGet("/", () =>
{
    string randomPizza = pizzas[rnd.Next(0, pizzaArraySize)];
    string randomTopping = toppings[rnd.Next(0, toppingsArraySize)];

    var result = new { Pizza = randomPizza, Topping = randomTopping };

    return Results.Json(result);
});

// Endpoint for all pizzas
app.MapGet("/allpizzas", () =>
{
    return Results.Json(allPizzaCombination);
});

app.Run();
```

---

## Frontend: HTML + JavaScript

### Features

1. Fetches and displays a random pizza from the `/` endpoint.
2. Fetches and displays all possible pizza combinations from `/allpizzas`.

### Frontend Code

```html
<!DOCTYPE html>
<html>
  <head>
    <title>Random Pizza</title>
  </head>

  <body>
    <h1>Random Pizza Shower</h1>
    <hr />
    <button type="button" id="getrandompizza" onclick="getRandomPizza()">
      Get Random Pizza
    </button>
    <div id="randompizzacontainer">
      <ul>
        <li id="pizzashower"></li>
      </ul>
    </div>

    <button type="button" id="getallpizzabutton" onclick="getAllPizza()">
      Get All Pizzas
    </button>
    <div id="allpizzas">
      <ul id="allpizzalist"></ul>
    </div>

    <script>
      async function getRandomPizza() {
        fetch("http://localhost:5298/", { method: "GET" })
          .then((response) => {
            if (!response.ok)
              throw new Error("Error fetching data: " + response.statusText);
            return response.json();
          })
          .then((data) => {
            const pizzaShower = document.getElementById("pizzashower");
            pizzaShower.innerHTML = `Pizza: ${data.pizza}<br>Topping: ${data.topping}`;
          })
          .catch((error) => console.error("Error:", error));
      }

      async function getAllPizza() {
        document.getElementById("allpizzalist").innerHTML = "";
        fetch("http://localhost:5298/allpizzas", { method: "GET" })
          .then((response) => {
            if (!response.ok)
              throw new Error("Error fetching data: " + response.statusText);
            return response.json();
          })
          .then((data) => {
            const allPizzaList = document.getElementById("allpizzalist");
            data.forEach((element, index) => {
              const listItem = document.createElement("li");
              listItem.textContent = `${index + 1}. Pizza: ${
                element[0]
              }, Topping: ${element[1]}`;
              listItem.setAttribute("draggable", true);
              listItem.addEventListener("dragstart", (event) => {
                event.dataTransfer.setData("text/plain", listItem.textContent);
              });
              allPizzaList.appendChild(listItem);
            });
          })
          .catch((error) => console.error("Error:", error));
      }
    </script>
  </body>
</html>
```

---

## Running the Application

### Backend

1. Open the ASP.NET Core project in Visual Studio or Visual Studio Code.
2. Run the application. By default, it runs on `http://localhost:5298/`.

### Frontend

1. Save the frontend HTML file in a directory.
2. Open the file in a browser.
3. Use the **Get Random Pizza** button to fetch a random pizza.
4. Use the **Get All Pizzas** button to view all pizza combinations.

---

## Expected Results

### `/` Endpoint

Returns a random pizza-topping combination, e.g.:

```json
{
  "Pizza": "Margherita",
  "Topping": "Extra Cheese"
}
```

### `/allpizzas` Endpoint

Returns all combinations, e.g.:

```json
[
    ["Margherita", "Extra Cheese"],
    ["Margherita", "Olives"],
    ["Pepperoni", "Jalapeños"],
    ...
]
```

---

## Conclusion

This tutorial provides a foundation for creating a simple ASP.NET Core API with a JavaScript frontend. Extend this application with additional features like user input, database storage, or advanced drag-and-drop functionality.
