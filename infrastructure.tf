terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=3.62.1"
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
  skip_provider_registration = "true"
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

resource "azurerm_log_analytics_workspace" "example" {
  name                = "acctest-01"
  location            = azurerm_resource_group.kartel.location
  resource_group_name = azurerm_resource_group.kartel.name
  sku                 = "PerGB2018"
  retention_in_days   = 30
}

variable "default_image" { default = "mcr.microsoft.com/azuredocs/containerapps-helloworld:latest" }
variable "default_cpu" { default = 0.25 }
variable "default_memory" { default = "0.5Gi" }

resource "azurerm_container_app_environment" "kartel" {
  name                       = "kartel"
  location                   = azurerm_resource_group.kartel.location
  resource_group_name        = azurerm_resource_group.kartel.name
  log_analytics_workspace_id = azurerm_log_analytics_workspace.example.id
}

resource "azurerm_container_app" "web" {
  name                         = "web"
  container_app_environment_id = azurerm_container_app_environment.kartel.id
  resource_group_name          = azurerm_resource_group.kartel.name
  revision_mode                = "Single"

  template {
    container {
      name   = "web"
      image  = var.default_image
      cpu    = var.default_cpu
      memory = var.default_memory
    }
  }
}

resource "azurerm_container_app" "api" {
  name                         = "api"
  container_app_environment_id = azurerm_container_app_environment.kartel.id
  resource_group_name          = azurerm_resource_group.kartel.name
  revision_mode                = "Single"

  template {
    container {
      name   = "api"
      image  = var.default_image
      cpu    = var.default_cpu
      memory = var.default_memory
    }
  }
}

resource "azurerm_container_app" "locale" {
  name                         = "locale"
  container_app_environment_id = azurerm_container_app_environment.kartel.id
  resource_group_name          = azurerm_resource_group.kartel.name
  revision_mode                = "Single"

  template {
    container {
      name   = "locale"
      image  = var.default_image
      cpu    = var.default_cpu
      memory = var.default_memory
    }
  }
}

resource "azurerm_container_app" "geocoding" {
  name                         = "geocoding"
  container_app_environment_id = azurerm_container_app_environment.kartel.id
  resource_group_name          = azurerm_resource_group.kartel.name
  revision_mode                = "Single"

  template {
    container {
      name   = "geocoding"
      image  = var.default_image
      cpu    = var.default_cpu
      memory = var.default_memory
    }
  }
}

resource "azurerm_container_app" "logistics" {
  name                         = "logistics"
  container_app_environment_id = azurerm_container_app_environment.kartel.id
  resource_group_name          = azurerm_resource_group.kartel.name
  revision_mode                = "Single"

  template {
    container {
      name   = "logistics"
      image  = var.default_image
      cpu    = var.default_cpu
      memory = var.default_memory
    }
  }
}

resource "azurerm_container_app" "property_market" {
  name                         = "property-market"
  container_app_environment_id = azurerm_container_app_environment.kartel.id
  resource_group_name          = azurerm_resource_group.kartel.name
  revision_mode                = "Single"

  template {
    container {
      name   = "property-market"
      image  = var.default_image
      cpu    = var.default_cpu
      memory = var.default_memory
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