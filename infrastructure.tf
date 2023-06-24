terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=3.0.0"
    }
  }
  backend "azurerm" {
    resource_group_name  = "terraform"
    storage_account_name = "kartelterraform"
    container_name       = "tfstate"
    key                  = "terraform.tfstate"
  }
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "kartel" {
  name     = "kartel-${var.environment}"
  location = "West Europe"
}

resource "azurerm_container_registry" "kartel" {
  name                     = "kartel"
  resource_group_name      = azurerm_resource_group.kartel.name
  location                 = azurerm_resource_group.kartel.location
  sku                      = "Basic"
  admin_enabled            = true
  
}

resource "azurerm_container_group" "kartel" {
  name                = "kartel-${var.environment}"
  location            = azurerm_resource_group.kartel.location
  resource_group_name = azurerm_resource_group.kartel.name
  ip_address_type     = "Public"
  dns_name_label      = "acitestcontainers"
  os_type             = "Linux"

  container {
    name   = "api"
    image  = "nginx:latest"
    cpu    = "0.2"
    memory = "0.2"

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
    image  = "nginx:latest"
    cpu    = "0.2"
    memory = "0.2"

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

output "acr_username" { 
  value = azurerm_container_registry.kartel.admin_username 
}

output "acr_password" { 
  value = azurerm_container_registry.kartel.admin_password
  sensitive = true
}