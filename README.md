# MVA - Microsoft Virtual Academy

## Comandi da terminale .NET Core

- ```dotnet --help``` : aiuto
- ```dotnet --version``` : mostra la versione
- ```dotnet new [template] --help``` : mostra la descrizione del template
- ```dotnet new [template] -o [nomeprogetto]``` : assegna il nome del progetto creato (anzichè assegnare quello di default del template)
- ```type [nomecsproj]``` : mostra il il csproj
- ```dotnet watch run``` : esegue il livereload al salvataggio di una nuova modifica

## Configurazione iniziale

### File Startup.cs

**Iniezione dei servizi**
```cs
    public IConfiguration Configuration { get; set; }
    public Startup(IConfiguration config)
    {
        Configuration = config;
    }
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc();
    }
```

**Abilitazione dei servizi iniettati**
```cs
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseMvc();

        app.Run(async (context) =>
        {
            await context.Response.WriteAsync("Hello World!");
        });
    }
```
## Estensioni utili

- vs-icons
- git-lens

## Tecnologie

#### Configurazione file di progetto

Esistono alcuni posti dove salvare le variabili di progetto

- `appsettings.json` dove salvare variabili in chiaro nel progetto
- `appsettings.developer.json` per salvare le variabili utilizzabili in ambiente di sviluppo
- In `impostazioni di progetto > Debug` per salvare le variabili d'ambiente.

E' possibile utilizzare tali variabili richiamando l'interfaccia 
```cs
Microsoft.Extensions.Configuration.IConfiguration
```
### MVC

I controller agganciati alle view hanno una naming convention pensata per gestire le richieste http in un certo modo. Ad esempio, il metodo **OnPostDeleteAsync**:

```cs
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        public IndexModel(AppDbContext db) { _db = db; }
        public IList<Customer> Customers { get; private set; }
        public async Task<IActionResult> OnGetAsync()
        {
            Customers = await _db.Customers.AsNoTracking().ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var customer = await _db.Customers.FindAsync(id);
            if (customer != null)
            {
                _db.Remove(customer);
                await _db.SaveChangesAsync();
            }

            return RedirectToPage();
        }


    }
```

Bisogna osservare 3 elementi che compongono il nome di questo metodo:
- OnPost definisce il tipo di metodo http
- Delete è il nome che abbiamo assegnato arbitrariamente, l'handler che è presente anche nella view in razor (a seguire)
- Async definisce che il metodo è async e quindi, ci si aspetta un `await`

Le view hanno degli helper che consistono in attributi (che iniziano con `asp-`) e agevolano di molto la stesura del codice.

```html
    <form method="post">
        <table class="table">
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Name</th>
                </tr>
            </thead>
            <tbody>
                @foreach (var customer in Model.Customers)
                {
                <tr>
                    <td>@customer.Id</td>
                    <td>@customer.Name</td>
                    <td>
                        <a asp-page="./Edit" asp-route-id="@customer.Id">Edit</a> | 
                        <!-- Qui è presente l'handler delete, quello che richiamava il suddetto metodo "OnPostDeleteAsync" -->
                        <button type="submit"
                        asp-page-handler="delete" asp-route-id="@customer.Id">Delete</button>
                    </td>
                </tr>
                }
            </tbody>
        </table>
        <a asp-page="./Create">Create</a>
    </form>
```
### Authorization

### Tag Helpers

Esiste anche la possibilità di creare, oltre ai tag `asp-`, di creare dei tag personalizzati. Per farlo, occorre estendere la classe `Microsoft.AspNetCore.Razor.TagHelpers.TagHelper`, fare un `override` sul metodo `Process` o `ProcessAsync` e far eseguire le istruzioni desiderate. Di seguito l'esempio:

```cs
    namespace Mymva.CustomTagHelpers
    {
        public class RepeatTagHelper : TagHelper
        {
            public int Count { get; set; }
            
            // Ripete l'elemento sotto il tag <repeat></repeat>
            // NB: TagHelper è il suffisso che non va ripetuto 
            public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
            {
                for (int i = 0; i < Count; i++)
                {
                    output.Content.AppendHtml(await output.GetChildContentAsync(useCachedResult: false));
                }
                    
            }
        }
    }
```  

## Utilità

Per utilizzare il watcher di .NET core, è necessario aggiungere questo elemento al file di progetto.


```xml
  <ItemGroup>
    <DotNetCliToolReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Tools" Version="2.0.2" />
    <DotNetCliToolReference Include="Microsoft.DotNet.Watcher.Tools" version="2.0.0" />
  </ItemGroup>
```
Per eseguirlo è necessario scrivere il comando `$ dotnet watch run`.
## Link utili

- mva.microsoft.com
- docs.microsoft.com

## Approfondimenti
- **Tag Helpers**:  https://docs.microsoft.com/it-it/aspnet/core/mvc/views/tag-helpers/intro
- **.AsNoTracking()**: http://www.c-sharpcorner.com/UploadFile/ff2f08/entity-framework-and-asnotracking/ 
- **Autenticazione di terze parti**: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/
- **Documentazione per le API**: https://docs.microsoft.com/it-it/aspnet/core/tutorials/web-api-help-pages-using-swagger?tabs=visual-studio
- **Middleware**: [middleware.md]("./Middleware.md")
- **View Component**: https://docs.microsoft.com/it-it/aspnet/core/mvc/views/view-components
- **Hosting**: [ https://docs.microsoft.com/it-it/aspnet/core/fundamentals/hosting?tabs=aspnetcore2x ]  [ [Kestrel]("https://docs.microsoft.com/it-it/aspnet/core/fundamentals/servers/?tabs=aspnetcore2x") ]
- **Location e Globalization** : https://docs.microsoft.com/it-it/aspnet/core/fundamentals/localization