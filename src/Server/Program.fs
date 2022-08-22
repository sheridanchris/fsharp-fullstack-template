open Saturn
open Giraffe
open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Shared
open Microsoft.FSharp.Control

let helloWorld () = async { return "Hello From Saturn!" }

let serverApi: IServerApi = { GetValue = helloWorld }

let routeBuilder (typeName: string) (methodName: string) =
  sprintf "/api/%s/%s" typeName methodName

let serverApiHandler: HttpHandler =
  Remoting.createApi ()
  |> Remoting.withRouteBuilder routeBuilder
  |> Remoting.fromValue serverApi
  |> Remoting.buildHttpHandler

let app = application {
  use_router serverApiHandler
  memory_cache
  use_static "public"
  use_gzip
  url "http://*:8888"
}

run app
