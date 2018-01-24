# View Components

> Gli step necessari per utilizzare le view components all'interno del progetto.

Inserire il tag helper all'interno del file `_ViewImports.cshtml`, così come nell'esempio successivo:

```cshtml
@using Web
@using Web.Models
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
<!-- Web è il nome dell'assembly dove è contenuto il taghelper -->
@addTagHelper *, Web
```

La convenzione suggerisce di utilizzare la seguente struttura ad albero, suddividendo:
- i viewcomponents limitati ad una specifica view;
- i viewcomponents condivisi con diverse view;

![](https://i2.wp.com/tahirnaushad.com/wp-content/uploads/2017/08/view-components-sln.png?resize=200%2C187&ssl=1)

È inoltre possibile richiamare una viewcomponent mediante il metodo `ViewComponent()`.

## Letture utili

- https://codewala.net/2017/03/09/exploring-asp-net-core-view-component/
- https://tahirnaushad.com/2017/08/24/asp-net-core-2-0-mvc-view-components/