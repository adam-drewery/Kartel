module Elements.BottomNavigation

open Fable
open Fable.Core
open Feliz
open Theme.Color

module CollapseButton =

    let view onClick =
        CollapseButton.view "fa fa-arrow-up" CollapseButton.CollapseButtonPosition.BottomLeft onClick