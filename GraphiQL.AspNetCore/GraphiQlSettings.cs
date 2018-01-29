namespace GraphiQL.AspNetCore
{
    /// <summary>
    /// Configuration setting for GraphiQL
    /// </summary>
    public class GraphiQLSettings
    {
        /// <summary>
        /// The GraphQL endpoint to be used by GraphiQL
        /// </summary>
        public string GraphQLEndpoint { get; set; }
        /// <summary>
        /// The path used for the GraphiQL web page
        /// </summary>
        public string GraphiQLPath { get; set; } = "/graphql";
        /// <summary>
        /// The title of the GraphiQL web page
        /// </summary>
        public string PageTitle { get; set; } = "GraphiQL";

        public string FaviconPath { get; set; } = "favicon.ico";
        /// <summary>
        /// An optional GraphQL string to use as the initial displayed query, if not provided, the stored query or defaultQuery will be used.
        /// </summary>
        public string Query { get; set; }
        /// <summary>
        /// An optional GraphQL string to use when no query is provided and no stored query exists from a previous session. If not provided, GraphiQL will use its own default query.
        /// </summary>
        public string DefaultQuery { get; set; }
    }
}