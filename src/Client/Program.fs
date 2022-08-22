open Elmish
open Feliz
open Feliz.UseElmish
open Feliz.DaisyUI
open Fable.Remoting.Client
open Browser.Dom
open Fable.Core.JsInterop
open Shared

let serverApi: IServerApi =
  Remoting.createApi ()
  |> Remoting.withBaseUrl "/api"
  |> Remoting.buildProxy<IServerApi>

module Counter =
  type State = { ServerValue: string; Count: int }

  type Msg =
    | Increment
    | Decrement
    | ResetCount
    | GotServerValue of string

  let init () =
    let cmd = Cmd.OfAsync.perform serverApi.GetValue () GotServerValue
    { ServerValue = "Empty"; Count = 0 }, cmd

  let update (msg: Msg) (state: State) =
    match msg with
    | Increment -> { state with Count = state.Count + 1 }, Cmd.none
    | Decrement -> { state with Count = state.Count - 1 }, Cmd.none
    | ResetCount -> init ()
    | GotServerValue value -> { state with ServerValue = value }, Cmd.none

  [<ReactComponent>]
  let Component () =
    let state, dispatch = React.useElmish (init, update, [||])

    Html.div [
      Html.h1 state.ServerValue
      Daisy.label [ prop.text state.Count ]
      Daisy.button.button [
        button.sm
        button.ghost
        prop.text "Increment"
        prop.onClick (fun _ -> dispatch Increment)
      ]
      Daisy.button.button [
        button.sm
        button.ghost
        prop.text "Decrement"
        prop.onClick (fun _ -> dispatch Decrement)
      ]
      Daisy.button.button [
        button.sm
        button.ghost
        prop.text "Reset count"
        prop.onClick (fun _ -> dispatch ResetCount)
      ]
    ]

importSideEffects "./index.css"
ReactDOM.render (Counter.Component(), document.getElementById "feliz-app")
