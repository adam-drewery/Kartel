module Elements.LeftNavigation

open Fable
open Fable.Core
open Feliz
open Theme.Color

module CollapseButton =
    let view onClick =
        CollapseButton.view "fa fa-arrow-left" CollapseButton.CollapseButtonPosition.TopLeft onClick