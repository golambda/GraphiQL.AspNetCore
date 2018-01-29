GraphiQL
========

A graphical interactive in-browser GraphQL IDE. [Try the live demo](http://graphql.org/swapi-graphql).

## Quick Start

To install from Nuget enter the following into the Package Manager Console

```
install-package GraphiQl.AspNetCore
```

Add the GraphiQL middleware in Startup.cs like this

```
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    app.UseGraphiQl();
}
```

Run the application and navigate to <your-domain>/graphql


## Settings

**DefaultQuery:** An optional GraphQL string to use when no query is provided and no stored query exists from a previous session. If not provided, GraphiQL will use its own default query.

**Query:** An optional GraphQL string to use as the initial displayed query, if not provided, the stored query or defaultQuery will be used.

**GraphiQlPath:** The path used for the graphiql web page.

**GraphQlEndpoint:** The graphql endpoint to be used by GraphiQL.

**PageTitle:** The title of the GraphiQL web page.

### Example

```
app.UseGraphiQl(new GraphiQlSettings
{
    DefaultQuery = "{myObject{value}}",
    Query = "{myObject{value}}",
    GraphiQlPath = "api/graphql",
    GraphQlEndpoint = "graphql-endpoint",
    PageTitle = "My GraphiQL Page"
});
```
