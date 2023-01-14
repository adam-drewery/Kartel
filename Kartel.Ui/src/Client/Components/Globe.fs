module Components.Globe

open Fable.Core
// open Fable.Core.JS
open Fable.Core.JsInterop
open Browser.Types
open Fable.Import.D3
open System
open Fable.Import.D3.Selection
open Fetch

importAll "./../../../node_modules/d3-fetch/src/index.js"
importAll "./../../../node_modules/d3-geo/src/index.js"

let topojson : TopoJson = import "*" "./../../../node_modules/topojson-client/src/index.js"

[<Emit("this")>]
let unsafeJsThis : string = jsNative

type Quaternion = float[] // unit quaternion for the given Euler rotation angles [λ, φ, γ]
type Angles = float[] // rotation angles [λ, φ, γ]
type Spherical = float[] // spherical coordinates [λ, φ]
type Cartesian = float[] // Cartesian coordinates [x, y, z]

type IVersor =
    abstract cartesian: Spherical -> Cartesian
    abstract rotation: Quaternion -> Angles
    abstract delta: Cartesian -> Cartesian -> Quaternion
    abstract multiply: Quaternion -> Quaternion -> Quaternion // TODO: Not sure about this?
    abstract rotate: unit -> float[]
    abstract rotate: int[] -> float[]
    abstract scale: float -> float[]
    abstract translate: float[] -> float[]
    abstract invert: float -> float -> float[]

[<ImportDefault("./../../../node_modules/versor/src/versor.js")>]
let versor : IVersor = jsNative

[<ImportDefault("./../../../node_modules/versor/src/versor.js")>]
let versorFn (v: Angles) : Quaternion = jsNative

