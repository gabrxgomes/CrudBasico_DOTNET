using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

/*app.MapGet("/", () => "Hello World!");*/
app.MapGet("/user", () => "Usuario 1");
app.MapPost("/user", () => "Usuario 2");

app.MapGet("/", () => new {Name =  "Testnomejson", Age = 35});

/*app.MapGet("/AddHeader", (HttpResponse response) => { 
    response.Headers.Append("Teste", "Nome test");
    return "Retornou um valor";
    
});*/




app.MapGet("/AddHeader", (HttpResponse response) => { 
    response.Headers.Append("Teste", "Nome test");
    return new {Name = "Nome Test", Age = 35};
    
});



app.MapPost("/saveproduct", (Product product) => { //passando pelo body
	//return product.Code + " - " + product.Name;
	ProductRepository.Add(product);
});

/*app.MapGet("/getproduct", ([FromQuery] string dateStart, [FromQuery] string dateEnd) => {
	*/


app.MapGet("/getproduct/{code}", ([FromRoute] string code) => {
	//return code;
	var product = ProductRepository.GetBy(code);
	return product;
});



app.MapGet("/getproduct", (HttpRequest request) => {
	return request.Headers["product-code"].ToString();
});


app.MapPut("/editproduct", (Product product) => { //aqui vamos alterar apenas o nome do produto pois o seu codigo se assemelha ao id um identificador unico
	var productSaved = ProductRepository.GetBy(product.Code);
	productSaved.Name = product.Name;//atualiza o nome
	//ele pega o nome que vem pelo body como parametro e o atualiza
});

app.MapDelete("/deleteproduct/{code}", ([FromRoute] string code) => {
	var productSaved = ProductRepository.GetBy(code);
	ProductRepository.Remove(productSaved);
});


app.Run();






//criar uma classe para servir de banco de dados

public static class ProductRepository {
	public static List<Product> Products { get; set; }

	public static void Add(Product product) {
		if(Products == null)
			Products = new List<Product>();
		Products.Add(product);
	}


	public static Product GetBy(string code) {
		return Products.FirstOrDefault(p => p.Code == code);//retorna os dados default caso seja nulo
	}


	public static void Remove(Product product) {
		Products.Remove(product);
	}
}

public class Product {
	public string Code { get; set; }
	public string Name { get; set; }

}