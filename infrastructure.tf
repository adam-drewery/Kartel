provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "kartel" {
  name     = "kartel-${var.environment}"
  location = "West Europe"
}

resource "azurerm_container_group" "kartel" {
  name                = "kartel-${var.environment}"
  location            = azurerm_resource_group.kartel.location
  resource_group_name = azurerm_resource_group.kartel.name
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
  
  container {
    name   = "web"
    image  = "myacr.azurecr.io/myimage:v1"  // Replace with your ACR and image details
    cpu    = "0.5"
    memory = "1.5"

    ports {
      port     = 80
      protocol = "TCP"
    }

    ports {
      port     = 8080
      protocol = "TCP"
    }
  }
}