// DO STUFF
let renderView (world) (cList) : unit =
    // Configuration
    let rotationDelay = 3000. // ms to wait after dragging before auto-rotating
    let scaleFactor = 0.9 // scale of the globe (not the canvas element)
    let degPerSec = 6. // autorotation speed
    let angles = {|x = -20; y = 40; z = 0; |}

    // colors
    let colorWater = "#fff"
    let colorLand = "#111"
    let colorGraticule = "#ccc"
    let colorCountry = "#a00"

    // Variables
    let mutable current : Selection<obj,obj,HTMLElement,obj option> = d3.select("#current")
    let mutable canvas : Selection<obj,obj,HTMLElement,obj option> = d3.select("#globe")
    let canvasNode : HTMLCanvasElement = canvas?node()
    let context : CanvasRenderingContext2D = canvasNode?getContext("2d")
    let water = createObj [ "type" ==> "Sphere" ]
    let projection = d3.geoOrthographic().precision(0.1)
    let graticule = d3.geoGraticule10()

    let path = d3?geoPath(projection)?context(context)
    let mutable lastTime = DateTime.Now
    let degPerMs = degPerSec / 1000.

    let mutable width = 0.0
    let mutable height = 0.0
    let mutable theLand : obj = Object()
    let mutable countries : Countries = unbox List.empty
    let mutable countryList : Feature[] = Array.empty
    let mutable currentCountry : Feature =
        let coord : Coordinate = [|0.;0.|]
        let coords : Coordinate[] = [|coord|]
        let geometry : Geometry = unbox [| coords |]
        let c : Feature =  unbox {|id = 1; name = ""; geometry = geometry |}
        c.geometry <-  geometry
        c.name <- "craig"
        c.id <- 113
        c
    let mutable versorEmpty = Array.empty
    let mutable v0, q0, r0 = versorEmpty,versorEmpty,versorEmpty
    let mutable autorotate : Autorotate = unbox ""
    let vec3dToIntArray (v3d: float[]) = [|v3d.[0];v3d.[1];v3d.[2]|]

    let stroke obj color =
      context.beginPath()
      path(obj) |> ignore
      context.strokeStyle <- color
      context.stroke()

    let fill obj color =
      context.beginPath()
      path(obj) |> ignore
      context.fillStyle <- color
      context.fill()

    let render() =
      context.clearRect (0.0, 0.0, width, height)
      fill water (U3.Case1 colorWater)
      stroke graticule (U3.Case1 colorGraticule)
      fill theLand (U3.Case1 colorLand)
      if not (isNullOrUndefined currentCountry)
        then fill currentCountry (U3.Case1 colorCountry)
        else ()

    let rotate elapsed =
      let now = DateTime.Now
      let diff = now - lastTime
      if (diff < elapsed)
      then
        let rotation : float[] = projection?rotate()
        rotation.[0] <- rotation.[0] + (float diff.Milliseconds * degPerMs)
        projection?rotate(rotation) |> ignore
        render() |> ignore
      else ()
      lastTime <- now

    let inline scale (ev: 'a) =
      width <- Browser.Dom.document.documentElement.clientWidth
      height <- Browser.Dom.document.documentElement.clientHeight
      canvas.attr("width", width).attr("height", height) |> ignore
      projection
        ?scale((scaleFactor * System.Math.Min(width, height)) / 2.)
        ?translate([| (width / 2.); (height / 2.) |]) |> ignore
      render()

    // Functions

    let startRotation delay =
      autorotate.restart rotate delay

    let stopRotation() =
      autorotate.stop()

    let dragstarted (ev: Event) : unit =
      v0 <- versor.cartesian(projection?invert(d3.mouse(unbox unsafeJsThis)))
      r0 <- projection?rotate()
      q0 <- versorFn(r0)
    //   stopRotation()

    let dragged (ev: Event) : unit =
      let v1 : float[] = versor.cartesian(projection?rotate(r0)?invert(d3.mouse(unbox unsafeJsThis)))
      let delt = versor.delta v0 v1
      let q1 = versor.multiply q0 delt
      let r1 = versor.rotation(q1)
      projection?rotate(r1 |> vec3dToIntArray) |> ignore
      render()

    let dragended (ev: Event) : unit  =
      startRotation(rotationDelay)

    // Initialization

    let setAngles() =
      let rotation : int[]= projection?rotate()
      rotation.[0] <- angles.y
      rotation.[1] <- angles.x
      rotation.[2] <- angles.z
      projection?rotate(rotation)

    // Handler

    let enter (country: Feature) =
        let country =
            countryList |> Array.find (
                fun c ->
                    c.id = country.id
            )
        let countryAndNameExist =
            match (not (isNullOrUndefined country)) with
            | true -> not (isNullOrUndefined (country.name))
            | _ -> false
        current.text(if countryAndNameExist then country.name else "")

    let leave country =
      current.text("")

    // https://github.com/d3/d3-polygon
    let polygonContains (polygon: Selection.Coordinate[]) (point: float[]) =
      let n = polygon |> Array.length
      let p = polygon.[n - 1]
      let x, y = point.[0], point.[1]
      let mutable x0, y0 = p.[0], p.[1]
      let mutable x1, y1 = 0,0
      let mutable inside = false
      for p in polygon do
        let x1 = p.[0]
        let y1 = p.[1]
        if (((y1 > y) <> (y0 > y)) && (x < (x0 - x1) * (y - y1) / (y0 - y1) + x1))
            then inside <- not inside
            else ()
        x0 <- x1
        y0 <- y1
      inside

    let inline getCountry(event) =
        let pos : float[] =
            projection?invert(d3.mouse(event))
            |> (fun (vec3d: float[]) -> [| vec3d.[0];vec3d.[1];vec3d.[2] |])
        countries.features
        |> Array.tryFind (
            fun f ->
                f.geometry.coordinates
                |> Array.exists (
                    fun c1 ->
                        polygonContains c1 pos
                        // ||
                        // c1 |> Array.exists(fun c2 -> polygonContains(c2, pos)) // TODO: Understand what the hell this was for?
                )
        )
        |> fun maybeCountry ->
            match maybeCountry with
            | Some c -> c
            | None -> unbox ""

    let inline mousemove (ev: Event) : unit =
        let c = getCountry(unbox unsafeJsThis)
        let jsOptional j = if (isNullOrUndefined (box j)) then None else Some j

        jsOptional c
        |> Option.map (fun c -> currentCountry <- c)
        |> ignore

        render()
        enter(c) |> ignore

    setAngles() |> ignore

    let emptyResize = new ResizeArray<obj option>()

    let buildMap =
            Browser.Dom.console.log("printWorld") // TODO: Get rid
            Browser.Dom.console.log(world)
            Browser.Dom.console.log("printcList")
            Browser.Dom.console.log(cList)
            theLand <- topojson?feature(world, world?objects?``land``)
            Browser.Dom.console.log("did topojson assign 1")
            Browser.Dom.console.log(theLand)
            countries <- topojson?feature(world, world?objects?countries)
            Browser.Dom.console.log("did topojson assign 2")
            Browser.Dom.console.log(countries)
            countryList <- unbox cList
            Browser.Dom.console.log(countryList)
            //   let scale = ignore
            Browser.Dom.window.addEventListener("resize", scale)
            scale()
            autorotate <- d3?timer(rotate)
    // ) |> ignore

    canvas
        ?call(
            d3.drag()
                ?on("start", dragstarted)
                ?on("drag", dragged)
                ?on("end", dragended)
        )
        ?on("mousemove", mousemove)
        |> ignore

    Browser.Dom.console.log(canvas)

    render()
