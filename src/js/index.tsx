import * as React from "react";
import {render} from "react-dom";
import * as GraphiQL from "graphiql";

declare let graphiQLConfig: {
    useSgConnect: boolean;
    graphQLEndpoint: string;
    query: string;
    defaultQuery: string;
};

function graphQLFetcher(graphQLParams) {
    return fetch(graphiQLConfig.graphQLEndpoint, {
        method: "post",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(graphQLParams),
    }).then((response: any) => response.json());
}

render(
    <GraphiQL fetcher={graphQLFetcher}
              query={graphiQLConfig.query}
              defaultQuery={graphiQLConfig.defaultQuery}
    />,
    document.getElementById("root"));