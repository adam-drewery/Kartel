provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "example" {
  name     = "example-resources"
  location = "West Europe"
}

resource "azurerm_container_group" "example" {
  name                = "example-containers"
  location            = azurerm_resource_group.example.location
  resource_group_name = azurerm_resource_group.example.name
  ip_address_type     = "public"
  dns_name_label      = "acitestcontainers"
  os_type             = "Linux"

  container {
    name   = "api"
    image  = "myacr.azurecr.io/myimage:v1"  // Replace with your ACR and image details
    cpu    = "0.5"
    memory = "1.5"

    ports {
      port     = 6840
      protocol = "TCP"
    }

    ports {
      port     = 6841
      protocol = "TCP"
    }
  }
}