module Elements.Contacts

open Elmish
open Fable.React
open Feliz
open Theme.Color
open Theme.Font
open Shared

let playerIcon (imgSrc: string) =
    let circleDimension = length.px 35
    Html.div [
        prop.style [
            style.height circleDimension
            style.width circleDimension
            // style.borderRadius circleDimension
            // style.borderRadius (length.px 1)
            // style.backgroundColor Color.Light.custom3
            style.display.flex
            style.alignItems.center
            style.color Color.custom2
            style.custom("justifyContent","center")
        ]
        prop.children [
            Html.img [
                prop.src imgSrc
                prop.style [
                    style.height circleDimension
                    style.width circleDimension
                    style.borderRadius circleDimension
                    // style.borderRadius (length.px 1)
                ]
            ]
            // Html.i [
            //     prop.className "fa fa-user"
            // ]
        ]
    ]

let leftNavigationButton contact onClick =
    let paddingVert = 10
    let paddingHorizontal = 10
    Html.div [
        prop.className "left-navigation-button"
        prop.onClick onClick
        prop.style [
            style.paddingTop paddingVert
            style.paddingBottom paddingVert
            style.paddingRight paddingHorizontal
            style.paddingLeft paddingHorizontal
            style.color Color.Light.custom3
            style.display.flex
            style.custom("justifyContent","space-between")
            style.flexGrow 1
            style.fontFamily Font.First.regular
            style.fontSize (length.em 1.2)
            style.alignItems.center
        ]
        prop.children [
            str (contact.Name.ToUpper())
            playerIcon contact.ImgUrl
        ]
    ]

let contactsSection (key: string) (contact: Contact) onClick =
    let paddingHorizontal = 2
    Html.div [
        prop.className "left-navigation-section"
        prop.key key
        prop.style [
            style.paddingRight paddingHorizontal
            style.paddingLeft paddingHorizontal
            style.paddingBottom 2
            style.display.flex
            style.alignContent.center
            style.alignItems.center
        ]
        prop.children [ leftNavigationButton contact onClick ]
    ]

// let contactSection (availableContacts: Contact list) onClick =
//     availableContacts
//     |> List.mapi (
//         fun i c -> leftNavigationSection (sprintf "left-nav-contact-%i" i) c onClick
//     )